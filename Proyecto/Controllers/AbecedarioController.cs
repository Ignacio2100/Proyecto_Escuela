using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
   
    public class AbecedarioController : Controller
    {
        [Authorize]
        // GET: Abecedario
        public ActionResult Index()
        {
            return View();
        }
    }
}