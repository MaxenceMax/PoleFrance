using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoleFrance.Models
{
    public class InformationSportiveModel
    {
        public int id { get; set; }
        public String Competition { get; set; }
        public String CategorieAge { get; set; }
        public String CategoriePoids { get; set; }
        public String Resultat { get; set; }
    }
}