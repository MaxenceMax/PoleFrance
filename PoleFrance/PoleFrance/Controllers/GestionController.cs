using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoleFrance.Controllers
{
    public class GestionController : Controller
    {
        //Route secondaire pour la connexion
        public ActionResult Connexion()
        {
            return View();
        }
    }
}