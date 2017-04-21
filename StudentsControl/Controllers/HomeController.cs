using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentsControl.Controllers
{
    public class HomeController : Controller
    {
        //Menu Principal
        public ActionResult Index()
        {
            return View();
        }

        //Menu Principal de Maestros de Datos
        public ActionResult MasterData()
        {
            ViewBag.Message = "Menu Principal de Maestros de Datos";
            return View();
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
    }
}