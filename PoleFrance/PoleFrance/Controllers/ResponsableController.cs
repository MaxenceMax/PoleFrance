﻿using PoleFrance.Models;
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

            var claimIdentity = User.Identity as ClaimsIdentity;

            var nomResponsable = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var candidature = from i in bd.PoleCandidature
                              join p in bd.Pole on i.Poleid equals p.id
                              join q in bd.Responsable on i.Poleid equals q.Poleid
                              where q.Login == nomResponsable
                              orderby i.Candidature.Traitement ascending, i.Candidature.Nom ascending
                              select i;



            ListeCandidatureViewModel lc = new ListeCandidatureViewModel
            {
                ListeDesPoles = candidature.ToList(),
            };


            return View(lc);

        }



        public ActionResult ListeToutesInscriptions()
        {

            PolesDataContext bd = new PolesDataContext();


            var candidature = from i in bd.PoleCandidature
                              orderby i.Pole.Nom ascending, i.Candidature.Nom
                              select i;


            ListeCandidatureViewModel lc = new ListeCandidatureViewModel
            {
                ListeDesPoles = candidature.ToList(),

            };



            return View(lc);

        }



        public ActionResult AffichageCandidature(decimal id)
        {

            PolesDataContext bd = new PolesDataContext();
            var poleCandidat = from i in bd.PoleCandidature
                               where i.Candidatureid == id
                               select i;

            AffichageCandidature af = new AffichageCandidature();
            if (poleCandidat.First().Candidature.Traitement == null)
                af.Traitement = 0;
            else
                af.Traitement = (int)poleCandidat.First().Candidature.Traitement;

            af.polecandidatureId = poleCandidat.First().id;
            af.PoleCandidature = poleCandidat.First();

            ViewBag.idPoleResp = getIdPoleResp(bd);
            return View(af);
        }

        private decimal getIdPoleResp(PolesDataContext bd)
        {
            var claimIdentity = User.Identity as ClaimsIdentity;
            var nomResponsable = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var resp = from i in bd.Responsable
                       where i.Login == nomResponsable
                       select i;
            return resp.First().Poleid; 
        }

        [HttpPost]
        [AllowAnonymous]
        [HandleError]
        public ActionResult AffichageCandidature(AffichageCandidature model)
        {

           
            PolesDataContext bd = new PolesDataContext();
            var poleCandidat = from i in bd.PoleCandidature
                               where i.id == model.polecandidatureId
                               select i;
            ViewBag.idPoleResp = getIdPoleResp(bd);
            poleCandidat.First().Candidature.Traitement = model.Traitement;
            bd.SubmitChanges();
            model.polecandidatureId = poleCandidat.First().id;
            model.PoleCandidature = poleCandidat.First();
            return View(model);
        }
    }
}