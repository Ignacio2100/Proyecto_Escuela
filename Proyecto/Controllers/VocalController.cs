using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class VocalController : Controller
    {
        [Authorize]
        // GET: Vocal
        public ActionResult Index()
        {
            return View();
        }
    }
}