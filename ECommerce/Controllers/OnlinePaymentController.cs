using ECommerce.DAO;
using ECommerce.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ECommerce.Controllers
{
    public class OnlinePaymentController : Controller
    {
        private ApplicationDbContext _context;

        public OnlinePaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Success(string pidx,
                            string transaction_id,
                            string tidx,
                            int amount,
                            string purchase_order_id)
        {
            int orderId = int.Parse(purchase_order_id);
            var data = _context.ProductOrderMaster.Where(x => x.ProductOrderMasterID == orderId).FirstOrDefault();
            data.Tid = transaction_id;
            data.PaymentStatus = "payed";
            _context.SaveChanges();
            return View();
        }




        [HttpPost]
        [Route("KhaltiPaymentInitiate")]
        public async Task<object> KhaltiPaymentInitiate([FromBody] KhaltiPaymentInitVM vm)
        {

            string khalti_pri_key = "b7a497bf84fa4ca7b034bba23c2709ae";

            //var url = "https://khalti.com/api/v2/epayment/initiate/";

            var url = "https://dev.khalti.com/api/v2/epayment/initiate/";

            var payload = new
            {
                return_url = vm.RedirectUrl + "/OnlinePayment/Success",
                website_url = vm.RedirectUrl,
                amount = vm.Amount * 100,
                purchase_order_id = vm.PurchaseOrderId,
                purchase_order_name = Guid.NewGuid().ToString(),
                merchant_info = new
                {
                    name = "ramesh",
                    email = "ramesh@gmail.com"
                },
                customer_info = new
                {
                    name = "Himalayan Colz Ecom",
                    address = "Lalitpur "
                },
            };



            var jsonPayload = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", "key " + khalti_pri_key);

            var response = await client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return Ok(new
            {
                Success = response.StatusCode == HttpStatusCode.OK,
                Message = "OK",
                Data = responseContent
            });
        }
        //}
    }
}