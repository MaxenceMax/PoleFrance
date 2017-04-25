using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using PoleFrance.Models;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using PoleFrance.ViewModels;

namespace PoleFrance.Controllers
{
    public class AuthentificationController : Controller
    {
        // GET: Authentification
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.LoginViewModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;


            if (!ModelState.IsValid)
            {
                return View(model);
            }
            StatutConnection statut = ValidateUser(model.Login, model.Password);
            if (!statut.Connected)
            {
                ModelState.AddModelError(string.Empty, "Le nom d'utilisateur ou le mot de passe est incorrect.");
                return View(model);
            }


            // L'authentification est réussie, 
            // injecter les informations utilisateur dans le cookie d'authentification :
            var userClaims = new List<Claim>();
            // Identifiant utilisateur :
            userClaims.Add(new Claim(ClaimTypes.NameIdentifier, model.Login));
            // Rôles utilisateur :
            if(model.Login == "admin")
            {
                userClaims.AddRange(LoadRolesAdmin(model.Login));
            }
            else if(statut.isLigue){
                userClaims.AddRange(LoadRolesResponsableLigue(model.Login));
            }
            else
            {
                userClaims.AddRange(LoadRolesResponsable(model.Login));
            }
            var claimsIdentity = new ClaimsIdentity(userClaims, DefaultAuthenticationTypes.ApplicationCookie);
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignIn(claimsIdentity);


            if (model.Login == "admin")
            {
                return RedirectToAction("AdminHome", "Gestion");
            }
            else if(statut.isLigue){
                return RedirectToAction("LigueHome", "Ligue");
            }
            else {
                return RedirectToAction("ResponsableHome", "Responsable");
            }
        }

        private IEnumerable<Claim> LoadRolesAdmin(string login)
        {
           
            yield return new Claim(ClaimTypes.Role, "Admin");
           
        }

        private IEnumerable<Claim> LoadRolesResponsable(string login)
        {
           
            yield return new Claim(ClaimTypes.Role, "Responsable");
            
        }

        private IEnumerable<Claim> LoadRolesResponsableLigue(string login)
        {
            yield return new Claim(ClaimTypes.Role, "ResponsableLigue");
        }

        private StatutConnection ValidateUser(string login, string password)
        {
            
            PolesDataContext bd = new PolesDataContext();
            StatutConnection statut = new StatutConnection();
            statut.isLigue = false;
            statut.Connected = false;
            if(login == "admin")
            {
                statut.Connected = (from i in bd.SuperAdmin
                                where i.Login == login && i.Password == encrypt(password)
                                select i).Count() != 0 ;
            }
             else
            {
                statut.Connected = (from i in bd.Responsable
                            where i.Login == login && i.Password == encrypt(password)
                            select i).Count() != 0;
            }
             if(!statut.Connected)
            {
                statut.Connected = (from i in bd.ResponsableLigue
                            where i.Login == login && i.Password == encrypt(password)
                            select i).Count() != 0;
                statut.isLigue = true;
            }
            return statut;  
        }

        [HttpGet]
        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();

            // Rediriger vers la page d'accueil :
            return RedirectToAction("Inscription", "Home");
        }

        public String encrypt(string mdp)
        {
            Byte[] clearBytes = new UnicodeEncoding().GetBytes(mdp);
            Byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
            string hashedText = BitConverter.ToString(hashedBytes);
            return hashedText;

        }

    }
}