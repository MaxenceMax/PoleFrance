using PoleFrance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoleFrance.ViewModels
{
    public class AffichageCandidature
    {
        public PoleCandidature PoleCandidature { get; set; }
        public int Traitement { get; set; }
        public decimal polecandidatureId { get; set; }
    }
}