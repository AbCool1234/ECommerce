using ECommerce.DAO;
using ECommerce.Models;
using ECommerce.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Runtime.Intrinsics.X86;

namespace ECommerce.Controllers
{
    public class ProductController : Controller
    {

        ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }



        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Pay(int id)
        {
            var orderMaster = _context.ProductOrderMaster.FirstOrDefault(o => o.ProductOrderMasterID == id);
            if (orderMaster == null)
            {
                return NotFound("Order not found.");
            }
            var orderDetails = _context.ProductOrderDetail
                                 .Where(d => d.ProductOrderMasterID == id)
                                 .Select(d => new ProductOrderDetailVM
                                 {
                                     ProductOrderDetailID = d.ProductOrderDetailID,
                                     ProductItemId = d.ProductItemId,
                                     ProductName = d.ProductItem.ProductName, // Get ProductName from ProductItem
                                     UnitPrice = d.UnitPrice,
                                     Quantity = d.Quantity,
                                     TotalPrice = d.UnitPrice * d.Quantity,
                                     ItemCount = d.Quantity
                                 })
                                 .ToList();

            var orderVM = new OrderVM
            {
                mast = orderMaster,
                dtls = orderDetails
            };

            return View(orderVM);
            //var mast = _context.ProductOrderMaster
            //        .Where(x => x.ProductOrderMasterID == id)
            //        .FirstOrDefault();

            //return View(mast);
        }




        [HttpPost]
        public JsonResult SaveOrder([FromBody] OrderVM vm)
        {
            if (string.IsNullOrEmpty(vm.mast.Fullname))
            {
                return Json(new
                {
                    Success = false,
                    Message = "Enter Fullname"
                });
            }
            else if (string.IsNullOrEmpty(vm.mast.MobileNo))
            {
                return Json(new
                {
                    Success = false,
                    Message = "Enter Mobile No"
                });
            }
            else if (string.IsNullOrEmpty(vm.mast.Email))
            {
                return Json(new
                {
                    Success = false,
                    Message = "Enter Email"
                });
            }
            else if (string.IsNullOrEmpty(vm.mast.Address))
            {
                return Json(new
                {
                    Success = false,
                    Message = "Enter Your Address"
                });
            }
            else if (vm.dtl.Count == 0)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Item Not Found"
                });
            }
            else
            {

                //first save in master
                ProductOrderMaster m = new ProductOrderMaster()
                {
                    Address = vm.mast.Address,
                    Email = vm.mast.Email,
                    Fullname = vm.mast.Fullname,
                    MobileNo = vm.mast.MobileNo,
                    OrderDate = DateTime.Now,
                    GrandTotal = vm.dtl.Sum(s => s.Quantity * s.UnitPrice),
                    PaymentStatus="pending",
                    Tid=""
                };


                _context.ProductOrderMaster.Add(m);
                _context.SaveChanges();

                //now save in detail
                List<ProductOrderDetail> d = new List<ProductOrderDetail>();
                foreach (var item in vm.dtl)
                {
                    d.Add(new ProductOrderDetail
                    {
                        Id = 0,
                        ProductItemId = item.Id,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity,
                        ProductOrderMasterID = m.ProductOrderMasterID,
                    });
                }


                _context.ProductOrderDetail.AddRange(d);
                _context.SaveChanges();


                return Json(new
                {
                    Success = true,
                    Message = "Order Placed Successfully",
                    Pk = m.ProductOrderMasterID
                });

            }
        }
    }
}
