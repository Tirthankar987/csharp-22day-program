using System.ComponentModel.DataAnnotations;

namespace EHRMvcDemo.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        public Guid PatientGuid { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;

        public string? SSN { get; set; } // PHI – never displayed
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        public string EmergencyContactName { get; set; } = string.Empty;
        public string EmergencyContactPhone { get; set; } = string.Empty;

        public bool? IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string FullName => FirstName + " " + LastName;
    }

}
