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
            List<Models.Responsable> temp = new List<Models.Responsable>();

            var req = from i in bd.Responsable
                      select i;

            foreach (var j in req)
            {
                
                temp.Add(j);
            }



            ResponsableViewModel vm = new ResponsableViewModel
            {

                ListeDesResponsables = temp.ToList(),

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

            Models.Responsable resp = new Models.Responsable();
            resp.Nom = model.Nom;
            resp.Prenom = model.Prenom;
            resp.Login = model.Login;
            resp.Password = model.Password;
            resp.AdresseEmail = model.AdresseEmail;
            resp.Poleid = model.Poleid;

           
            bd.Responsable.InsertOnSubmit(resp);
            bd.SubmitChanges();
            

            return RedirectToAction("ListeResponsable", "Gestion");

        }


        [AllowAnonymous]
        public ActionResult SuppressionResponsable(decimal id)
        {


            PolesDataContext bd = new PolesDataContext();

            var req = from i in bd.Responsable
                      where i.id == id
                      select i;


            foreach (var detail in req)
            {
                bd.Responsable.DeleteOnSubmit(detail);
            }

            try
            {
                bd.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Provide for exceptions.
            }

            return RedirectToAction("ListeResponsable", "Gestion");

        }


}
}

