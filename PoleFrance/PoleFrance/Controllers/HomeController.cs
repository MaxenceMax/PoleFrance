using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoleFrance.Controllers
{
    public class HomeController : Controller
    {
        // Route principale pour l'inscription
        public ActionResult Inscription()
        {
            return  View();
        }
    }
}