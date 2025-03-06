using ECommerce.DAO;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using System.Text.Json;

namespace ECommerce.Controllers
{

    public class Bird
    {
        public string BirdName { get; set; }
        public string Color { get; set; }
        public bool CanFly { get; set; }
    }




    public class CategoryController : Controller
    {
        ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;

        }

        public IActionResult Index()
        {
            var datas = _db.Category.ToList();
            return View(datas);
        }


        public JsonResult Save(int id, string categoryName, string categoryCode)
        {
            if (id == 0)
            {
                // insert mode
                Category c = new Category()
                {
                    CategoryName = categoryName,
                    CategoryCode = categoryCode,
                    CreatedDate = DateTime.Now,
                };

                _db.Category.Add(c);
                _db.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = "Data save successfully"
                });
            }
            else
            {
                // update mode
                var dbData = _db.Category.Where(x => x.CategoryID == id).FirstOrDefault();
                if (dbData == null)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "Data Not Found"

                    });
                }
                else
                { 
                    dbData.CategoryName = categoryName;
                    dbData.CategoryCode = categoryCode;

                    _db.SaveChanges();

                    return Json(new
                    {
                        Success = true,
                        Message = "Data updated successfully"

                    });
                }



            }
        }

        public JsonResult Delete(int id)
        {
            var data = _db.Category
                            .Where(x => x.CategoryID == id)
                            .FirstOrDefault();

            if (data == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Data not found"
                });
            }
            else
            {
                _db.Category.Remove(data);
                _db.SaveChanges();
                return Json(new
                {
                    success = true,
                    message = "Data deleted successfully"
                });
            }
        }

        public JsonResult GetById(int id)
        {
            var dbData = _db.Category
                .Where(x => x.CategoryID == id)
                .FirstOrDefault();

            if (dbData == null)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Data Not Found"

                });
            }
            else
            {
                return Json(new
                {
                    Success = true,
                    Data = dbData
                });
            }
        }
    }
}
