namespace ECommerce.Models.ViewModel
{
    public class KhaltiPaymentInitVM
    {
        public decimal Amount { get; set; }
        public string RedirectUrl { get; set; }
        public string FullName { get; set; }  // Merchant Full Name
        public string Email { get; set; }     // Merchant Email
        public string Address { get; set; }   // Merchant Address
        public string PurchaseOrderId { get; set; } // Order ID
    }
}
