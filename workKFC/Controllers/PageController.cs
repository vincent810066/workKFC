using Microsoft.AspNetCore.Mvc;
using workKFC.Models;
using X.PagedList;
using X.PagedList.Extensions;



namespace workKFC.Controllers
{
    public class pageController : Controller
    {
        workKFCContext db=new workKFCContext();
        int pageSize = 4;
        public IActionResult Index(int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            var products = db.meals.OrderBy(m => m.num).ToList();
            var result = products.ToPagedList(currentPage, pageSize);
            return View(result);

        }
    }

}
