using Newtonsoft.Json;

namespace LibraryManagementSystem.Models
{
    public class OpenLibBook
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author_name")]
        public List<string> Authors { get; set; }//= new List<string>() { "Not Found"};

        [JsonProperty("cover_i")]
        public int CoverId { get; set; }
    }
}
