using PoleFrance.Models;
using PoleFrance.ViewModels;
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

            PolesDataContext bd = new PolesDataContext();
            List<AjoutResponsableModel> temp = new List<AjoutResponsableModel>();

            var req = from i in bd.Responsable
                      select new { i.Nom, i.Prenom, i.Login, i.AdresseEmail };

            foreach (var j in req)
            {
                AjoutResponsableModel v = new AjoutResponsableModel();
                v.Nom = j.Nom;
                v.Prenom = j.Prenom;
                v.Pseudo = j.Login;
                v.Mail = j.AdresseEmail;
                temp.Add(v);
            }


            ResponsableViewModel vm = new ResponsableViewModel
            {

                ListeDesResponsables = new List<AjoutResponsableModel>
                {
                    new AjoutResponsableModel { Nom = "JPM party", Prenom = "Jean Pierre", Pseudo = "P1", Mail = "test@test.fr" },
                    new AjoutResponsableModel { Nom = "Hello", Prenom = "Don", Pseudo = "P2", Mail = "test@test2.fr" },
                    

                }

            };

    
            return View(vm);
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

