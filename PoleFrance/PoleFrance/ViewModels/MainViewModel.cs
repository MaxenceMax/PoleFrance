using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoleFrance.Models
{
    public class MainModel
    {
        public Candidature Candidature { get; set; }
        public Pole Pole { get; set; }
        public bool Redirection { get; set; }
        public bool Internat { get; set; }
        public bool AutorisationParent { get; set; }

        public InformationSportive Information1 { get; set; }
        public InformationSportive Information2 { get; set; }
        public InformationSportive Information3 { get; set; }

        public InformationScolaire InformationScolaire { get; set; }
        public SouhaitScolaire SouhaitScolaire { get; set; }
        public Langue LV1 { get; set; }
        public Langue LV2 { get; set; }
        public Options option { get; set; }
    }
}