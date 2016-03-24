using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace PoleFrance.Controllers
{
 
    public class GestionController : Controller
    {

        
        //Route secondaire pour la connexion
        public ActionResult AdminHome()
        {
            return View();
        }

       
        public ActionResult AjoutResponsable()
        {
            return View();
        }

    }

}
