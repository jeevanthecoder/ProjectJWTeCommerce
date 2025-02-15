using System.ComponentModel.DataAnnotations;

namespace ProjectJWTeCommerce.Models.UserAPIs
{
    public class Address
    {
        [Key]
        public int AId { get; set; }
        public int userId { get; set; } // Foreign Key
        public string FullName  { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string Phone { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Pincode should be of 6 digits.")]
        public string Pincode { get; set; }
        public string Landmark { get; set; }
        public string HouseDetails { get; set; }
        public string StreetDetails { get; set; }

        public int cartId { get; set; }               // Foreign Key

        // Relationship
        public UserDetails? user { get; set; }

    }
}
