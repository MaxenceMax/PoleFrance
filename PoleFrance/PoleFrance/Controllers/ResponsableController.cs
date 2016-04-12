using PoleFrance.Models;
using PoleFrance.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace PoleFrance.Controllers
{
    public class ResponsableController : Controller
    {


        // GET: Responsable
        public ActionResult ResponsableHome()
        {

            PolesDataContext bd = new PolesDataContext();

            if (Request.IsAuthenticated)
            {
                var claimIdentity = User.Identity as ClaimsIdentity;
                if (claimIdentity != null)
                {
                     var nomResponsable = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                    var req = (from i in bd.Responsable
                              where i.Login == nomResponsable
                              select i).First();

                    ViewBag.texte = req.Pole.Nom;
                }
            }


            return View();
        }


        public ActionResult ListeMesInscriptions()
        {


            PolesDataContext bd = new PolesDataContext();

            var all = bd.Responsable;

            var req = bd.Pole;

            var test = from i in bd.Candidature
                       where i.id < 40
                       select i;



              ListeCandidatureViewModel lc = new ListeCandidatureViewModel
              {
                  ListeDesCandidatures = test.ToList(),
              }; 

            ResponsableViewModel vm = new ResponsableViewModel
            {
                ListeDesResponsables = all.ToList(),

            };



            return View(lc);

        }


        public ActionResult ListeToutesInscriptions()
        {




            return View();

        }


    }
    

}