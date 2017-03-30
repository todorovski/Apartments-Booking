using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_MyHotel_example.Models;
using System.Data;
using System.Data.Entity;

namespace MVC_MyHotel_example.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index()
        {
            return View(db.Hotels.Take(4).ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult NoAccess()
        {
            ViewBag.Message = "No access";
            return View();
        }
    }
}