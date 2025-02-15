using System.ComponentModel.DataAnnotations;
using ProjectJWTeCommerce.Models.ProductAPIs;

namespace ProjectJWTeCommerce.Models.SellerAPIs
{
    public class Seller
    {
        [Key]
        public int SId { get; set; }
        public string SellerName { get; set; }
        public string SellerPhone { get; set; }
        public string SellerEmail { get; set; }
        public int NoOfProducts { get; set; }
        public int userId { get; set; }

        // Relationship
        public List<Product> Products { get; set; }
    }
}
