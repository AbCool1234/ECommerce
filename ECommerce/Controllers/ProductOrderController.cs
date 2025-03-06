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
                                                                    .Count()
                                                })
                                                .ToList();
            return View(orders);
        }
    }
}
