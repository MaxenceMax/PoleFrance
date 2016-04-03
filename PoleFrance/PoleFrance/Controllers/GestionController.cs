using PoleFrance.Models;
using PoleFrance.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
           
            PolesDataContext bd = new PolesDataContext();

            var admin = bd.SuperAdmin.First();

            ViewBag.texte = "Les inscriptions sont actuellement fermées.";
            ViewBag.image = "~/Content/Images/Admin/open.png";
            if (admin.Open == true)
            {
                ViewBag.image = "~/Content/Images/Admin/close.png";
                ViewBag.texte = "Les inscriptions sont actuellement ouvertes.";
            }




            return View();
        }


       //Route pour afficher les responsables
        public ActionResult ListeResponsable()
        {

            PolesDataContext bd = new PolesDataContext();
        
            var all = bd.Responsable;


            ResponsableViewModel vm = new ResponsableViewModel
            {
                ListeDesResponsables = all.ToList(),
              
            };



            return View(vm);
        }


        //Fonction pour ouvrir les inscriptions
        public ActionResult OuvertureInscription()
        {

            PolesDataContext bd = new PolesDataContext();

            var admin = bd.SuperAdmin.First();

            if(admin.Open == true){
                admin.Open = false;}
            else{
                admin.Open = true;}

            try
            {
                bd.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Provide for exceptions.
            }

            return RedirectToAction("AdminHome", "Gestion");

        }


        //Route pour ajouter un responsable
        [Authorize(Roles = "Admin")]
        public ActionResult AjoutResponsable()
         {

            PolesDataContext bd = new PolesDataContext();

            var all = bd.Pole;

            ViewBag.listePole = all;

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
            resp.Password = encrypt(model.Password);
            resp.AdresseEmail = model.AdresseEmail;
            resp.Poleid = model.Poleid;

           
            bd.Responsable.InsertOnSubmit(resp);
            bd.SubmitChanges();
            

            return RedirectToAction("ListeResponsable", "Gestion");

        }

        public String encrypt(string mdp)
        {
            Byte[] clearBytes = new UnicodeEncoding().GetBytes(mdp);
            Byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
            string hashedText = BitConverter.ToString(hashedBytes);
            return hashedText;

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

