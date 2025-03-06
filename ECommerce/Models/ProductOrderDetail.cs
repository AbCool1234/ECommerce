using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    public class ProductOrderDetail
    {
        [Key]
        public int ProductOrderDetailID { get; set; }
        public int ProductOrderMasterID { get; set; }
        public int Id { get; set; }

        public int ProductItemId { get; set; }
     
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }



        [ForeignKey("ProductOrderMasterID")]
        public virtual ProductOrderMaster ProductOrderMaster { get; set; }

        [ForeignKey("ProductItemId")]
        public virtual ProductItem ProductItem { get; set; }



    }

    public class ProductOrderDetailVM
    {
        [Key]
        public int ProductOrderDetailID { get; set; }
        public string ProductName { get; set; }
        public int ProductItemId { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public int ItemCount { get; set; }


        [ForeignKey("ProductOrderMasterID")]
        public virtual ProductOrderMaster ProductOrderMaster { get; set; }

        [ForeignKey("ProductItemId")]
        public virtual ProductItem ProductItem { get; set; }
    }
}
