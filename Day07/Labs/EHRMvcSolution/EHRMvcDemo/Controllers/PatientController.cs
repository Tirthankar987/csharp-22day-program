// Required to build MVC controllers and return views
using Microsoft.AspNetCore.Mvc;

// Required for async database operations like ToListAsync()
using Microsoft.EntityFrameworkCore;

// Gives access to our EHRDbContext (database connection)
using EHRMvcDemo.Data;

namespace EHRMvcDemo.Controllers
{
    // This class is a CONTROLLER in MVC
    // Controller = handles incoming HTTP requests
    public class PatientsController : Controller
    {
        // Private field to hold the database context
        // readonly = cannot be reassigned after constructor
        private readonly EHRDbContext _context;

        // Constructor is called automatically by ASP.NET Core
        // Dependency Injection provides EHRDbContext here
        public PatientsController(EHRDbContext context)
        {
            // Store the injected DbContext for later use
            _context = context;
        }

        // This method handles HTTP GET requests to:
        // /Patients  OR  /Patients/Index
        // IActionResult allows returning View, Redirect, Json, etc.
        public async Task<IActionResult> Index()
        {
            // Query the Patients table from the database
            // Where(p => p.IsActive) applies the "Minimum Necessary" rule (HIPAA)
            // ToListAsync() executes the SQL query asynchronously
            var patients = await _context.Patients
                .Where(p => p.IsActive==true)
                .ToListAsync();

            // Create a new audit log entry
            // HIPAA requirement: all access to patient data must be logged
            _context.AuditLogs.Add(new Models.AuditLog
            {
                // In real systems, this comes from the logged-in user
                UserId = "DemoUser",

                // Action performed on the data
                Action = "View",

                // Name of the table accessed
                TableName = "Patients",

                // UTC time used for compliance and consistency
                AccessDate = DateTime.UtcNow,

                // Additional context about what happened
                Details = "Viewed patient list"
            });

            // Persist both:
            // 1. Audit log entry
            // 2. Any pending database changes
            await _context.SaveChangesAsync();

            // Send the patient list to the View
            // MVC rule: Controller sends data, View decides how to display it
            return View(patients);
        }
    }
}
