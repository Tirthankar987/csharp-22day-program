// DataAnnotations gives us [Key] to mark the primary key
// EF Core refuses to manage any table without a defined primary key — it can't track rows without an identity
using System.ComponentModel.DataAnnotations;

namespace EHRMvcAuditLedgerDemo.Models
{
    // This is NOT an operational table — it's a tamper-evident audit trail
    // Think of it as a sealed envelope: once written, it should never be opened and rewritten
    // Real hospital: this table would be APPEND-ONLY enforced at SQL level via DDL trigger
    // blocking any UPDATE or DELETE — app-level protection alone is not enough for HIPAA audit trails
    public class TransactionLedger
    {
        // Auto-incremented by SQL Server — we never set this manually
        // Sequential LedgerIds matter: the Verify() method walks entries in LedgerId order
        // to reconstruct the chain — a gap in IDs is itself a red flag worth investigating
        [Key]
        public int LedgerId { get; set; }

        // Foreign key pointing back to Healthcare.PatientTransactions
        // This ties every audit record to exactly one transaction — no orphan ledger entries
        // Real hospital: this would have a SQL FOREIGN KEY constraint + a composite index
        // so looking up "all audit records for Transaction #1042" is fast, not a full table scan
        public int TransactionId { get; set; }

        // This is the most important field in this entire class
        // It is a SNAPSHOT of the amount at the exact moment of recording
        // If someone later runs UPDATE PatientTransactions SET Amount = 999999,
        // this field stays unchanged — and the mismatch is exactly what Verify() detects
        // Real hospital: store in decimal(18,2) in SQL — never float or money type
        // Float loses precision on large amounts; money type has rounding quirks with calculations
        public decimal Amount { get; set; }

        // The hash of the ledger entry that came before this one
        // This is what makes it a chain — each entry is mathematically linked to its predecessor
        // If anyone deletes a middle entry, the chain breaks and Verify() catches it instantly
        // First entry stores "START" here — the agreed starting point with no predecessor
        public string PreviousHash { get; set; } = string.Empty;

        // SHA-256 fingerprint of: TransactionId + PatientName + Amount + PreviousHash
        // Changing ANY of those four inputs produces a completely different hash
        // Real hospital: this hash is also what you'd store on an external immutable ledger
        // (Azure Immutable Blob Storage, AWS WORM) for a second layer of tamper evidence
        public string CurrentHash { get; set; } = string.Empty;

        // Nullable DateTime — SQL DEFAULT GETUTCDATE() sets this automatically on INSERT
        // UTC is non-negotiable in healthcare: a timestamp in "IST" or "EST" is ambiguous
        // across time zones and daylight saving changes; UTC is always unambiguous
        // Real hospital: this column would be non-nullable with a server-side default —
        // never let the application layer set the audit timestamp, SQL Server sets it
        public DateTime? CreatedDate { get; set; }
    }
}

