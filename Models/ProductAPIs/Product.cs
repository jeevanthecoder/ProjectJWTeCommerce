using System.ComponentModel.DataAnnotations;
using ProjectJWTeCommerce.Models.SellerAPIs;

namespace ProjectJWTeCommerce.Models.ProductAPIs
{
    public class Product
    {
        [Key]
        public int Pid { get; set; }
        public int SId { get; set; }    // Foreign Key to Seller
        public string PTitle { get; set; }
        public string PCategory { get; set; }
        public string PImageURL { get; set; }
        public string PDescription { get; set; }
        public string Image {  get; set; }
        public decimal PPrice { get; set; }
        public int Quantity { get; set; }


        // Relationships
        public Seller? seller { get; set; }
        public List<ItemQuantity>? itemQuantities { get; set; }
        public List<Features>? features { get; set; }
    }
}
