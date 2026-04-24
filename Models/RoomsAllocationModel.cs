using System;

namespace AVADH_PRIME_Consume.Models
{
    public class RoomsAllocationModel
    {
        public int Allocation_Id { get; set; }

        public int Student_Id { get; set; }
        public int Bed_No { get; set; }

        public DateTime Allocation_Date { get; set; }
        public DateTime Vacate_Date { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}