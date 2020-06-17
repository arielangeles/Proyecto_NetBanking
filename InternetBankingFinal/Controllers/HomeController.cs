using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternetBankingFinal.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return Redirect("../User/Login");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contactenos!";

            return View();
        }

        [Authorize]
        public ActionResult NetBank()
        {
            return View();
        }
    }
}