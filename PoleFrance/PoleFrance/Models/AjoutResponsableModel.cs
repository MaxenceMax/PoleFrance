using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PoleFrance.Models
{
    public class AjoutResponsableModel
    {

        [Required]
        [Display(Name = "Identifiant")]
        public string Pseudo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Pass { get; set; }

        [Required]
        [Display(Name = "Nom")]
        public string Nom { get; set; }

        [Required]
        [Display(Name = "Prenom")]
        public string Prenom { get; set; }

        [Required]
        [Display(Name = "Mail")]
        public string Mail { get; set; }


      


    }
}