using System.ComponentModel.DataAnnotations;

namespace AVADH_PRIME_Consume.Models
{
    public class AttendanceModel
    {
        [Required]
        public int Attendance_Id { get; set; }

        // Foreign Key
        [Required]
        public int Student_Id { get; set; }

        // Attendance Info
        [Required]
        public DateTime Attendance_Date { get; set; }

        [Required]
        public string Status { get; set; }

        public TimeSpan? CheckIn_Time { get; set; }
        public TimeSpan? CheckOut_Time { get; set; }

        public string? Remarks { get; set; }

        // Tracking
        public int? Marked_By { get; set; }

        // Audit Fields
        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
