using System;
using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using MediaTekDocuments.manager;

namespace MediaTekDocuments.view
{
    public partial class FrmAuthentification : Form
    {
        private readonly FrmAuthentificationController controller;
        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        public FrmAuthentification()
        {
            InitializeComponent();
            this.controller = new FrmAuthentificationController();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txbLoginAuthentUser_TextChanged(object sender, EventArgs e)
        {

        }

        //private void btnConnexionAuthentificationUser_Click(object sender, EventArgs e)
        //{
        //    string user = txbLoginAuthentUser.Text;
        //    string password = txbPasswordAuthentUser.Text;
        //    Console.WriteLine($"Login: {user}"); Console.WriteLine($"Password: {password}");
        //    if (!txbLoginAuthentUser.Text.Equals("") && !txbPasswordAuthentUser.Text.Equals(""))
        //    {
        //        if(!controller.GetUtilisateur(user, password))
        //        {
        //            MessageBox.Show("Erreur d'authentification!!", "ALERTE!");
        //            txbPasswordAuthentUser.Text = "";
        //        }
        //        else if(Service.Libelle == "culture")
        //        {
        //            MessageBox.Show("Vous n'êtes pas autorisé d'accès à l'applictaion!!", "ALERTE!");
        //            this.Close();
        //        }
        //        else
        //        {
        //            MessageBox.Show("connecté!!", "INFORMATION!");
        //            FrmMediatek frmMediatek = new FrmMediatek();
        //            frmMediatek.ShowDialog();
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Veuillez saisir tous les champs!!");
        //    }

        //}
        private async void btnConnexionAuthentificationUser_Click(object sender, EventArgs e)
        {
            try
            {
                string user = txbLoginAuthentUser.Text;
                string password = txbPasswordAuthentUser.Text;
                Console.WriteLine($"Login: {user}");
                Console.WriteLine($"Password: {password}");

                if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password))
                {
                    var apiRest = ApiRest.GetInstance("http://localhost/rest_mediatekdocuments", "admin:adminpwd");
                    var requestBody = new
                    {
                        table = "utilisateur",
                        login = user,
                        password = password
                    };

                    string message = JsonConvert.SerializeObject(requestBody);
                    var utilisateur = apiRest.RecupDistant("POST", message);

                    Console.WriteLine("Réponse de l'API: " + utilisateur);

                    if (utilisateur != null)
                    {
                        if ((string)utilisateur["code"] == "200" || (string)utilisateur["code"] == "409")
                        {
                            if (utilisateur["result"] != null)
                            {
                                // Stocker les informations du service
                                Service.Id = int.Parse(utilisateur["result"]["idService"].ToString());
                                Service.Libelle = utilisateur["result"]["libelle"]?.ToString() ?? "";
                                Console.WriteLine("Service ID: " + Service.Id);
                                Console.WriteLine("Service Libelle: " + Service.Libelle);

                                MessageBox.Show("Connecté!!", "INFORMATION!");
                                FrmMediatek frmMediatek = new FrmMediatek();
                                frmMediatek.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Erreur: les détails de l'utilisateur sont manquants", "ALERTE!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Erreur d'authentification!!", "ALERTE!");
                            txbPasswordAuthentUser.Text = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erreur d'authentification!!", "ALERTE!");
                        txbPasswordAuthentUser.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez saisir tous les champs!!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la tentative de connexion : " + ex.Message);
            }
        }






        private static async Task<JObject> EnvoyerUtilisateur(string login, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var byteArray = Encoding.ASCII.GetBytes("admin:adminpwd");
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    var data = new Dictionary<string, string>
            {
                { "table", "utilisateur" },
                { "login", login },
                { "password", password }
            };

                    var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://localhost/rest_mediatekdocuments/mediatekDocument.php", content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<JObject>(responseString);

                    Console.WriteLine("Réponse brute de l'API : " + responseString);

                    return result;
                }
                catch (HttpRequestException httpRequestEx)
                {
                    Console.WriteLine("HttpRequestException: " + httpRequestEx.Message);
                    if (httpRequestEx.InnerException != null)
                    {
                        Console.WriteLine("InnerException: " + httpRequestEx.InnerException.Message);
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur lors de l'envoi de la requête : " + ex.Message);
                    return null;
                }
            }
        }



    }










}

