using System.ComponentModel.DataAnnotations;

namespace EHRMvcDemo.Models
{
    public class AuditLog
    {
        [Key]
        public int AuditId { get; set; }

        public string UserId { get; set; }
        public string Action { get; set; }
        public string TableName { get; set; }
        public int RecordId { get; set; }

        public int? PatientId { get; set; }

        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }

        public DateTime AccessDate { get; set; }
        public string? Details { get; set; }
    }
}

