using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ShopNest.Services.User.Model
{
    public class UserDetails
    {
        [Key]
        [JsonPropertyName("UserID")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters.")]
        [JsonPropertyName("LastName")]
        public string LastName { get; set; }

        // Combined field for email or phone
        [Required(ErrorMessage = "Email or Phone is required.")]
        [JsonPropertyName("EmailOrPhone")]
        public string EmailOrPhone { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, ErrorMessage = "Password cannot exceed 50 characters.")]
        [JsonPropertyName("Password")]
        public string Password { get; set; }



    }
}
