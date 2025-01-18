using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;
using System;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
     public class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }
        /// <summary>
        /// getter sur les documents
        /// </summary>
        /// <param name="id">id de document concerné</param>
        /// <returns>liste d'objet document</returns>
        public List<Document> GetAllDocuments(string id)
        {
            return access.GetAllDocuments(id);

        }
        /// <summary>
        /// getter sur les suivis
        /// </summary>
        /// <returns>liste d'objet suivi</returns>
        //public List<Suivi> GetAllSuivis()
        //{
        //    return access.GetAllSuivis();
        //}
        public List<Suivi> GetAllSuivis()
        {
            List<Suivi> lesSuivis = access.GetAllSuivis();
            Console.WriteLine("Les Suivis récupérés:");
            if (lesSuivis != null)
            {
                foreach (var suivi in lesSuivis)
                {
                    Console.WriteLine($"Id: {suivi.Id}, Stade: {suivi.Stade}");
                }
            }
            else
            {
                Console.WriteLine("Les Suivis sont null");
            }
            return lesSuivis;
        }


        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return access.GetExemplairesRevue(idDocuement);
        }
       
        /// <summary>
        /// récupère les abonnements d'une revue
        /// </summary>
        /// <param name="idDocuement"></param>
        /// <returns></returns>
        public List<Abonnement> GetAbonnementRevue(string idDocuement)
        {
            return access.GetAbonnementRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaireRevue(string id, int numero, DateTime dateachat, string photo, string idEtat)
        {
            return access.CreerExemplaireRevue(id, numero, dateachat, photo, idEtat);
        }
        
        /// <summary>
        /// modifier l'etat d'un exemplaire d'un document dans la bdd
        /// </summary>
        /// <param name="exemplaire"></param>
        /// <returns></returns>
        public bool EditeEtatExemplaireDocument(Exemplaire exemplaire)
        {
            return access.EditeEtatExemplaireDocument(exemplaire);
        }
        /// <summary>
        /// Supprimer un exemplaire d'un document dans la bdd
        /// </summary>
        /// <param name="exemplaire"></param>
        /// <returns></returns>
        public bool DeleteExemplaireDocument(Exemplaire exemplaire)
        {
            return access.DeleteExemplaireDocument(exemplaire);
        }

       
        /// <summary>
        /// recuperer un exemplaire d'un document dans la bdd
        /// </summary>
        /// <param name="idDocuement"></param>
        /// <returns>Liste d'objets Exemplaire d'un document</returns>
        public List<Exemplaire> GetExemplairesDocument(string idDocuement)
        {
            return access.GetExemplairesDocument(idDocuement);
        }
        /// <summary>
        /// creer une commande d'un document dans la bdd
        /// </summary>
        /// <param name="idDocuement"></param>
        /// <returns>Liste d'objets commande d'un document</returns>
        public List<CommandeDocument> GetCommandeDocument(string idDocuement)
        {
            return access.GetCommandeDocument(idDocuement);
        }
        /// <summary>
        /// recupère les abonnements qui expirent dans 30 jours
        /// </summary>
        /// <returns></returns>
        public List<Abonnement> GetAbonnementsExpires()
        {
            return access.GetAbonnementExpire();
        }
        /// <summary>
        /// recupere les etats
        /// </summary>
        /// <returns></returns>
        public List<Etat> GetAllEtats()
        {
            return access.GetAllEtats();
        }
        /// <summary>
        /// creer un document dans la bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Titre"></param>
        /// <param name="Image"></param>
        /// <param name="IdGenre"></param>
        /// <param name="IdPublic"></param>
        /// <param name="IdRayon"></param>
        /// <returns>true si la creation a pu se faire</returns>
        public bool CreerDocument(string Id, string Titre, string Image, string IdGenre, string IdPublic, string IdRayon)
        {
            return access.CreerDocument(Id, Titre, Image, IdGenre, IdPublic, IdRayon);

        }
        /// <summary>
        /// modification d'un document dans la bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Titre"></param>
        /// <param name="Image"></param>
        /// <param name="IdGenre"></param>
        /// <param name="IdPublic"></param>
        /// <param name="IdRayon"></param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool EditDocument(string Id, string Titre, string Image, string IdGenre, string IdPublic, string IdRayon)
        {
            return access.EditDocument(Id, Titre, Image, IdGenre, IdPublic, IdRayon);

        }
        /// <summary>
        /// suppression d'un document dans la bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteDocument(string Id)
        {
            return access.DeleteDocument(Id);

        }
        /// <summary>
        /// creer un livre dans la bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Isbn"></param>
        /// <param name="Auteur"></param>
        /// <param name="Collection"></param>
        /// <returns>true si la creation a pu se faire</returns>
        public bool CreerLivre(string Id, string Isbn, string Auteur, string Collection)
        {
            return access.CreerLivre(Id, Isbn, Auteur, Collection);
        }
        /// <summary>
        /// modifier un livre dans la bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Isbn"></param>
        /// <param name="Auteur"></param>
        /// <param name="Collection"></param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool EditLivre(string Id, string Isbn, string Auteur, string Collection)
        {
            return access.EditLivre(Id, Isbn, Auteur, Collection);
        }
        /// <summary>
        /// supprimer un livre dans la bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool DeleteLivre(string Id)
        {
            return access.DeleteLivre(Id);

        }
        /// <summary>
        /// ajouter un dvd dans la bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Duree"></param>
        /// <param name="Realisateur"></param>
        /// <param name="Synopsis"></param>
        /// <returns>true si la creation a pu se faire</returns>
        public bool CreerDvd(string Id, int Duree, string Realisateur, string Synopsis)
        {
            return access.CreerDvd(Id, Duree, Realisateur, Synopsis);
        }
        /// <summary>
        /// modifier un dv dans la bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Duree"></param>
        /// <param name="Realisateur"></param>
        /// <param name="Synopsis"></param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool EditDvd(string Id, int Duree, string Realisateur, string Synopsis)
        {
            return access.EditDvd(Id, Duree, Realisateur, Synopsis);
        }
        /// <summary>
        /// supprimer un dvd dans la bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool DeleteDvd(string Id)
        {
            return access.DeleteDvd(Id);

        }
        /// <summary>
        /// ajouter une revue dans la bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="periodicite"></param>
        /// <param name="delaiMiseADispo"></param>
        /// <returns>true si la creation a pu se faire</returns>
        public bool CreerRevue(string Id, string periodicite, int delaiMiseADispo)
        {
            return access.CreerRevue(Id, periodicite, delaiMiseADispo);
        }
        /// <summary>
        /// modifier une revue dans la bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="periodicite"></param>
        /// <param name="delaiMiseADispo"></param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool EditRevue(string Id, string periodicite, int delaiMiseADispo)
        {
            return access.EditRevue(Id, periodicite, delaiMiseADispo);
        }
        /// <summary>
        /// supprimer une revue dans la bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool DeleteRevue(string Id)
        {
            return access.DeleteRevue(Id);

        }
        /// <summary>
        /// ajouter une commande dans la bdd
        /// </summary>
        /// <param name="commande"></param>
        /// <returns>true si la creation a pu se faire</returns>
        public bool CreerCommande(Commande commande)
        {
            return access.CreerCommande(commande);
        }
        /// <summary>
        /// ajouter une commande d'un document dans la bdd
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nbExemplaire"></param>
        /// <param name="idLivreDvd"></param>
        /// <param name="idSuivi"></param>
        /// <returns>true si la creation a pu se faire</returns>
        public bool CreerCommandeDocument(string id, int nbExemplaire, string idLivreDvd, string idSuivi)
        {
            return access.CreerCommandeDocument(id, nbExemplaire, idLivreDvd, idSuivi);
        }
        /// <summary>
        /// modifier l'étape de suivi de la commande d'un document dans la bdd
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nbExemplaire"></param>
        /// <param name="idLivreDvd"></param>
        /// <param name="idSuivi"></param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool EditSuiviCommandeDocument(string id, int nbExemplaire, string idLivreDvd, string idSuivi)
        {
            return access.EditSuiviCommandeDocument(id, nbExemplaire, idLivreDvd, idSuivi);
        }
        /// <summary>
        /// supprimer une commande du document dans la bdd
        /// </summary>
        /// <param name="commandeDocument"></param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool DeleteCommandeDocument(CommandeDocument commandeDocument)
        {
            return access.DeleteCommandeDocument(commandeDocument);
        }
        /// <summary>
        /// creer un abonnement d'une revue dans la bdd
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateFinAbonnement"></param>
        /// <param name="idRevue"></param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public bool CreerAbonnementRevue(string id, DateTime dateFinAbonnement, string idRevue)
        {
            return access.CreerAbonnementRevue(id, dateFinAbonnement, idRevue);
        }
        /// <summary>
        /// Supprimer l'abonnement d'une revue dans la bdd
        /// </summary>
        /// <param name="abonnement"></param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool DeleteAbonnementRevue(Abonnement abonnement)
        {
            return access.DeleteAbonnementRevue(abonnement);
        }


    }
}
