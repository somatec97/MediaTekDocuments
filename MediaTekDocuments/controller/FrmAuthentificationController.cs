using System;
using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.controller
{
    public class FrmAuthentificationController
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Access access;
        /// <summary>
        /// 
        /// </summary>
        public FrmAuthentificationController()
        {
            access = Access.GetInstance();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        //public bool GetUtilisateur(string login, string password)
        //{
        //    Utilisateur utilisateur = access.GetUtilisateur(login);
        //    if (utilisateur == null)
        //    {
        //        return false;
        //    }
        //    if (utilisateur.Password.Equals(password))
        //    {
        //        Service.Id = utilisateur.IdService;
        //        Service.Libelle = utilisateur.Libelle;
        //        return true;
        //    }
        //    return false;
        //}
        public bool GetUtilisateur(string login, string password)
        {
            try
            {
                Console.WriteLine($"Tentative d'authentification pour l'utilisateur: {login}");
                Utilisateur utilisateur = access.GetUtilisateur(login);

                if (utilisateur == null)
                {
                    Console.WriteLine("Utilisateur non trouvé.");
                    return false;
                }
                if (utilisateur.Password.Equals(password))
                {
                    Service.Id = utilisateur.IdService;
                    Service.Libelle = utilisateur.Libelle;
                    Console.WriteLine("Authentification réussie.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Mot de passe incorrect.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'authentification : " + ex.Message);
                return false;
            }
        }

    }
}
