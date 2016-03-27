using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Data.Linq;
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
        //[ValidateAntiForgeryToken]
        public ActionResult Inscription(Models.CandidatureViewModel model)
        {
            if (!ValidateNumLicencie(model))
            {
                ModelState.AddModelError(string.Empty, "Le numéro de licence est incorrect.");
                return View(model);
            }
            return View();
        }

        private Boolean ValidateNumLicencie(CandidatureViewModel model)
        {
            if (model.NumLicencie!= null)
            {
                if (model.NumLicencie.Length != 16)
                    return false;
                else if (!IsExistInWebService(model))
                    return true;

                return true;
            }
            return false;
        }

        /** call web service url and check the licence number*/
        private Boolean IsExistInWebService(CandidatureViewModel model)
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
                    items.TryGetValue("adresse", out item);

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