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
        //Route pour ajouter un responsable
        [Authorize(Roles = "Responsable")]
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


        // Génére l'historique des saison pour les inscriptions
        public ActionResult HistoriqueInscriptions()
        {
            return View();
        }

        [Authorize(Roles = "Responsable")]
        public ActionResult ListeMesInscriptions()
        {

            PolesDataContext bd = new PolesDataContext();

            var claimIdentity = User.Identity as ClaimsIdentity;

            var nomResponsable = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var candidature = from i in bd.PoleCandidature
                              join p in bd.Pole on i.Poleid equals p.id
                              join q in bd.Responsable on i.Poleid equals q.Poleid
                              where q.Login == nomResponsable && i.Candidature.Annee == Int32.Parse(DateTime.Now.Year.ToString())
            orderby i.Candidature.Traitement ascending, i.Candidature.Nom ascending
                              select i;



            ListeCandidatureViewModel lc = new ListeCandidatureViewModel
            {
                ListeDesPoles = candidature.ToList(),
            };


            return View(lc);

        }




        [Authorize(Roles = "Admin, Responsable")]
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
        [Authorize(Roles = "Responsable")]
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

              


            var infosglobales = from i in bd.PoleCandidature
                                where i.Poleid == poleid
                                orderby i.Candidature.Nom ascending
                                select i;

            sw.WriteLine("\"NOM\";\"Prénom\";\"Année\";\"Numéro de Licence\";\"Sexe\";\"Pole Actuel\";\"Pole Demandé\";\"Statut de la demande\";\"Commentaire Ligue\";\"Adresse Mail\";\"Catégorie d'âge\";\"Catégorie de poids\";\"Taille\";\"Poids\";\"Adresse\";\"Code Postal\";\"Ville\";\"Téléphone\";\"Téléphone Parents\";\"Email Parents\";\"Classe Actuelle\";\"Etablissement Actuel\";\"Adresse Etablissement Actuel\";\"Classe Souhaitée\";\"Etablissement Souhaité\"");

            foreach (var info in infosglobales)
            {


                sw.WriteLine(string.Format("\"{0}\";\"{1}\";\"{2}\";\"{3}\";\"{4}\";\"{5}\";\"{6}\";\"{7}\";\"{8}\";\"{9}\";\"{10}\";\"{11}\";\"{12}\";\"{13}\";\"{14}\";\"{15}\";\"{16}\";\"{17}\";\"{18}\";\"{19}\";\"{20}\";\"{21}\";\"{22}\";\"{23}\";\"{24}\"",

                info.Candidature.Nom,
                info.Candidature.Prenom,
                info.Candidature.Annee,
                info.Candidature.NumLicencie,
                info.Candidature.Sexe,
                info.Candidature.PoleActuel,
                info.Pole.Nom,
                getTraitement(info.Candidature.Traitement),
                info.Candidature.CommentaireLigue,
                info.Candidature.AdresseEmail,
                info.Candidature.CategorieAgeActuelle,
                info.Candidature.CategoriePoidsActuelle,
                info.Candidature.Taille,
                info.Candidature.Poids,
                info.Candidature.Rue,
                info.Candidature.CodePostal,
                info.Candidature.Ville,
                info.Candidature.Telephone,
                info.Candidature.TelephoneParents,
                info.Candidature.AdresseEmailParent,
                info.Candidature.InformationScolaire.Classe,
                info.Candidature.InformationScolaire.Etablissement,
                info.Candidature.InformationScolaire.Adresse,
                info.Candidature.InformationScolaire.SouhaitScolaire.First().Classe,
                info.Candidature.InformationScolaire.SouhaitScolaire.First().Etablissement

                ));

                /*
                sw.WriteLine();
                sw.WriteLine("\" \";\"Résultats Sportifs\";\"Compétition\";\"Résultat\";\"Catégorie d'âge\";\"Catégorie de poids\";\"Année\";");

                foreach (var infos in info.InformationSportive)
                {


                    sw.WriteLine(string.Format("\"{0}\";\"{1}\";\"{2}\";\"{3}\";\"{4}\";\"{5}\";\"{6}\"",

                        " ",
                        " ",
                        infos.Competition,
                        infos.Resultat,
                        infos.CategorieAge,
                        infos.CategoriePoids,
                        infos.Annee

                  ));

                }
                sw.WriteLine();
                */

            }

              Response.Write(sw.ToString());
              Response.End();
              

            return RedirectToAction("ResponsableHome", "Responsable");
        }

        [Authorize(Roles = "Admin, Responsable")]
        public ActionResult ExtractionToutesInscriptions()
        {


            StringWriter sw = new StringWriter();

           

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=ListeInscriptions.csv");
            Response.ContentType = "text/csv";
            Response.Charset = "utf-8";
            Response.Write("\uFEFF");


            PolesDataContext bd = new PolesDataContext();




            var infosglobales = from i in bd.PoleCandidature
                                orderby i.Candidature.Nom ascending
                                select i;

            sw.WriteLine("\"NOM\";\"Prénom\";\"Année\";\"Numéro de Licence\";\"Sexe\";\"Pole Actuel\";\"Pole Demandé\";\"Statut de la demande\";\"Commentaire Ligue\";\"Adresse Mail\";\"Catégorie d'âge\";\"Catégorie de poids\";\"Taille\";\"Poids\";\"Adresse\";\"Code Postal\";\"Ville\";\"Téléphone\";\"Téléphone Parents\";\"Email Parents\";\"Classe Actuelle\";\"Etablissement Actuel\";\"Adresse Etablissement Actuel\";\"Classe Souhaitée\";\"Etablissement Souhaité\"");

            foreach (var info in infosglobales)
            {


                sw.WriteLine(string.Format("\"{0}\";\"{1}\";\"{2}\";\"{3}\";\"{4}\";\"{5}\";\"{6}\";\"{7}\";\"{8}\";\"{9}\";\"{10}\";\"{11}\";\"{12}\";\"{13}\";\"{14}\";\"{15}\";\"{16}\";\"{17}\";\"{18}\";\"{19}\";\"{20}\";\"{21}\";\"{22}\";\"{23}\";\"{24}\"",

                info.Candidature.Nom,
                info.Candidature.Prenom,
                info.Candidature.Annee,
                info.Candidature.NumLicencie,
                info.Candidature.Sexe,
                info.Candidature.PoleActuel,
                info.Pole.Nom,
                getTraitement(info.Candidature.Traitement),
                info.Candidature.CommentaireLigue,
                info.Candidature.AdresseEmail,
                info.Candidature.CategorieAgeActuelle,
                info.Candidature.CategoriePoidsActuelle,
                info.Candidature.Taille,
                info.Candidature.Poids,
                info.Candidature.Rue,
                info.Candidature.CodePostal,
                info.Candidature.Ville,
                info.Candidature.Telephone,
                info.Candidature.TelephoneParents,
                info.Candidature.AdresseEmailParent,
                info.Candidature.InformationScolaire.Classe,
                info.Candidature.InformationScolaire.Etablissement,
                info.Candidature.InformationScolaire.Adresse,
                info.Candidature.InformationScolaire.SouhaitScolaire.First().Classe,
                info.Candidature.InformationScolaire.SouhaitScolaire.First().Etablissement

                ));

                /*
                sw.WriteLine();
                sw.WriteLine("\" \";\"Résultats Sportifs\";\"Compétition\";\"Résultat\";\"Catégorie d'âge\";\"Catégorie de poids\";\"Année\";");
               

                foreach (var infos in info.InformationSportive)
                {


                    sw.WriteLine(string.Format("\"{0}\";\"{1}\";\"{2}\";\"{3}\";\"{4}\";\"{5}\";\"{6}\"",

                        " ",
                        " ",
                        infos.Competition,
                        infos.Resultat,
                        infos.CategorieAge,
                        infos.CategoriePoids,
                        infos.Annee

                  ));

                }
                sw.WriteLine();
                */
            }

            Response.Write(sw.ToString());
            Response.End();


            return RedirectToAction("AdminHome", "Gestion");
        }

        [Authorize(Roles = "Admin, Responsable")]
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

        [Authorize(Roles = "Admin, Responsable")]
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