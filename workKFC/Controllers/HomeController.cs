using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using workKFC.Models;

namespace workKFC.Controllers
{
    public class HomeController : Controller
    {
        workKFCContext db=new workKFCContext();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var meals = db.meals.ToList();
            return View(meals);
        }

        public IActionResult Meals()   //�n�J���\�᭺��
        {
            var meals = db.meals.ToList();
            return View(meals);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string account, string password)
        {
            var member = db.customer.Where(m => m.account == account && m.password == password).FirstOrDefault();
            if (member == null)
            {
                ViewBag.Message = "�n�J����";
                return View("Login");
            }
            
            HttpContext.Session.SetString("account", account);

            //Cookies
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(3);     // �]�w�ɶ�
            HttpContext.Response.Cookies.Append("myaccount", account, options);

            //CookiesŪ��
            if (HttpContext.Request.Cookies.TryGetValue("myaccount", out string myaccount))
            {
                ViewBag.myaccount = myaccount; // �x�s�� ViewBag
            }
            else
            {
                ViewBag.myaccount = "Cookie ���s�b"; // �B�z���s�b�����p
            }    //�x�s��ViewBag,��VView���

            return RedirectToAction("Meals");   //�n�J���\����
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult AddCar(string mNum)
        {
            var mydate = Convert.ToDateTime(DateTime.Now.ToShortDateString());    //�ഫ�{�b���
            var acc = HttpContext.Session.GetString("account");
            var currentCar = db.orders
                 .Where(m => m.account == acc && m.mealnum == mNum && m.orderdate == mydate)
                 .FirstOrDefault();

            if (currentCar == null)
            {

                var meal = db.meals.Where(m => m.num == mNum).FirstOrDefault();
                var title = meal.title;
                var price = meal.price;
                //�q��
                orders myorder = new orders();
                myorder.ordersnum = Guid.NewGuid().ToString();  //�q��s��
                myorder.account = acc;
                myorder.orderdate = mydate;
                myorder.mealnum = mNum;   //�\�I�s��

                myorder.qty = 1;
                myorder.mealtitle = title;
                myorder.price = price;
                myorder.total = price * myorder.qty;
                db.orders.Add(myorder);
            }
            else
            {
                currentCar.qty += 1;
                currentCar.total = currentCar.qty * currentCar.price;
            }
            db.SaveChanges();
            var data = db.orders
                .Where(m => m.account == acc && m.mealnum == mNum && m.orderdate == mydate)
                .ToList();
            return RedirectToAction("showCar");
        }

        public ActionResult showCar()
        {
            string acc = HttpContext.Session.GetString("account");
            var mydate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            var data = db.orders
             .Where(m => m.account == acc && m.orderdate == mydate)
             .ToList();

            //�p�p�[�`,Linq�d��
            var result = from m in db.orders
                         where m.account == acc && m.orderdate == mydate
                         select m;
            ViewBag.sum = result.Sum(m => m.total);  //�p�p�[�`
            return View(data);
        }

        public ActionResult register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult register(string account, string password, string name, string tel, string address)
        {
            customer cust = new customer();
            cust.account = account;
            cust.password = password;
            cust.name = name;
            cust.tel = tel;
            cust.address = address;
            db.customer.Add(cust);
            db.SaveChanges();
            HttpContext.Session.SetString("account", account);
            return RedirectToAction("Meals");   //���U���\�᭺��
        }

        public IActionResult Delete(string mNum)
        {
            string acc = HttpContext.Session.GetString("account");
            var mydate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            var data = db.orders.Where(m => m.mealnum == mNum && m.account == acc && m.orderdate == mydate).FirstOrDefault();
            db.orders.Remove(data);
            db.SaveChanges();
            //�p�p�[�`,Linq�d��
            var result = from m in db.orders
                         where m.account == acc && m.orderdate == mydate
                         select m;
            ViewBag.sum = result.Sum(m => m.total);  //�p�p�[�`
            return RedirectToAction("showCar");
        }


    }
}
