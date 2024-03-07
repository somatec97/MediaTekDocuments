using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class CommandeDocument : Commande
    {
        public string IdLivreDvd { get; set; }
        public int NbExemplaire { get; set; }
        public string IdSuivi { get; set; }
        public string Stade { get; set; }

        public CommandeDocument(string id,DateTime dateCommande,double montant, string idLivreDvd, int nbExemplaire, string idSuivi, string stade) :base(id, dateCommande, montant)
        {
            this.IdLivreDvd = idLivreDvd;
            this.NbExemplaire = nbExemplaire;
            this.IdSuivi = idSuivi;
            this.Stade = stade;
        }
    }
}
