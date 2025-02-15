using System.ComponentModel.DataAnnotations;

namespace ProjectJWTeCommerce.Models.ProductAPIs
{
    public class Features
    {
        [Key]
        public int FId { get; set; }
        public int PId { get; set; }        // Foreign key to Product
        public string FName { get; set; }


        // Relationship
        public Product? product { get; set; }
    }
}
