using PoleFrance.Models;
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


        [HttpPost]
        public ActionResult AjoutResponsable(Models.AjoutResponsableModel model)
        {

            //PolesDataContext bd = new PolesDataContext();




            return Redirect("http://fr.openclassrooms.com/");



        }


        private bool InfosVide(string pseudo, string mail)
        {
            bool connecte = false;

            if (pseudo == "" && mail == "")
            {
                connecte = true;
            }

            return connecte;
          
        }
}
}

