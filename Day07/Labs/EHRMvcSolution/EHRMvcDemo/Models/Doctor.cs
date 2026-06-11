using System.ComponentModel.DataAnnotations;

namespace EHRMvcDemo.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialty { get; set; }
        public string LicenseNumber { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string FullName => FirstName + " " + LastName;
    }
}
