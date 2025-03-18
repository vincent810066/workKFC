using Microsoft.AspNetCore.Mvc;
using workKFC.Models;

namespace workKFC.Controllers
{
    public class TypeController : Controller
    {
        workKFCContext db=new workKFCContext();
        public IActionResult Index(int id=1)
        {
            if (id == 1)
                ViewBag.name = "套餐";
            else if (id == 2)
                ViewBag.name = "蛋塔盒";
            else if (id == 3)
                ViewBag.name = "桶餐";
            else
                ViewBag.name = "飲料其它";
            var data = db.meals.Where(m => m.num.Substring(1, 1).Equals(id + "")).ToList();

            return View(data);

        }
    }
}
