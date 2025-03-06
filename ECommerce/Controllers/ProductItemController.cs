using ECommerce.DAO;
using ECommerce.Models.ViewModel;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    //public class ProductItemController : Controller
    //{
    //    public IActionResult Index()
    //    {
    //        return View();
    //    }
    //}
    public class ProductItemController : Controller
    {
        private ApplicationDbContext _context;

        public ProductItemController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public JsonResult SaveOrder([FromBody] OrderVM vm)
        {
            if (vm == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Invalid order data"
                });
            }
            if (string.IsNullOrEmpty(vm.mast.Fullname))
            {
                return Json(new
                {
                    success = false,
                    message = "Enter Full Name"
                });

            }
            else if (string.IsNullOrEmpty(vm.mast.Email))
            {
                return Json(new
                {
                    success = false,
                    message = "Enter Email"
                });
            }
            else if (string.IsNullOrEmpty(vm.mast.MobileNo))
            {
                return Json(new
                {
                    success = false,
                    message = "Enter Mobile Number"
                });
            }
            else if (string.IsNullOrEmpty(vm.mast.Address))
            {
                return Json(new
                {
                    success = false,
                    message = "Enter Your Address"
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
                    GrandTotal = vm.mast.GrandTotal,
                    OrderDate = DateTime.Now,
                    PaymentStatus = "Pending",
                    Tid = ""
                };

                _context.ProductOrderMaster.Add(m);
                _context.SaveChanges();


                //Now save detail

                List<ProductOrderDetail> d = new List<ProductOrderDetail>();
                foreach (var item in vm.dtl)
                {
                    d.Add(new ProductOrderDetail
                    {
                        ProductItemId = item.Id,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity,
                        ProductOrderMasterID = m.ProductOrderMasterID
                    });
                }
                _context.ProductOrderDetail.AddRange(d);
                _context.SaveChanges();
                return Json(new
                {
                    success = true,
                    message = "User Detail Saved and Order Placed Successfully",
                    data = d
                });

            }
        }



        public IActionResult Cart(int? id)
        {
            return View();
        }

        public IActionResult Index()
        {
            var datas = _context.ProductItem.ToList();
            return View(datas);
        }


        public JsonResult GetProductItems()
        {
            var datas = _context.ProductItem.Select(x => new
            {
                productItemId = x.ProductItemId,
                ProductName = x.ProductName,
                ProductCode = x.ProductCode,
                categoryId = x.CategoryId,
                description = x.description,
                unitPrice = x.UnitPrice,
                thumbnail = x.Thumbnail
            }).ToList();

            return Json(new { data = datas });
        }

        public IActionResult Page(int id)
        {
            var datas = _context.ProductItem.Where(x => x.ProductItemId == id).FirstOrDefault();
            return View(datas);
        }
        public JsonResult Save(int hiddenId, string ProductName, string ProductCode, int CategoryId, string description, decimal UnitPrice, string Thumbnail)
        {
            if (hiddenId == 0)
            {
                ProductItem productItem = new ProductItem();
                productItem.ProductName = ProductName;
                productItem.ProductCode = ProductCode;
                productItem.CategoryId = CategoryId;
                productItem.description = description;
                productItem.UnitPrice = UnitPrice;
                productItem.Thumbnail = Thumbnail;

                _context.Add(productItem);
                _context.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = "Product item has been saved successfully"
                });
            }
            else
            {
                var dbData = _context.ProductItem.Where(x => x.ProductItemId == hiddenId).FirstOrDefault();
                if (dbData == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Product item not found"
                    });
                }
                else
                {
                    dbData.ProductName = ProductName;
                    dbData.ProductCode = ProductCode;
                    dbData.CategoryId = CategoryId;
                    dbData.description = description;
                    dbData.UnitPrice = UnitPrice;
                    dbData.Thumbnail = Thumbnail;

                    _context.Update(dbData);
                    _context.SaveChanges();

                    return Json(new
                    {
                        success = true,
                        message = "Product item has been updated successfully"
                    });
                }
            }
        }

        public JsonResult Edit(int id)
        {
            var dbData = _context.ProductItem.Where(x => x.ProductItemId == id).FirstOrDefault();

            if (dbData == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Product item not found"
                });
            }
            else
            {
                return Json(new
                {
                    success = true,
                    data = dbData
                });
            }
        }

        public JsonResult Delete(int? id)
        {
            var dbData = _context.ProductItem.FirstOrDefault(x => x.ProductItemId == id);

            if (dbData == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Product item not found"
                });
            }
            else
            {
                _context.ProductItem.Remove(dbData);
                _context.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = "Product item deleted successfully"
                });
            }
        }


    }
}
