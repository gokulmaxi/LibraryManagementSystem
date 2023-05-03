using LibraryManagementSystem.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class FineDetails
    {
        [Key]
        public int FineId { get; set;}
        public int FineAmount { get; set;}
        public LMSUser User { get; set; }
        public ReservationDetails Reservation { get; set; }
        public bool Paid { get; set; } = false;
    }
}
