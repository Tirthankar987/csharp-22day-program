// MVC base classes: Controller, IActionResult, [HttpPost], RedirectToAction, ModelState
using Microsoft.AspNetCore.Mvc;

// Our EF Core gateway — the single class that talks to SQL Server
using EHRMvcAuditLedgerDemo.Data;

// C# representations of our two DB tables
using EHRMvcAuditLedgerDemo.Models;

// SHA-256 hashing service — the tamper detection engine
using EHRMvcAuditLedgerDemo.Services;

// Forces "." as decimal separator regardless of server region — critical for hash consistency
using System.Globalization;

namespace EHRMvcAuditLedgerDemo.Controllers
{
    public class TransactionController : Controller
    {
        // Private + readonly = this reference is set once at construction and never swapped
        // Real hospital: if this were not readonly, a rogue middleware could replace the DB
        // connection mid-request and redirect writes to an attacker-controlled database
        private readonly EHRDbContext _context;

        // ASP.NET Core's DI container calls this automatically — you never write "new TransactionController()"
        // The framework creates one instance per HTTP request, injects a scoped EHRDbContext, then destroys both
        // Real hospital: scoped lifetime is mandatory — a singleton DbContext means
        // two concurrent users share the same change tracker, risking cross-patient data leaks
        public TransactionController(EHRDbContext context)
        {
            _context = context;
        }

        // GET /Transaction/Create — renders the blank form, zero logic
        // No [HttpGet] needed — GET is the default; only POST/PUT/DELETE need explicit attributes
        public IActionResult Create()
        {
            return View();
        }

        // POST /Transaction/Create — the entire save pipeline lives here
        // async because DB calls are I/O-bound; awaiting them frees the thread to serve other requests
        // Real hospital: a synchronous SaveChanges() on a busy EHR blocks threads
        // under load and can bring the entire hospital's billing system to a crawl
        [HttpPost]
        public async Task<IActionResult> Create(PatientTransaction transaction)
        {
            // MODEL BINDING already happened before this line
            // ASP.NET Core parsed "PatientName=John&Amount=1500&..." from the request body
            // and populated the 'transaction' object automatically — zero manual parsing needed

            // All [Required], [Range], [AllowedTransactionType], [FutureOrTodayDate] rules
            // have already fired during model binding — this check reads their combined result
            // Real hospital: NEVER skip this check and go straight to saving
            // Skipping it means a $0 charge, a blank patient name, or a backdated
            // billing entry can silently enter the database and corrupt financial records
            if (!ModelState.IsValid)
            {
                // Return the form WITH the user's input still filled in
                // They fix only the broken fields — they don't retype everything
                // Real hospital: validation errors are also written to an audit log
                // so compliance teams can detect patterns (e.g., repeated backdating attempts)
                return View(transaction);
            }

            // Everything below can fail: SQL Server down, network timeout, disk full
            // Without try/catch the user sees a raw ASP.NET error page with stack traces,
            // table names, and connection strings — a textbook HIPAA breach
            try
            {
                // .Add() stages the row in EF Core's change tracker — nothing hits SQL yet
                // SaveChangesAsync() flushes the INSERT and populates TransactionId (IDENTITY column)
                // TransactionId is needed in the next step to compute the hash — order matters
                _context.PatientTransactions.Add(transaction);
                await _context.SaveChangesAsync();

                // Fetch the CurrentHash of the most recently saved ledger entry
                // This becomes the PreviousHash of the entry we're about to create
                // ?? "GENESIS" = first-ever entry has no predecessor, so we use a known starting string
                // Real hospital: "GENESIS" would be replaced with a cryptographically signed
                // root hash generated at system initialisation and stored in an HSM (Hardware Security Module)
                var previousHash =
                    _context.TransactionLedger
                        .OrderByDescending(l => l.LedgerId)
                        .Select(l => l.CurrentHash)
                        .FirstOrDefault() ?? "GENESIS";

                // Do not skip normalisation — 1000 vs 1000.0 vs 1000.00 are identical numbers
                // but three different strings that produce three completely different SHA-256 hashes
                // Real hospital: this format spec is treated as a versioned protocol
                // change it once and every single historical hash becomes unverifiable forever
                var normalizedAmount =
                    transaction.Amount.ToString("F2", CultureInfo.InvariantCulture);

                // Concatenate 4 fields and hash the result into a 64-char hex fingerprint
                // Order is a contract — VerifyChain() must use the EXACT same order
                // Swapping any two fields makes every verification fail on untampered data
                // Real hospital: you'd also HMAC this with a secret key from Azure Key Vault
                // so even knowing the algorithm, an attacker cannot forge a valid hash
                var currentHash = LedgerHashService.Compute(
                    transaction.TransactionId +
                    transaction.PatientName +
                    normalizedAmount +
                    previousHash
                );

                // Write the immutable audit record — this table is append-only by design
                // Amount here is a SNAPSHOT: if someone later runs
                // UPDATE PatientTransactions SET Amount = 999999, this field stays as the original
                // That mismatch is exactly what CHECK 2 in Verify() detects
                // Real hospital: a SQL DDL trigger blocks UPDATE and DELETE on this table
                // at the database level — application-layer protection alone isn't sufficient for HIPAA
                _context.TransactionLedger.Add(new TransactionLedger
                {
                    TransactionId = transaction.TransactionId,
                    Amount = transaction.Amount,
                    PreviousHash = previousHash,
                    CurrentHash = currentHash
                });

                await _context.SaveChangesAsync();

                // PRG pattern: Post → Redirect → Get
                // Without this, pressing F5 replays the POST and creates a duplicate billing entry
                // Real hospital: duplicate charges are an insurance fraud liability
                // PRG is a compliance safeguard, not just a UX nicety
                return RedirectToAction("Success");
            }
            catch
            {
                // Never expose exception details to the user — stack traces reveal
                // table names, column names, server paths, and ORM internals
                // Real hospital: before redirecting, write the full exception to a
                // HIPAA-compliant audit log (not stdout, not a browser response)
                // HIPAA requires you to detect AND record system failures — silent swallowing is not compliant
                return RedirectToAction("Error", "Home");
            }
        }

        // Confirmation page after a successful save — no logic, no PHI displayed
        // Real hospital: even this page requires an authenticated session
        // An unauthenticated user reaching /Transaction/Success is a session management failure
        public IActionResult Success()
        {
            return View();
        }

        // GET /Transaction/Verify — walks every ledger row and runs two independent checks
        // Real hospital: this would run on an automated nightly schedule, not just on user request
        // HIPAA requires proactive integrity monitoring — waiting for a user to click Verify is not enough
        public IActionResult Verify()
        {
            // Load all ledger rows oldest-first — order is non-negotiable
            // Each entry's PreviousHash references the entry before it
            // Walking out of order breaks the chain reconstruction entirely
            var ledgers = _context.TransactionLedger
                .OrderBy(l => l.LedgerId)
                .ToList();

            var results = new List<string>();

            for (int i = 0; i < ledgers.Count; i++)
            {
                var ledger = ledgers[i];

                // Load the LIVE operational row — needed to detect post-recording modifications
                // Real hospital: use .FirstOrDefault() here and handle null
                // A missing transaction for an existing ledger entry is itself a tampering signal
                var transaction = _context.PatientTransactions
                    .First(t => t.TransactionId == ledger.TransactionId);

                // First entry must match "GENESIS" — any other value means the chain was injected into
                // All other entries must match the CurrentHash of the immediately preceding row
                // A gap between LedgerIds here (1, 2, 4 — where is 3?) is also a red flag worth flagging
                var expectedPreviousHash =
                    i == 0 ? "GENESIS" : ledgers[i - 1].CurrentHash;

                // CHECK 1 — LEDGER INTEGRITY
                // Recomputes hash from current data using ledger.Amount (snapshot, not live)
                // If the ledger row itself was modified in SQL, the recomputed hash won't match storedHash
                // Real hospital: false here triggers a HIPAA Breach Assessment
                // You have 60 days to notify affected patients if PHI was compromised
                bool ledgerIntact = LedgerHashService.VerifyChain(
                    transaction.TransactionId,
                    transaction.PatientName,
                    ledger.Amount,        // snapshot — deliberately NOT the live amount
                    expectedPreviousHash,
                    ledger.CurrentHash
                );

                // CHECK 2 — POST-RECORDING DATA MODIFICATION
                // Ledger hash can be perfectly valid yet the live amount still differs
                // This catches: UPDATE PatientTransactions SET Amount = X after original save
                // The two checks are independent — you need both to catch both attack vectors
                bool dataModified = transaction.Amount != ledger.Amount;

                // Ledger tamper outranks data tamper in severity — direct hash chain attack vs. field edit
                // Real hospital: both statuses trigger different incident response workflows
                // "LEDGER TAMPERED" escalates to the CISO; "DATA TAMPERED" goes to the billing compliance team
                string status =
                    !ledgerIntact ? "LEDGER TAMPERED"
                    : dataModified ? "DATA TAMPERED AFTER RECORDING"
                    : "VALID";

                results.Add(
                    $"Ledger {ledger.LedgerId} (Transaction {ledger.TransactionId}): {status}"
                );
            }

            // Passes List<string> to Verify.cshtml — the view renders each as a list item
            return View(results);
        }
    }
}

