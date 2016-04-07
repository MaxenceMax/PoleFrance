using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PoleFrance.Models;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections;
using System.Net.Mail;
using System.Linq;

namespace PoleFrance.Controllers
{

    public class HomeController : Controller
    {
        // Route principale pour l'inscription
        [AllowAnonymous]
        public ActionResult Inscription()
        {
            ViewBag.FormVisibility = CheckInscription();

            return View();
        }

        private String CheckInscription()
        {
            PolesDataContext pdc = new PolesDataContext();
            SuperAdmin sa = pdc.SuperAdmin.First();
            String returnString = "";
            if ((bool)sa.Open)
                returnString = "visible";
            return returnString;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Inscription(Models.Candidature model)
        {
            ViewBag.FormVisibility = CheckInscription();
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
            // Get candidature from previous controller
            Models.Candidature candidature = TempData["model"] as Models.Candidature;
            MainModel mainModel = new MainModel();
            mainModel.Candidature = candidature;

            
            ViewBag.listePole = GetAllPole();
            // check if we come from inscription 1
            if (mainModel.Candidature == null)
                return RedirectToAction("Inscription");
            return View(mainModel);
        }

        private IEnumerable GetAllPole()
        {
            // get all pôle from database
            PolesDataContext bd = new PolesDataContext();
            var all = bd.Pole;
            return all;
        }

        // test si je n'ai pas déjà une personne inscrite pour l'année en cours
        private bool TestInscription(Models.Candidature candidature)
        {
            PolesDataContext bd = new PolesDataContext();
            int numCount = (from i in bd.Candidature
                            where i.NumLicencie == candidature.NumLicencie && i.Annee == candidature.Annee
                            select i).Count();
            return numCount == 0;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Inscription2(MainModel model)
        {
            ViewBag.listePole = GetAllPole();

            // About general infos
            DateTime today = DateTime.Today;
            model.Candidature.DateDemarche = today.ToString("dd/MM/yyyy");
            model.Candidature.Annee = Int32.Parse(DateTime.Now.Year.ToString());
            if (!TestInscription(model.Candidature))
            {
                ModelState.AddModelError(string.Empty, "Vous avez déjà une inscription enregistrée dans notre système pour l'année en cours.");
                return View(model);
            }
            // Enregistrement de toutes les informations concernant la canditure. 
            model.Candidature.Internat = model.Internat;
            model.Candidature.AutorisationParent = model.AutorisationParent;
            model.Candidature.Redirection = model.Redirection;

            // ABout scolaire infos
            model.Candidature.InformationScolaire = model.InformationScolaire;
            model.SouhaitScolaire.InformationScolaire = model.InformationScolaire;
            model.LV1.SouhaitScolaire = model.SouhaitScolaire;
            model.LV2.SouhaitScolaire = model.SouhaitScolaire;
            model.option.SouhaitScolaire = model.SouhaitScolaire;

            // About sportive infos
            model.Information1.Candidature = model.Candidature;
            model.Information2.Candidature = model.Candidature;
            model.Information3.Candidature = model.Candidature;

            //Pole/candidature requirement
            PoleCandidature PC = new PoleCandidature();
            PC.Candidature = model.Candidature;
            PC.Poleid = model.Pole.id;

            PolesDataContext bd = new PolesDataContext();
            // Insertions in data base
            bd.Candidature.InsertOnSubmit(model.Candidature);
            bd.SouhaitScolaire.InsertOnSubmit(model.SouhaitScolaire);
            bd.InformationSportive.InsertOnSubmit(model.Information1);
            bd.InformationSportive.InsertOnSubmit(model.Information1);
            bd.InformationSportive.InsertOnSubmit(model.Information1);
            bd.SubmitChanges();

            // Envoyer un mail
            String tmp = envoyer(model);
            return RedirectToAction("Inscription3");
        }

        private String envoyer(MainModel model)
        {
            String retour = "";
            String signature = "<br><br>Fédération Française de Judo, Jujitsu, Kendo et Disciplines Associées<br>";
            signature = signature + "Association Loi 1901<br>";
            signature = signature + "21-25, avenue de la Porte de Châtillon - 75014 Paris<br>";
            signature = signature + "Tél : 01 40 52 16 16<br>";
            String message = "";


            MailMessage email = new MailMessage();
            email.From = new MailAddress("informations@licences-ffjudo.com");

            // Enregistrement des destinataires
            email.To.Add(new MailAddress(model.Candidature.AdresseEmail));
            email.To.Add(new MailAddress(model.Candidature.AdresseEmailParent));

            PolesDataContext bd = new PolesDataContext();
            Pole p = (from i in bd.Pole
                            where i.id == model.Pole.id select i).First();

            // structure du message
            String _message = "<div style=' font-family: Calibri;'>Bonjour "+model.Candidature.Prenom+" "+model.Candidature.Nom+
                ",<br><br>"+
                "Votre inscription à la structure suivante: <br>"+
                "<b style = 'font-size:19' >"+ p.Nom +"</b ><br>"+
                "à bien été enregistrée à la date du <b style = 'font-size:19'>"+ model.Candidature.DateDemarche +
                "</b>. Veuillez conservez cet email comme preuve de votre inscriptions."+
                " Vous serez contacté par mail, ou par voie postale aprés la délibération du jury.<br>"+
                "<br><br>Cordialement</div>";
            email.Subject = "Confirmation inscription à la structure :"+p.Nom;
            message = "<div style=' font-family: Calibri;'>" + _message + signature + "</div>";
            email.Body = message;
            email.IsBodyHtml = true;
            //email.Priority = MailPriority.High;
            //SmtpClient client = new SmtpClient("195.154.95.219", 587);
            // Command line argument must the the SMTP host.
            SmtpClient client = new SmtpClient();
            //client.Port = 587;
            client.Host = "mail.sevenstorm.com";
            //client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("informations@licences-ffjudo.com", "nBD)dB3");
            try
            {
                client.Send(email);
            }
            catch (Exception ex)
            {
                retour = ex.Message;
            }
            return retour;
        }

        public ActionResult Inscription3()
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