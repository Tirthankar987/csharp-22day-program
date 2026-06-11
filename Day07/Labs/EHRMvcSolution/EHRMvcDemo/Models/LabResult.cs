using System.ComponentModel.DataAnnotations;

namespace EHRMvcDemo.Models
{
    public class LabResult
    {
        [Key]
        public int LabResultId { get; set; }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        public string TestName { get; set; }
        public string TestCategory { get; set; }

        public DateTime TestDate { get; set; }
        public string Result { get; set; }

        public string? ReferenceRange { get; set; }
        public string? Unit { get; set; }

        public string Status { get; set; }
        public bool IsAbnormal { get; set; }

        public string? TechnicianNotes { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }
}

