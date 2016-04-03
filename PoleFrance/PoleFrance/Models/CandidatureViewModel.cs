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
        [Display(Name = "Numéro de licence")]
        public string NumLicencie { get; set; }

        [Display(Name ="Redirection")]
        public bool Redirection { get; set; }

        [Display(Name ="Demande d'internat")]
        public bool Internat { get; set; }

        [Display(Name ="Autorisation parental")]
        public bool AutorisationParent { get; set; }

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
        public String AdresseEmail { get; set; }

        [Display (Name ="Date de naissance")]
        public String DateNaissance { get; set; }

        [Display (Name ="Sexe")]
        public String Sexe { get; set; }

        [Display (Name ="Téléphone")]
        public String Telephone { get; set; }

        [Display (Name ="Poids")]
        public String Poids { get; set; }

        [Display (Name ="Taille")]
        public String Taille { get; set; }

        [Display (Name ="Pôle actuel")]
        public String PoleActuel { get; set; }

        [Display (Name ="Catégorie d'age actuelle")]
        public String CategorieAgeActuelle { get; set; }

        [Display (Name ="Date démarche")]
        public String DateDemarche { get; set; }

        [Required]
        [Display(Name = "Téléphone des parents")]
        public String TelephoneParents { get; set; }

        [Required]
        [Display(Name ="Adresse email des parents")]
        public String AdresseEmailParent { get; set; }
        
    }
}