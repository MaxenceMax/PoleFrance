using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PoleFrance.Models
{
    public class CandidatureViewModel
    {
        [Required]
        [Display(Name="Numéro de licence")]
        public string NumLicencie { get; set;}

        [Display(Name ="Redirection")]
        public Boolean Redirection { get; set; }

        [Display(Name ="Demande d'internat")]
        public Boolean Internat { get; set; }

        [Display(Name ="Autorisation parental")]
        public Boolean AutorisationParental { get; set; }

        [Display(Name ="Ville")]
        public String Ville { get; set; }

        [Display (Name ="Code postal")]
        public String CodePostal { get; set; }

        [Display (Name ="Rue")]
        public String Rue { get; set; }

        [Display (Name ="Ligue")]
        public String Ligue { get; set; }

        [Display (Name ="Nom")]
        public String Nom { get; set; }

        [Display (Name ="Prénom")]
        public String Prenom { get; set;}

        [Display (Name ="Email")]
        public String Email { get; set; }

        [Display (Name ="Date de naissance")]
        public String DateNaissance { get; set; }

        [Display (Name ="Sexe")]
        public String Sexe { get; set; }

        [Display (Name ="Téléphone")]
        public String Telephone { get; set; }

    }
}