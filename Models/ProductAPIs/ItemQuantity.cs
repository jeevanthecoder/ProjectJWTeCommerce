using System.ComponentModel.DataAnnotations;
using ProjectJWTeCommerce.Models.CartAPIs;

namespace ProjectJWTeCommerce.Models.ProductAPIs
{
    public class ItemQuantity
    {
        [Key]
        public int QId { get; set; }
        public int? CId { get; set; }    //Foreign key to cart
        public int PId { get; set; }    //Foreign key to Product
        public int Quantity { get; set; }


        //Relationships
        public Cart? cart { get; set; }
        public Product? product { get; set; }

    }
}
