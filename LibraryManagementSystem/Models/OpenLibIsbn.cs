using Newtonsoft.Json;
namespace LibraryManagementSystem.Models
{
    public class OpenLibIsbn
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author_name")]
        public List<string> Authors { get; set; }//= new List<string>() { "Not Found"};

        [JsonProperty("cover_i")]
        public int CoverId { get; set; }

        [JsonProperty("isbn")]
        public List<string> Isbn { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("publisher")]
        public List<string> Publication { get; set; }
    }
}
