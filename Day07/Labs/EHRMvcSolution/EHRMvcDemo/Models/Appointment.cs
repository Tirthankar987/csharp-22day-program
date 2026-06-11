using System.ComponentModel.DataAnnotations;

namespace EHRMvcDemo.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        public DateTime AppointmentDate { get; set; }
        public int DurationMinutes { get; set; }

        // Required medical info
        public string ReasonForVisit { get; set; } = string.Empty;

        // Scheduled / Completed / Cancelled
        public string Status { get; set; } = "Scheduled";

        public string? Notes { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

}

