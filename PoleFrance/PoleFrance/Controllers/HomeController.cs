using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PoleFrance.Models;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections;

namespace PoleFrance.Controllers
{

    public class HomeController : Controller
    {
        // Route principale pour l'inscription
        [AllowAnonymous]
        public ActionResult Inscription()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Inscription(Models.Candidature model)
        {
            if (!ValidateNumLicencie(model))
            {
                ModelState.AddModelError(string.Empty, "Le numéro de licence est incorrect.");
                return View(model);
            }
            TempData["model"] = model;
            return RedirectToAction("Inscription2");
        }

        [AllowAnonymous]
        public ActionResult Inscription2()
        {
            Models.Candidature candidature = TempData["model"] as Models.Candidature;
            MainModel mainModel = new MainModel();
            mainModel.Candidature = candidature;

            // Recherche la listes pôles disponible
            Pole tmp = new Pole();
            tmp.Nom = "caca";
            tmp.id = 2;
            var all = new[] {tmp};

            //PolesDataContext bd = new PolesDataContext();
            //var all = bd.Pole;
            ViewBag.listePole = all;
            // if (mainModel.Candidature == null)
            //   return RedirectToAction("Inscription");
            return View(mainModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Inscription2(MainModel model)
        {
            return View();
        }

        private Boolean ValidateNumLicencie(Models.Candidature model)
        {
            if (model.NumLicencie!= null)
            {
                if (model.NumLicencie.Length != 16)
                    return false;
                else if (!IsExistInWebService(model))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        /** call web service url and check the licence number*/
        private Boolean IsExistInWebService(Models.Candidature model)
        {
            /**
            Create url from licencie
            */
            String url = "http://www.ffjda.org/ws_mobile/webRestGet/service.svc/infosInscriptionASP/";
            String numLicChange = model.NumLicencie.Replace("*", "@").Replace(" ", "§");
            /**
            Make the request
            */
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url+numLicChange);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    // Reader to open http response
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    String back = reader.ReadToEnd();
                    // test le retour de la fonction web
                    // Si j'ai rien le licencie n'existe pas, sinon 
                    if (back.Length == 0 || back == null)
                        return false;

                    // Use dictionnary to reade Json string
                    var dict = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(back);
                    // List to skip first stage
                    ArrayList list = (ArrayList)dict["infosInscriptionASPResult"];
                    // Je récupére tous les items de ma chaine json
                    Dictionary<String, Object> items = (Dictionary<String, Object>)list[0];
                    // Et je traite ceux que je veux
                    object item;
                    items.TryGetValue("numLicence", out item);
                    model.NumLicencie = (String)item;
                    // Si le num licence est vide alors la licence n'existe pas
                    if (model.NumLicencie == null || model.NumLicencie.Length == 0)
                        return false;
                    items.TryGetValue("naissance", out item);
                    model.DateNaissance = (String)item;
                    items.TryGetValue("adresse", out item);
                    model.Ville = (String)item;
                    items.TryGetValue("codeP", out item);
                    model.CodePostal = (String)item;
                    items.TryGetValue("rue", out item);
                    model.Rue = (String)item;
                    items.TryGetValue("mail", out item);
                    model.AdresseEmail = (String)item;
                    items.TryGetValue("prenom", out item);
                    model.Prenom = (String)item;
                    items.TryGetValue("nom", out item);
                    model.Nom = (String)item;
                    items.TryGetValue("sexe", out item);
                    model.Sexe = (String)item;
                    items.TryGetValue("tel", out item);
                    model.Telephone = (String)item;
                    return true;
                  }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }
    }
}