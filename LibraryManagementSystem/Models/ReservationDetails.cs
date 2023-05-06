using LibraryManagementSystem.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public enum ReservationStatus
    {
        Reserved,
        Issued,
        Returned,
        Rejected
    }
    public class ReservationDetails
    {
        [Key]
        public int ReservationId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ReservedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? IssuedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ReturnDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ReturnedDate { get; set; }
        public LMSUser ReservedUser { get; set; }
        public BookDetails Book { get; set; }
        public ReservationStatus ReservationStatus { get; set; } = ReservationStatus.Reserved;
    }
}
