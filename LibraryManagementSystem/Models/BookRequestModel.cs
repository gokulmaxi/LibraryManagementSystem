using LibraryManagementSystem.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class BookRequestModel : BookDetails
    {
        [Key] public string RequestId{ get; set; }
        public LMSUser RequestedBy { get; set; }
        public bool IsAdded { get; set; }
    }
}
