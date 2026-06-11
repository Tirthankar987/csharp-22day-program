using EHRMvcDemo.Data;
using EHRMvcDemo.Models;
using EHRMvcDemo.Data;
using EHRMvcDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EHRMvcCleanDemo.Controllers
{
    // Handles appointment creation
    public class AppointmentController : Controller
    {
        private readonly EHRDbContext _context;

        public AppointmentController(EHRDbContext context)
        {
            _context = context;
        }

        // GET: Appointment/Create
        public IActionResult Create()
        {
            ViewBag.Doctors = new SelectList(_context.Doctors, "DoctorId", "FullName");
            ViewBag.Patients = new SelectList(_context.Patients, "PatientId", "FullName");
            return View();
        }

        // POST: Appointment/Create
        [HttpPost]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (!ModelState.IsValid)
                return View(appointment);

            _context.Appointments.Add(appointment);

            // HIPAA audit
            _context.AuditLogs.Add(new AuditLog
            {
                UserId = "DemoUser",
                Action = "Create",
                TableName = "Appointments",
                RecordId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                AccessDate = DateTime.UtcNow,
                Details = "Created appointment"
            });

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Doctor");
        }
    }
}

