using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    public enum BookCategory
    {
        Fiction,
        Fantasy,
        ScienceFiction,
        Romance,
        Horror,
        Thriller,
        Biography,
        SelfHelp,
        HealthAndFitness,
        History
    }
    public class BookDetails
    {
        [Key]
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public BookCategory Category { get; set; }
        public string Author { get; set; }
        public string Publication { get; set; }
        public int BookEdition { get; set; }
        public int Price { get; set; }
        public int RackNo { get; set;}
        public int CoverId { get; set; }
    }
}
