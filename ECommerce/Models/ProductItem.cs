using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public class ProductItem
    {
        [Key]
        public int ProductItemId { get; set; }
        public string ProductName { get; set; } 
        public string ProductCode { get; set; }
        public int CategoryId { get; set; }
        public string description { get; set; }
        public decimal UnitPrice { get; set; }
        public string Thumbnail { get; set; }
    }
}
