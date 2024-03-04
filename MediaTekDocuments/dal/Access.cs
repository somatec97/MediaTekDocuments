using System;
using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// adresse de l'API
        /// </summary>
        private static readonly string uriApi = "http://localhost/rest_mediatekdocuments/";
        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// méthode HTTP pour select
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// méthode HTTP pour insert
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// méthode HTTP pour update
        /// </summary>
        private const string PUT = "PUT";
        /// <summary>
        /// méthode HTTP pour delete
        /// </summary>
        private const string DELETE = "DELETE";


        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        private Access()
        {
            String authenticationString;
            try
            {
                authenticationString = "admin:adminpwd";
                api = ApiRest.GetInstance(uriApi, authenticationString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if(instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre");
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon");
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public");
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre");
            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd");
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue");
            return lesRevues;
        }
        /// <summary>
        /// Retourne touts les documents de la BDD
        /// </summary>
        /// <param name="idDocument">id de document concerné</param>
        /// <returns>liste d'objet documents</returns>
        public List<Document> GetAllDocuments(string idDocument)

        {
            String jsonAllIdDocument = convertToJson("id", idDocument);
            List<Document> LesDocuments = TraitementRecup<Document>(GET, "document/" +jsonAllIdDocument);
            return LesDocuments;
        }


        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument);
            return lesExemplaires;
        }
        /// <summary>
        /// retourne les exemplaires d'un document
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns></returns>
        public List<Exemplaire> GetExemplairesDocument(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument);
            return lesExemplaires;
        }
        /// <summary>
        /// retourne les commandes d'un document
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns></returns>
        public List<CommandeDocument> GetCommandeDocument(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<CommandeDocument> lesCommandesDocuments = TraitementRecup<CommandeDocument>(GET, "commandeDocument/" + jsonIdDocument);
            return lesCommandesDocuments;
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(POST, "exemplaire/" + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false; 
        }
        /// <summary>
        /// ecrire un document en bdd
        /// </summary>
        /// <param name="Id">Id document à insérer</param>
        /// <param name="Titre">Titre document à insérer</param>
        /// <param name="Image"></param>
        /// <param name="IdGenre">IdGenre document à insérer</param>
        /// <param name="IdPublic">IdPublic document à insérer</param>
        /// <param name="IdRayon">IdRayon document à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerDocument(string Id, string Titre, string Image, string IdGenre, string IdPublic, string IdRayon)
        {
           
            String jsonDocument = "{ \"id\" : \"" + Id + "\", \"titre\" : \"" + Titre + "\", \"image\" : \"" + Image + "\", \"idGenre\" : \"" + IdGenre + "\", \"idPublic\" : \"" + IdPublic + "\", \"idRayon\" : \"" + IdRayon + "\"}";
            Console.WriteLine("jsonDocument" + jsonDocument);

            try
            {
                //récupération doit d'une liste vide (requête ok) soit de null (erreur)
                List<Document> liste = TraitementRecup<Document>(POST, "document/" + jsonDocument);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return false;
        }
        /// <summary>
        /// modification d'un document en bdd
        /// </summary>
        /// <param name="Id">id à modifier</param>
        /// <param name="Titre">titre à modifier</param>
        /// <param name="Image">image à modifier</param>
        /// <param name="IdGenre">idGenre à modifier</param>
        /// <param name="IdPublic">idPublic à modifier</param>
        /// <param name="IdRayon">idrayon à modifier</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool EditDocument(string Id, string Titre, string Image, string IdGenre, string IdPublic, string IdRayon)
        {

            String jsonEditDocument = "{ \"id\" : \"" + Id + "\", \"titre\" : \"" + Titre + "\", \"image\" : \"" + Image + "\", \"idGenre\" : \"" + IdGenre + "\", \"idPublic\" : \"" + IdPublic + "\", \"idRayon\" : \"" + IdRayon + "\"}";
            Console.WriteLine("jsonEditDocument" + jsonEditDocument);

            try
            {
                //récupération doit d'une liste vide (requête ok) soit de null (erreur)
                List<Document> liste = TraitementRecup<Document>(PUT, "document/" + Id+ "/"+ jsonEditDocument);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return false;
        }
        /// <summary>
        /// supprimer un document en bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public bool DeleteDocument(string Id)
        {
            string jsonDeleteDocument = "{\"id\" : \"" + Id + "\"}";
            Console.WriteLine("jsonDeleteDocument" + jsonDeleteDocument);
            try
            {
                //récupération doit d'une liste vide (requête ok) soit de null (erreur)
                List<Document> liste = TraitementRecup<Document>(DELETE, "document/" + Id + "/" + jsonDeleteDocument);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

        }


        /// <summary>
        /// ecrire un livre en bdd
        /// </summary>
        /// <param name="Id">Id à insérer</param>
        /// <param name="Isbn">Isbn à insérer</param>
        /// <param name="Auteur">Auteur à insérer</param>
        /// <param name="Collection">Collection à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerLivre(string Id, string Isbn, string Auteur, string Collection)
        {
            String jsonCreerLivre = "{ \"id\" : \"" + Id + "\", \"isbn\" : \"" + Isbn + "\", \"auteur\" : \"" + Auteur + "\", \"collection\" : \"" + Collection + "\"}";
            Console.WriteLine("jsonCreerLivre" + jsonCreerLivre);
            try
            {
                //récupération doit d'une liste vide (requête ok) soit de null (erreur)
                List<Livre> liste = TraitementRecup<Livre>(POST, "livre/" + jsonCreerLivre);
                return (liste != null);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

        }
        /// <summary>
        /// modification d'un livre en bdd
        /// </summary>
        /// <param name="Id">id à modifier</param>
        /// <param name="Isbn">isbn à modifier</param>
        /// <param name="Auteur">auteur à modifier</param>
        /// <param name="Collection">collection à modifier</param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public bool EditLivre(string Id, string Isbn, string Auteur, string Collection)
        {
            String jsonEditLivre = "{ \"id\" : \"" + Id + "\", \"isbn\" : \"" + Isbn + "\", \"auteur\" : \"" + Auteur + "\", \"collection\" : \"" + Collection + "\"}";
            Console.WriteLine("jsonEditLivre" + jsonEditLivre);
            try
            {
                //récupération doit d'une liste vide (requête ok) soit de null (erreur)
                List<Livre> liste = TraitementRecup<Livre>(PUT, "livre/" +Id+"/" + jsonEditLivre);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

        }
        /// <summary>
        /// supprimer un livre en bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public bool DeleteLivre(string Id)
        {
            string jsonDeleteLivre = "{\"id\" : \"" + Id + "\"}";
            Console.WriteLine("jsonDeleteLivre" + jsonDeleteLivre);
            try
            {
                //récupération doit d'une liste vide (requête ok) soit de null (erreur)
                List<Livre> liste = TraitementRecup<Livre>(DELETE, "livre/" + Id + "/" + jsonDeleteLivre);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

        }
        /// <summary>
        /// ajouter un dvd en bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Duree"></param>
        /// <param name="Realisateur"></param>
        /// <param name="Synopsis"></param>
        /// <returns></returns>
        public bool CreerDvd(string Id, int Duree, string Realisateur, string Synopsis)
        {
            String jsonCreerDvd = "{ \"id\" : \"" + Id + "\", \"duree\" : \"" + Duree + "\", \"realisateur\" : \"" + Realisateur + "\", \"synopsis\" : \"" + Synopsis + "\"}";
            Console.WriteLine("jsonCreerDvd" + jsonCreerDvd);
            try
            {
                //récupération doit d'une liste vide (requête ok) soit de null (erreur)
                List<Dvd> liste = TraitementRecup<Dvd>(POST, "dvd/" + jsonCreerDvd);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

        }
        /// <summary>
        /// modifier un dvd en bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Duree"></param>
        /// <param name="Realisateur"></param>
        /// <param name="Synopsis"></param>
        /// <returns></returns>
        public bool EditDvd(string Id, int Duree, string Realisateur, string Synopsis)
        {
            String jsonEditDvd = "{ \"id\" : \"" + Id + "\", \"duree\" : \"" + Duree + "\", \"realisateur\" : \"" + Realisateur + "\", \"synopsis\" : \"" + Synopsis + "\"}";
            Console.WriteLine("jsonEditDvd" + jsonEditDvd);
            try
            {
                //récupération doit d'une liste vide (requête ok) soit de null (erreur)
                List<Dvd> liste = TraitementRecup<Dvd>(PUT, "dvd/" + Id + "/" + jsonEditDvd);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

        }
        /// <summary>
        /// supprimer un dvd en bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteDvd(string Id)
        {
            string jsonDeleteDvd = "{\"id\" : \"" + Id + "\"}";
            Console.WriteLine("jsonDeleteDvd" + jsonDeleteDvd);
            try
            {
                //récupération doit d'une liste vide (requête ok) soit de null (erreur)
                List<Dvd> liste = TraitementRecup<Dvd>(DELETE, "dvd/" + Id + "/" + jsonDeleteDvd);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

        }
        /// <summary>
        /// ajouter une revue en bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="periodicite"></param>
        /// <param name="delaiMiseADispo"></param>
        /// <returns></returns>
        public bool CreerRevue(string Id, string periodicite, int delaiMiseADispo)
        {
            String jsonCreerRevue = "{ \"id\" : \"" + Id + "\", \"periodicite\" : \"" + periodicite + "\", \"delaiMiseADispo\" : \"" + delaiMiseADispo +  "\"}";
            Console.WriteLine("jsonCreerRevue" + jsonCreerRevue);
            try
            {
                //récupération doit d'une liste vide (requête ok) soit de null (erreur)
                List<Revue> liste = TraitementRecup<Revue>(POST, "revue/" + jsonCreerRevue);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

        }
        /// <summary>
        /// modifier une revue en bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="periodicite"></param>
        /// <param name="delaiMiseADispo"></param>
        /// <returns></returns>
        public bool EditRevue(string Id, string periodicite, int delaiMiseADispo)
        {
            String jsonEditRevue = "{ \"id\" : \"" + Id + "\", \"periodicite\" : \"" + periodicite + "\", \"delaiMiseADispo\" : \"" + delaiMiseADispo +  "\"}";
            Console.WriteLine("jsonEditRevue" + jsonEditRevue);
            try
            {
                //récupération doit d'une liste vide (requête ok) soit de null (erreur)
                List<Revue> liste = TraitementRecup<Revue>(PUT, "revue/" + Id + "/" + jsonEditRevue);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

        }
        /// <summary>
        /// supprimer une revue en bdd
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteRevue(string Id)
        {
            string jsonDeleteRevue = "{\"id\" : \"" + Id + "\"}";
            Console.WriteLine("jsonDeleteRevue" + jsonDeleteRevue);
            try
            {
                //récupération doit d'une liste vide (requête ok) soit de null (erreur)
                List<Revue> liste = TraitementRecup<Revue>(DELETE, "revue/" + Id + "/" + jsonDeleteRevue);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

        }

        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T> (String methode, String message)
        {
            List<T> liste = new List<T>();
            try
            {
                JObject retour = api.RecupDistant(methode, message);
                // extraction du code retourné
                String code = (String)retour["code"];
                if (code.Equals("200"))
                {
                    // dans le cas du GET (select), récupération de la liste d'objets
                    if (methode.Equals(GET))
                    {
                        String resultString = JsonConvert.SerializeObject(retour["result"]);
                        // construction de la liste d'objets à partir du retour de l'api
                        liste = JsonConvert.DeserializeObject<List<T>>(resultString, new CustomBooleanJsonConverter());
                    }
                }
                else
                {
                    Console.WriteLine("code erreur = " + code + " message = " + (String)retour["message"]);
                }
            }catch(Exception e)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : "+e.Message);
                Environment.Exit(0);
            }
            return liste;
        }

        /// <summary>
        /// Convertit en json un couple nom/valeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="valeur"></param>
        /// <returns>couple au format json</returns>
        private String convertToJson(Object nom, Object valeur)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            dictionary.Add(nom, valeur);
            return JsonConvert.SerializeObject(dictionary);
        }

        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// Modification du convertisseur Json pour prendre en compte les booléens
        /// classe trouvée sur le site :
        /// https://www.thecodebuzz.com/newtonsoft-jsonreaderexception-could-not-convert-string-to-boolean/
        /// </summary>
        private sealed class CustomBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Convert.ToBoolean(reader.ValueType == typeof(string) ? Convert.ToByte(reader.Value) : reader.Value);
            }

            public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }

    }
}
