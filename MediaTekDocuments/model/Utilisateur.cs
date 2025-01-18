using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class Utilisateur
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public int IdService { get; set; }
        public string Libelle { get; set; }

        public Utilisateur(string login, string password, int idService, string libelle)
        {
            this.Login = login;
            this.Password = password;
            this.IdService = idService;
            this.Libelle = libelle;
        }
    }
}
