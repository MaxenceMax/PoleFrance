using PoleFrance.Models;
using PoleFrance.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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

        public ActionResult ExtractionInscriptions()
        {
            
            StringWriter sw = new StringWriter();
           
      
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=ListeInscriptions.csv");
            Response.ContentType = "text/csv";
            Response.Charset = "utf-8";
            Response.Write("\uFEFF");


            PolesDataContext bd = new PolesDataContext();

            decimal poleid = 0;
            var claimIdentity = User.Identity as ClaimsIdentity;
            if (claimIdentity != null)
            {
                var nomResponsable = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                poleid = (from i in bd.Responsable
                          where i.Login == nomResponsable
                          select i.Poleid).First();
            }


            var infosglobales = from i in bd.VuesInformationsGlobales
                                where i.Poleid == poleid
                                select i;
            sw.WriteLine("\"NOM\";\"Prénom\";\"Année\";\"Numéro de Licence\";\"Sexe\";\"Pole Actuel\";\"Statut de la demande\";\"Commentaire Ligue\";\"Adresse Mail\";\"Catégorie d'âge\";\"Catégorie de poids\";\"Taille\";\"Poids\";\"Adresse\";\"Code Postal\";\"Ville\";\"Téléphone\";\"Téléphone Parents\";\"Email Parents\";\"Classe Actuelle\";\"Etablissement Actuel\";\"Adresse Etablissement Actuel\";\"Classe Souhaitée\";\"Etablissement Souhaité\"");

            foreach (var info in infosglobales)
            {


                sw.WriteLine(string.Format("\"{0}\";\"{1}\";\"{2}\";\"{3}\";\"{4}\";\"{5}\";\"{6}\";\"{7}\";\"{8}\";\"{9}\";\"{10}\";\"{11}\";\"{12}\";\"{13}\";\"{14}\";\"{15}\";\"{16}\";\"{17}\";\"{18}\";\"{19}\";\"{20}\";\"{21}\";\"{22}\";\"{23}\"",

                info.Nom,
                info.Prenom,
                info.Annee,
                info.NumLicencie,
                info.Sexe,
                info.PoleActuel,
                getTraitement(info.Traitement),
                info.CommentaireLigue,
                info.AdresseEmail,
                info.CategorieAgeActuelle,
                info.CategoriePoidsActuelle,
                info.Taille,
                info.Poids,
                info.Rue,
                info.CodePostal,
                info.Ville,
                info.Telephone,
                info.TelephoneParents,
                info.AdresseEmailParent,
                info.Classe,
                info.Etablissement,
                info.Adresse,
                info.ClasseSouhait,
                info.EtablissementSouhait

                ));

                sw.WriteLine();
                sw.WriteLine("\" \";\"Résultats Sportifs\";\"Compétition\";\"Résultat\";\"Catégorie d'âge\";\"Catégorie de poids\";\"Année\";");
                var infosSportives = from i in bd.VuesInformationSportive
                                     where i.Candidatureid == info.id
                                     select i;

                foreach (var infos in infosSportives)
                {


                    sw.WriteLine(string.Format("\"{0}\";\"{1}\";\"{2}\";\"{3}\";\"{4}\";\"{5}\";\"{6}\"",

                        " ",
                        " ",
                        infos.Competition,
                        infos.Resultat,
                        infos.CategorieAge,
                        infos.CategoriePoids,
                        infos.AnneeSportive

                  ));

                }
                sw.WriteLine();

            }

            Response.Write(sw.ToString());
            Response.End();

            return RedirectToAction("ResponsableHome", "Responsable");
        }


        public ActionResult ExtractionToutesInscriptions()
        {


            StringWriter sw = new StringWriter();

           

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=ListeInscriptions.csv");
            Response.ContentType = "text/csv";
            Response.Charset = "utf-8";
            Response.Write("\uFEFF");


            PolesDataContext bd = new PolesDataContext();

          


            var infosglobales = from i in bd.VuesInformationsGlobales
                                select i;

            sw.WriteLine("\"NOM\";\"Prénom\";\"Année\";\"Numéro de Licence\";\"Sexe\";\"Pole Actuel\";\"Statut de la demande\";\"Commentaire Ligue\";\"Adresse Mail\";\"Catégorie d'âge\";\"Catégorie de poids\";\"Taille\";\"Poids\";\"Adresse\";\"Code Postal\";\"Ville\";\"Téléphone\";\"Téléphone Parents\";\"Email Parents\";\"Classe Actuelle\";\"Etablissement Actuel\";\"Adresse Etablissement Actuel\";\"Classe Souhaitée\";\"Etablissement Souhaité\"");

            foreach (var info in infosglobales)
            {


                sw.WriteLine(string.Format("\"{0}\";\"{1}\";\"{2}\";\"{3}\";\"{4}\";\"{5}\";\"{6}\";\"{7}\";\"{8}\";\"{9}\";\"{10}\";\"{11}\";\"{12}\";\"{13}\";\"{14}\";\"{15}\";\"{16}\";\"{17}\";\"{18}\";\"{19}\";\"{20}\";\"{21}\";\"{22}\";\"{23}\"",

                info.Nom,
                info.Prenom,
                info.Annee,
                info.NumLicencie,
                info.Sexe,
                info.PoleActuel,
                getTraitement(info.Traitement),
                info.CommentaireLigue,
                info.AdresseEmail,
                info.CategorieAgeActuelle,
                info.CategoriePoidsActuelle,
                info.Taille,
                info.Poids,
                info.Rue,
                info.CodePostal,
                info.Ville,
                info.Telephone,
                info.TelephoneParents,
                info.AdresseEmailParent,
                info.Classe,
                info.Etablissement,
                info.Adresse,
                info.ClasseSouhait,
                info.EtablissementSouhait

                ));

                sw.WriteLine();
                sw.WriteLine("\" \";\"Résultats Sportifs\";\"Compétition\";\"Résultat\";\"Catégorie d'âge\";\"Catégorie de poids\";\"Année\";");
                var infosSportives = from i in bd.VuesInformationSportive
                                     where i.Candidatureid == info.id
                                     select i;

                foreach (var infos in infosSportives)
                {


                    sw.WriteLine(string.Format("\"{0}\";\"{1}\";\"{2}\";\"{3}\";\"{4}\";\"{5}\";\"{6}\"",

                        " ",
                        " ",
                        infos.Competition,
                        infos.Resultat,
                        infos.CategorieAge,
                        infos.CategoriePoids,
                        infos.AnneeSportive

                  ));

                }
                sw.WriteLine();

            }

            Response.Write(sw.ToString());
            Response.End();


            return RedirectToAction("AdminHome", "Gestion");
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
            if (resp.Count() == 0)
                return 12;
            return resp.First().Poleid; 
        }

        private string getTraitement(decimal? trait)
        {

            string retour = "";
            if (trait == 0)
                retour = "En attente de statut";
            else if (trait == 1)
                retour = "Préselectionné";
            else if (trait == 2)
                retour = "Possible redirection";
            else if (trait == 3)
                retour = "Provisoirement écarté";

            return retour;
        }

        private string getBoolean(bool vrai)
        {
            string retour = "";
            if (vrai == true)
                retour = "Oui";
            else if (vrai == false)
                retour = "Non";

            return retour;

        }


        [HttpPost]        
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