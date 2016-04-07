using PoleFrance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

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


        [AllowAnonymous]
        public ActionResult ListeMesInscriptions(decimal id)
        {


            PolesDataContext bd = new PolesDataContext();


            return View();

        }



    }
}