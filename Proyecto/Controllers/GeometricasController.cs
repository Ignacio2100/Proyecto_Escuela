﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class GeometricasController : Controller
    {
        [Authorize]
        // GET: Geometricas
        public ActionResult Index()
        {
            return View();
        }
    }
}