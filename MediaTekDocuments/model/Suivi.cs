using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class Suivi
    {
        public string Id { get; set; }
        public string Stade { get; set; }

        public Suivi(string id, string stade)
        {
            this.Id = id;
            this.Stade = stade;
        }
    }
}
