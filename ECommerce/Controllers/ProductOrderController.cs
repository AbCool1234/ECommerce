using ECommerce.DAO;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    public class ProductOrderController : Controller
    {
        ApplicationDbContext _context;
        public ProductOrderController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<ProductOrderMasterVM> orders = _context
                                                .ProductOrderMaster
                                                .Select(s => new ProductOrderMasterVM
                                                {
                                                    Address = s.Address,
                                                    Email = s.Email,
                                                    Fullname = s.Fullname,
                                                    GrandTotal = s.GrandTotal,
                                                    MobileNo = s.MobileNo,
                                                    OrderDate = s.OrderDate,
                                                    ProductOrderMasterID = s.ProductOrderMasterID,
                                                    ItemCount = _context
                                                                    .ProductOrderDetail
                                                                    .Where(w => w.ProductOrderMasterID == s.ProductOrderMasterID)
                                                                    .Count(),
                                                    PaymentStatus = s.PaymentStatus,
                                                    Tid = s.Tid,

                                                    // Get the list of ProductOrderDetailVM with ProductName included
                                                    ItemDetails = _context.ProductOrderDetail
                                                                        .Where(w => w.ProductOrderMasterID == s.ProductOrderMasterID)
                                                                        .Select(d => new ProductOrderDetailVM
                                                                        {
                                                                            ProductName = d.ProductItem.ProductName,  // Assuming 'ProductItem' has a 'Name' field
                                                                            ProductItemId = d.ProductItemId,
                                                                            UnitPrice = d.UnitPrice,
                                                                            Quantity = d.Quantity,
                                                                            TotalPrice = d.UnitPrice * d.Quantity
                                                                        })
                                                                        .ToList()
                                                })
                                                .ToList();
            return View(orders);
        }

        [HttpGet]
        public IActionResult GetOrderItems(int id)
        {
            // Fetch the items for the given ProductOrderMasterID
            var orderItems = _context.ProductOrderDetail
                                    .Where(d => d.ProductOrderMasterID == id)
                                    .Select(d => new
                                    {
                                        ProductName = d.ProductItem.ProductName,  // Assuming 'ProductItem' has 'Name'
                                        d.Quantity,
                                        d.UnitPrice
                                    })
                                    .ToList();

            return Json(orderItems);  // Return as JSON for AJAX to process
        }



    }
}
