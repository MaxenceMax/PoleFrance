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
    }
}