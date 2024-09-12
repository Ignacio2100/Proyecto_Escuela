using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class ColorController : Controller
    {
        [Authorize]
        // GET: Color
        public ActionResult Index()
        {
            return View();
        }
    }
}