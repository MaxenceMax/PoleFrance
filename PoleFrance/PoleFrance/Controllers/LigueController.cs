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

    public class LigueController : Controller
    {
        [Authorize(Roles = "ResponsableLigue")]
        public ActionResult LigueHome()
        {
            PolesDataContext bd = new PolesDataContext();

            if (Request.IsAuthenticated)
            {
                var claimIdentity = User.Identity as ClaimsIdentity;
                if (claimIdentity != null)
                {
                    var loginLigue = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                    var req = (from i in bd.ResponsableLigue
                               where i.Login == loginLigue
                               select i).First();
                    ViewBag.NomLigue = req.NomLigue;
                    ViewBag.NomResp = req.Prenom + " " + req.Nom;
                }
            }


            return View();
        }

        [Authorize(Roles = "ResponsableLigue")]
        public ActionResult LiguesInscriptions()
        {
            PolesDataContext bd = new PolesDataContext();

            var claimIdentity = User.Identity as ClaimsIdentity;

            var nomLigue = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var candidature = from i in bd.Candidature
                              join l in bd.ResponsableLigue on i.IdLigue equals l.idLigue
                              where l.Login == nomLigue
                              orderby i.Nom ascending
                              select i;

            var Ligue = from q in bd.ResponsableLigue
                        where q.Login == nomLigue select q;

            ViewBag.NomLigue = Ligue.First().NomLigue;
            ListeCandidatureLigueViewModel lc = new ListeCandidatureLigueViewModel{ListeDesCandidats = candidature.ToList()};
            return View(lc);
        }
        [Authorize(Roles = "ResponsableLigue")]
        public ActionResult DetailCandidat(decimal id)
        {
            PolesDataContext bd = new PolesDataContext();
            var poleCandidat = from i in bd.PoleCandidature
                               where i.Candidatureid == id
                               select i;

            AffichageCandidature af = new AffichageCandidature();
            af.polecandidatureId = poleCandidat.First().id;
            af.PoleCandidature = poleCandidat.First();

            ViewBag.idPoleResp = getIdPoleResp(bd);
            return View(af);
        }

        [Authorize(Roles = "ResponsableLigue")]
        [HttpPost]
        [HandleError]
        public ActionResult DetailCandidat(AffichageCandidature model)
        {

            PolesDataContext bd = new PolesDataContext();
            var poleCandidat = from i in bd.PoleCandidature
                               where i.id == model.polecandidatureId
                               select i;
            ViewBag.idPoleResp = getIdPoleResp(bd);
            poleCandidat.First().Candidature.CommentaireLigue = model.PoleCandidature.Candidature.CommentaireLigue;
            bd.SubmitChanges();
            model.polecandidatureId = poleCandidat.First().id;
            model.PoleCandidature = poleCandidat.First();
            return View(model);
        }




        private decimal getIdPoleResp(PolesDataContext bd)
        {
            var claimIdentity = User.Identity as ClaimsIdentity;
            var nomResponsable = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var resp = from i in bd.Responsable
                       where i.Login == nomResponsable
                       select i;
            if (resp.Count() == 0)
                return 12;
            return resp.First().Poleid;
        }
    }
}