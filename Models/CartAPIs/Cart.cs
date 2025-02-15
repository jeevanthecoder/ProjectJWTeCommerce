using System.ComponentModel.DataAnnotations;
using ProjectJWTeCommerce.Models.ProductAPIs;

namespace ProjectJWTeCommerce.Models.CartAPIs
{
    public class Cart
    {
        [Key]
        public int CId { get; set; }
        public decimal TotalCost { get; set; }

        public int userId { get; set; }         // Foreign Key

        public int addressId { get; set; }

        // Relationships
        public List<ItemQuantity> Items { get; set; }
    }
}
