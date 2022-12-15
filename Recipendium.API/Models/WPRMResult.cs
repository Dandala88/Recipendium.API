using System.Text.Json.Serialization;

namespace Recipendium.API.Models
{
    public class WPRMResult
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("html")]
        public string Html { get; set; }
    }
}
