using EHRMvcDemo.Models;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace EHRMvcDemo.Data
{
    public class EHRDbContext : DbContext
    {
        public EHRDbContext(DbContextOptions<EHRDbContext> options)
            : base(options)
        {
        }

        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        public DbSet<LabResult> LabResults { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>().ToTable("Patients", "Healthcare");
            modelBuilder.Entity<Doctor>().ToTable("Doctors", "Healthcare");
            modelBuilder.Entity<Appointment>().ToTable("Appointments", "Healthcare");
            modelBuilder.Entity<LabResult>().ToTable("LabResults", "Healthcare");
            modelBuilder.Entity<AuditLog>().ToTable("AuditLog", "Healthcare");

            base.OnModelCreating(modelBuilder);
        }
    }
}
