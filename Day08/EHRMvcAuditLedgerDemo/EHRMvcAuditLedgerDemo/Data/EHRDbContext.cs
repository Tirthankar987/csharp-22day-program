// Entity Framework Core — the engine that converts C# operations into SQL
// e.g., _context.PatientTransactions.Add(t) becomes INSERT INTO Healthcare.PatientTransactions
using Microsoft.EntityFrameworkCore;

// Our model classes — each one maps to a real table in SQL Server
// Without this, EF Core doesn't know what tables exist
using EHRMvcAuditLedgerDemo.Models;

namespace EHRMvcAuditLedgerDemo.Data
{
    // DbContext is EF Core's unit of work — it tracks every object you load,
    // add, or modify, and sends them all to SQL in one transaction on SaveChanges()
    // Real hospital: one DbContext per request (scoped lifetime registered in Program.cs)
    // If it were shared across requests, Patient A's nurse could accidentally
    // see Patient B's uncommitted changes in memory — a silent PHI leak
    public class EHRDbContext : DbContext
    {
        // DbContextOptions carries the connection string and provider (SQL Server)
        // injected by Program.cs at startup — this class never hardcodes the server name
        // Real hospital: the connection string itself comes from Azure Key Vault,
        // so even if this file is leaked in a breach, no DB credentials are exposed
        public EHRDbContext(DbContextOptions<EHRDbContext> options)
            : base(options) { }

        // DbSet<T> = a queryable, in-memory representation of a database table
        // Every LINQ query you write against this becomes a SQL SELECT
        // Every .Add() becomes a staged INSERT, committed only on SaveChangesAsync()
        // Real hospital: you'd also have DbSet<AuditLog>, DbSet<Patient>, DbSet<User>
        // and each would be restricted by row-level security in SQL Server
        // so a user can only query rows they're authorized to see
        public DbSet<PatientTransaction> PatientTransactions => Set<PatientTransaction>();

        // Separate DbSet for the immutable ledger table
        // Keeping it separate from PatientTransactions enforces the design intent:
        // operational data (can be corrected) vs. audit trail (append-only, never updated)
        // Real hospital: the ledger table would have a SQL Server DDL trigger
        // that blocks UPDATE and DELETE at the database level — app-level protection alone isn't enough
        public DbSet<TransactionLedger> TransactionLedger => Set<TransactionLedger>();

        // OnModelCreating runs ONCE at app startup to build the internal schema map
        // EF Core reads this to know exactly which SQL table each C# class points to
        // Without this, EF Core guesses table names from class names — and guesses wrong
        // when your tables live inside a named schema like "Healthcare"
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // "Healthcare" is a SQL Server schema — like a namespace for tables
            // Real hospital systems use schemas to separate concerns:
            // Healthcare.*, Billing.*, Pharmacy.*, Admin.* — each with its own
            // SQL permissions so a billing service account can't touch pharmacy tables
            modelBuilder.Entity<PatientTransaction>()
                .ToTable("PatientTransactions", "Healthcare")
                // Tells EF Core which column is the PRIMARY KEY
                // Without HasKey(), EF Core throws at runtime — it refuses to
                // track or save any entity that has no defined identity
                .HasKey(t => t.TransactionId);

            modelBuilder.Entity<TransactionLedger>()
                .ToTable("TransactionLedger", "Healthcare")
                .HasKey(l => l.LedgerId);

            // Real hospital — what you'd add here beyond this demo:
            // .HasIndex(t => t.PatientName)        → fast lookups by patient
            // .Property(t => t.Amount).HasPrecision(18, 2) → exact money storage
            // .HasQueryFilter(t => !t.IsDeleted)   → soft-delete, PHI is never hard-deleted
            // .Property(t => t.RowVersion).IsRowVersion() → concurrency protection
            // so two nurses updating the same record don't silently overwrite each other
        }
    }
}

