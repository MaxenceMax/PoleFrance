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

        public List<Models.InformationSportive> InformationsSportives { get; set; }

        public InformationScolaire InformationScolaire { get; set; }
        public SouhaitScolaire SouhaitScolaire { get; set; }
        public Langue LV1 { get; set; }
        public Langue LV2 { get; set; }
        public Options option { get; set; }



    }
}