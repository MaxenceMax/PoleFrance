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


        //Route pour afficher les responsables
        public ActionResult ListeResponsable()
        {


            ViewData["message"] = "Bonjour depuis le contrôleur";
            ViewData["resp"] = new AjoutResponsableModel { Nom = "JPM party", Prenom = "Jean Pierre" };


            return View();
        }


        //Route pour ajouter un responsable
        public ActionResult AjoutResponsable()
         {
             return View();
         } 


        [HttpPost]
        [AllowAnonymous]
        public ActionResult AjoutResponsable(Models.AjoutResponsableModel model)
        {

            PolesDataContext bd = new PolesDataContext();
            Responsable resp = new Responsable();

            resp.Nom = model.Nom;
            resp.Prenom = model.Prenom;
            resp.Login = model.Pseudo;
            resp.Password = model.Pass;
            resp.AdresseEmail = model.Mail;
            resp.Adresse = "test adresse";
            resp.Poleid = 0;

            bd.Responsable.InsertOnSubmit(resp);
            bd.SubmitChanges();

            
            return RedirectToAction("AdminHome", "Gestion");



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

