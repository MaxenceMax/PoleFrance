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
            if (!ValidateUser(model.Login, model.Password))
            {
                ModelState.AddModelError(string.Empty, "Le nom d'utilisateur ou le mot de passe est incorrect.");
                return View(model);
            }

            // L'authentification est réussie, 
            // injecter l'identifiant utilisateur dans le cookie d'authentification :
          /*  var loginClaim = new Claim(ClaimTypes.NameIdentifier, model.Login);
            var claimsIdentity = new ClaimsIdentity(new[] { loginClaim }, DefaultAuthenticationTypes.ApplicationCookie);
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignIn(claimsIdentity); */

            

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
            else {
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
            else {
                return RedirectToAction("Index", "Responsable");
            }

            /*
            // Rediriger vers l'URL d'origine :
            if (Url.IsLocalUrl(ViewBag.ReturnUrl))
                return Redirect(ViewBag.ReturnUrl);
            // Par défaut, rediriger vers la page d'accueil :
            return RedirectToAction("Inscription", "Home"); */
        }

        private IEnumerable<Claim> LoadRolesAdmin(string login)
        {
           
            yield return new Claim(ClaimTypes.Role, "Admin");
           
        }

        private IEnumerable<Claim> LoadRolesResponsable(string login)
        {
           
            yield return new Claim(ClaimTypes.Role, "Responsable");
        }

        private bool ValidateUser(string login, string password)
        {

             PolesDataContext bd = new PolesDataContext();

             bool connecte = false;

             if(login == "admin")
            {
                int numCount = (from i in bd.SuperAdmin
                                where i.Login == login && i.Password == encrypt(password)
                                select i).Count();

                if (numCount != 0)
                {
                    connecte = true;
                }

            }

             else
            {

                int numCount = (from i in bd.Responsable
                                where i.Login == login && i.Password == encrypt(password)
                                select i).Count();

                if (numCount != 0)
                {
                    connecte = true;
                }

            }

            


             return connecte;  

            /*
           bool connecte = false; 

            if (login == "test" && password == "test")
            {
                connecte = true;
            } 
           
            return connecte; */
            //return login == password;
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