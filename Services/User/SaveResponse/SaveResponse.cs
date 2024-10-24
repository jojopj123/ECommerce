using Newtonsoft.Json;

namespace ShopNest.Services.User.Controller
{
    public class SaveResponse
    {
        [JsonProperty(PropertyName = "Saved")]
        public Boolean Saved { get; set; }
        [JsonProperty(PropertyName = "RecordsAffected")]
        public int RecordsAffected { get; set; }
        [JsonProperty(PropertyName = "Message")]
        public string? Message { get; set; }

        [JsonProperty(PropertyName = "ErrorMessage")]
        public string? ErrorMessage { get; set; }

        [JsonProperty(PropertyName = "ID")]
        public int? ID { get; set; }
    }
}