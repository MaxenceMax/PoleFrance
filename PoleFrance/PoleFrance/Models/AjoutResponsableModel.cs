using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoleFrance.Models
{
    
    public class AjoutResponsableModel
    {

        [Required]
        [Display(Name = "Identifiant")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Nom")]
        public string Nom { get; set; }

        [Required]
        [Display(Name = "Prenom")]
        public string Prenom { get; set; }

        [Required]
        [Display(Name = "Mail")]
        public string AdresseEmail { get; set; }

        public decimal id { get; set; }

        [Required]
        [Display(Name = "Pole")]
        public decimal Poleid { get; set; }

   



    }
}