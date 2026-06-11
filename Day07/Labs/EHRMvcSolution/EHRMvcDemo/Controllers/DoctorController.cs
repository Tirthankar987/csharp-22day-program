using EHRMvcDemo.Data;
using EHRMvcDemo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EHRMvcCleanDemo.Controllers
{
    // Handles Doctor directory
    public class DoctorController : Controller
    {
        private readonly EHRDbContext _context;

        public DoctorController(EHRDbContext context)
        {
            _context = context;
        }

        // GET: /Doctor
        public async Task<IActionResult> Index()
        {
            // Minimum necessary rule
            var doctors = await _context.Doctors
                .Where(d => d.IsActive == true)
                .ToListAsync();

            return View(doctors);
        }

        // GET: /Doctor/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.DoctorId == id);

            if (doctor == null)
                return NotFound();

            return View(doctor);
        }
    }
}
