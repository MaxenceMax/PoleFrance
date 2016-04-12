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

        [Required]
        [Display(Name = "Catégorie de poids actuelle")]
        public String CategoriePoidsActuelle { get; set; }

        [Display(Name ="Redirection")]
        public bool Redirection { get; set; }

        [Display(Name ="Demande d'internat")]
        public bool Internat { get; set; }

        [Display(Name ="Autorisation parental")]
        public bool AutorisationParent { get; set; }

        [Required]
        [Display(Name ="Ville")]
        public String Ville { get; set; }

        [Required]
        [Display (Name ="Code postal")]
        public String CodePostal { get; set; }

        [Required]
        [Display (Name ="Rue")]
        public String Rue { get; set; }

        [Display (Name ="Ligue")]
        public String Ligue { get; set; }

        [Required]
        [Display (Name ="Nom")]
        public String Nom { get; set; }

        [Required]
        [Display (Name ="Prénom")]
        public String Prenom { get; set;}

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display (Name ="Email")]
        public String AdresseEmail { get; set; }

        [Required]
        [Display (Name ="Date de naissance")]
        public String DateNaissance { get; set; }

        [Required]
        [Display (Name ="Sexe")]
        public String Sexe { get; set; }

        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Le numéro de téléphone renseigné n'est pas valide")]
        [Display (Name ="Téléphone")]
        public String Telephone { get; set; }

        [Required]
        [Display (Name ="Poids")]
        public String Poids { get; set; }

        [Required]
        [Display (Name ="Taille")]
        public String Taille { get; set; }

        [Required]
        [Display (Name ="Pôle actuel")]
        public String PoleActuel { get; set; }

        [Required]
        [Display (Name ="Catégorie d'age actuelle")]
        public String CategorieAgeActuelle { get; set; }

        [Display (Name ="Date démarche")]
        public String DateDemarche { get; set; }

        [Required]
        [Display(Name = "Téléphone des parents")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Le numéro de téléphone renseigné n'est pas valide")]
        public String TelephoneParents { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name ="Adresse email des parents")]
        public String AdresseEmailParent { get; set; }
     
    }
}