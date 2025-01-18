using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MediaTekDocuments.manager
{
    /// <summary>
    /// Classe indépendante d'accès à une API REST avec éventuellement une "basic authorization"
    /// </summary>
    class ApiRest
    {
        /// <summary>
        /// Unique instance de la classe
        /// </summary>
        private static ApiRest instance = null;
        /// <summary>
        /// Objet de connexion à l'API
        /// </summary>
        private readonly HttpClient httpClient;
        /// <summary>
        /// Canal HTTP pour l'envoi du message et la récupération de la réponse
        /// </summary>
        private HttpResponseMessage httpResponse;

        /// <summary>
        /// Constructeur privé pour préparer la connexion (éventuellement sécurisée)
        /// </summary>
        /// <param name="uriApi">Adresse de l'API</param>
        /// <param name="authenticationString">Chaîne d'authentification</param>
        private ApiRest(String uriApi, String authenticationString = "")
        {
            httpClient = new HttpClient() { BaseAddress = new Uri(uriApi) };
            // Prise en compte dans l'URL de l'authentification (basic authorization), si elle n'est pas vide
            if (!String.IsNullOrEmpty(authenticationString))
            {
                String base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
            }
            // Configuration pour accepter le JSON
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Crée une instance unique de la classe
        /// </summary>
        /// <param name="uriApi">Adresse de l'API</param>
        /// <param name="authenticationString">Chaîne d'authentification (login:pwd)</param>
        /// <returns>Instance de ApiRest</returns>
        public static ApiRest GetInstance(String uriApi, String authenticationString)
        {
            if (instance == null)
            {
                instance = new ApiRest(uriApi, authenticationString);
            }
            return instance;
        }

        /// <summary>
        /// Envoi une demande à l'API et récupère la réponse
        /// </summary>
        /// <param name="methode">Verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">Message à envoyer dans le corps de la requête</param>
        /// <returns>Objet JSON contenant la réponse de l'API</returns>
        //public JObject RecupDistant(string methode, string message)
        //{
        //    try
        //    {
        //        var httpContent = new StringContent(message, Encoding.UTF8, "application/json");

        //        switch (methode)
        //        {
        //            case "GET":
        //                httpResponse = httpClient.GetAsync($"mediatekDocument.php?table={message}").Result;
        //                break;
        //            case "POST":
        //                httpResponse = httpClient.PostAsync("mediatekDocument.php", httpContent).Result;
        //                break;
        //            case "PUT":
        //                httpResponse = httpClient.PutAsync("mediatekDocument.php", httpContent).Result;
        //                break;
        //            case "DELETE":
        //                var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, "mediatekDocument.php")
        //                {
        //                    Content = httpContent
        //                };
        //                httpResponse = httpClient.SendAsync(deleteRequest).Result;
        //                break;
        //            default:
        //                throw new InvalidOperationException("Méthode HTTP non supportée");
        //        }

        //        var contentType = httpResponse.Content.Headers.ContentType?.MediaType;
        //        Console.WriteLine("Type de contenu de la réponse : " + contentType);

        //        if (contentType == "application/json")
        //        {
        //            var responseContent = httpResponse.Content.ReadAsStringAsync().Result;
        //            Console.WriteLine("Réponse brute de l'API : " + responseContent);

        //            // Utilisation de JsonTextReader pour une lecture plus robuste
        //            using (var stringReader = new StringReader(responseContent))
        //            using (var jsonReader = new JsonTextReader(stringReader))
        //            {
        //                var jsonResponse = JObject.Load(jsonReader);
        //                return jsonResponse;
        //            }
        //        }
        //        else
        //        {
        //            throw new InvalidOperationException($"Type de contenu non supporté : {contentType}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Erreur lors de l'accès à l'API : {ex.Message}");
        //        return new JObject();
        //    }
        //}
        public JObject RecupDistant(string methode, string table, string id = null)
        {
            try
            {
                string url = $"mediatekDocument.php?table={Uri.EscapeDataString(table)}";
                if (!string.IsNullOrEmpty(id))
                {
                    url += $"&id={Uri.EscapeDataString(id)}";
                }

                Console.WriteLine("URL API: " + url); // Débogage de l'URL

                switch (methode)
                {
                    case "GET":
                        httpResponse = httpClient.GetAsync(url).Result;
                        break;
                    case "POST":
                        var httpContent = new StringContent("", Encoding.UTF8, "application/json");
                        httpResponse = httpClient.PostAsync("mediatekDocument.php", httpContent).Result;
                        break;
                    case "PUT":
                        httpContent = new StringContent("", Encoding.UTF8, "application/json");
                        httpResponse = httpClient.PutAsync("mediatekDocument.php", httpContent).Result;
                        break;
                    case "DELETE":
                        httpContent = new StringContent("", Encoding.UTF8, "application/json");
                        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, "mediatekDocument.php")
                        {
                            Content = httpContent
                        };
                        httpResponse = httpClient.SendAsync(deleteRequest).Result;
                        break;
                    default:
                        throw new InvalidOperationException("Méthode HTTP non supportée");
                }

                var contentType = httpResponse.Content.Headers.ContentType?.MediaType;
                Console.WriteLine("Type de contenu de la réponse : " + contentType);

                if (contentType == "application/json")
                {
                    var responseContent = httpResponse.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("Réponse brute de l'API : " + responseContent);

                    using (var stringReader = new StringReader(responseContent))
                    using (var jsonReader = new JsonTextReader(stringReader))
                    {
                        var jsonResponse = JObject.Load(jsonReader);
                        return jsonResponse;
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Type de contenu non supporté : {contentType}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'accès à l'API : {ex.Message}");
                return new JObject();
            }
        }

    }

}

