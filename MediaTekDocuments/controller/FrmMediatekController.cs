using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    class FrmMediatekController
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
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return access.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }
        /// <summary>
        /// creer un exemplaire d'un document dans la bdd
        /// </summary>
        /// <param name="idDocuement"></param>
        /// <returns></returns>
        public List<Exemplaire> GetExemplairesDocument(string idDocuement)
        {
            return access.GetExemplairesDocument(idDocuement);
        }
        /// <summary>
        /// creer une commande d'un document dans la bdd
        /// </summary>
        /// <param name="idDocuement"></param>
        /// <returns></returns>
        public List<CommandeDocument> GetCommandeDocument(string idDocuement)
        {
            return access.GetCommandeDocument(idDocuement);
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
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
        public bool EditDvd(string Id, int Duree, string Realisateur, string Synopsis)
        {
            return access.EditDvd(Id, Duree, Realisateur, Synopsis);
        }
        /// <summary>
        /// supprimer un dvd dans la bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
        public bool EditRevue(string Id, string periodicite, int delaiMiseADispo)
        {
            return access.EditRevue(Id, periodicite, delaiMiseADispo);
        }
        /// <summary>
        /// supprimer une revue dans la bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteRevue(string Id)
        {
            return access.DeleteRevue(Id);

        }


    }
}
