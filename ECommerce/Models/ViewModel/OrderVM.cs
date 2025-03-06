namespace ECommerce.Models.ViewModel
{
    public class OrderVM
    {
        public ProductOrderMaster mast { get; set; }
        public List<ProductOrderDetail> dtl { get; set; }
        public List<ProductOrderDetailVM> dtls { get; set; }
    }
}
