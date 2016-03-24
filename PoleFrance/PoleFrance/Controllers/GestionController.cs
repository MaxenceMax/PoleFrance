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

        
        //Route pour la page d'administration
        public ActionResult AdminHome()
        {
            return View();
        }

       
        //Route pour ajouter un responsable
        public ActionResult AjoutResponsable()
        {
            return View();
        }

    }

}
