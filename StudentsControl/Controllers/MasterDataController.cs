using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentsControl.Controllers
{
    public class MasterDataController : Controller
    {
        // GET: MasterData/M_CentrosEducativos
        public ActionResult M_CentrosEducativos()
        {
            return View();
        }
        //public ActionResult M_Estudiantes()
        //{
        //    return View();
        //}
    }
}