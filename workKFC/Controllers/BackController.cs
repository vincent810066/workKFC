using Microsoft.AspNetCore.Mvc;
using workKFC.Models;

namespace workKFC.Controllers
{
    public class BackController : Controller
    {
        workKFCContext db = new workKFCContext();
        public IActionResult Index()
        {
            var meals = db.meals.ToList();
            return View(meals);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string num, string title, int price, IFormFile photo)
        {
            //上傳圖檔
            string fileName = "";
            //檔案上傳
            if (photo != null)
            {
                if (photo.Length > 0)
                {
                    string filePath = "wwwroot/images/" + photo.FileName;
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        //程式寫入的本地資料夾裡面
                        await photo.CopyToAsync(stream);
                    }
                }

            }
            meals m = new meals();
            m.num = num;
            m.title = title;
            m.price = price;
            m.img = photo.FileName;
            db.meals.Add(m);
            db.SaveChanges();

            return RedirectToAction("Index", "Back");  //轉向Index首頁
        }

        //修改
        public ActionResult Edit(string id)
        {
            //先查出這筆記錄           
            var data = db.meals.Where(m => m.num == id).FirstOrDefault();
            //暫存至ViewBag,表單欄位可以顯示出來        
            ViewBag.name = data.title;
            ViewBag.price = data.price;
            ViewBag.img = data.img;
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string num, string title, int price, IFormFile photo)
        {
            //先找到這筆記錄
            var data = db.meals.Where(m => m.num == num).FirstOrDefault();
            //上傳圖檔
            string fileName = "";
            //檔案上傳
            if (photo != null)
            {
                if (photo.Length > 0)
                {
                    string filePath = "wwwroot/images/" + photo.FileName;
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        data.img = photo.FileName;   //圖片檔名
                        //程式寫入的本地資料夾裡面
                        await photo.CopyToAsync(stream);
                    }
                }
            }
            data.title = title;
            data.price = price;
            db.SaveChanges();
            return RedirectToAction("index", "Back");
        }

        public ActionResult Delete(string id)
        {
            var data = db.meals.Where(m => m.num == id).FirstOrDefault();
            db.meals.Remove(data);
            db.SaveChanges();
            return RedirectToAction("index", "Back");
        }

    }

}

