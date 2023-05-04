using LibraryManagementSystem.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class BookRequestModel 
    {
        [Key]
        public int RequestId { get; set; }
        public string BookTitle { get; set; }
        public BookCategory Category { get; set; }
        public string Author { get; set; }
        public string Publication { get; set; }
        public int BookEdition { get; set; }
        public int Price { get; set; }
        public int RackNo { get; set;}
        public int CoverId { get; set; }
        public LMSUser RequestedBy { get; set; }
        public bool IsAdded { get; set; }
    }
}
