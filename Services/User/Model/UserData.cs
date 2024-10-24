using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShopNest.Services.User.Model
{
    public class UserData
    {
        [Key]
        [JsonPropertyName("UserID")]
        public int UserID { get; set; }

        [JsonPropertyName("FirstName")]
        public string? FirstName { get; set; }  // Made nullable

        [JsonPropertyName("LastName")]
        public string? LastName { get; set; }   // Made nullable

        [JsonPropertyName("EmailOrPhone")]
        public string? EmailOrPhone { get; set; }      // Made nullable

        //[JsonPropertyName("PhoneNumber")]
        //public string? PhoneNumber { get; set; } // Made nullable

        [JsonPropertyName("Password")]
        public string? Password { get; set; }   // Made nullable
    }
}
