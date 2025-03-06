using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }    
        public string CategoryCode { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
