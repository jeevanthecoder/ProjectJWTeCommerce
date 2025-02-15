using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ProjectJWTeCommerce.Models.UserAPIs
{
    [Index(nameof(UserEmail), IsUnique = true)]
    [Index(nameof(UserName), IsUnique = true)]
    public class UserDetails
    {
        [Key]
        public int UId { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string UserPassword { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]  // In-built email validation
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string UserPhone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int cartId { get; set; }                         // Foreign key
        public int sellerId { get; set; }                       // Foreign Key  

        // Relationship
        public List<Address>? Addresses { get; set; }
    }
}
