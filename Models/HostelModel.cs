using System.ComponentModel.DataAnnotations;

namespace AVADH_PRIME_Consume.Models
{
    public class HostelModel
    {
        public int Hostel_Id { get; set; }

        // Basic Information
        [Required]
        public string Hostel_Name { get; set; }

        [Required]
        public string Hostel_Type { get; set; }  // Boys / Girls / Co-ed

        // Capacity Details
        [Required]
        [Range(1, int.MaxValue)]
        public int Total_Rooms { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Total_Beds { get; set; }

        // Location Details
        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Pincode { get; set; }

        // Contact Information
        [Required]
        [Phone]
        public string Contact_No { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // Fees
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Monthly_Rent { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Security_Deposit { get; set; }

        // Status
        public bool IsActive { get; set; } = true;

        // Audit Fields
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
