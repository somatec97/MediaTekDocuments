﻿using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);
            RemplirCbxNewGenreLivre();
            RemplirCbxNewPublicLivre();
            RemplirCbxNewRayonLivre();
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }
        /// <summary>
        /// remplire la conbobox du genre du livre a ajouter ou modifier
        /// </summary>
        /// <returns></returns>
        private string RemplirCbxNewGenreLivre()
        {
            List<Categorie> LesGenresLivres = controller.GetAllGenres();
            foreach (Categorie genre in LesGenresLivres)
            {
                cbxNewGenreLivre.Items.Add(genre.Libelle);
            }
            if (cbxNewGenreLivre.Items.Count > 0)
            {
                cbxNewGenreLivre.SelectedIndex = 0;
            }
            return cbxNewGenreLivre.SelectedItem?.ToString();
        }
        /// <summary>
        /// filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxNewGenreLivre_SelectedIndexChanged(object sender, EventArgs e)
        {
            RemplirCbxNewGenreLivre();
        }
        /// <summary>
        /// remplir la combobox du public du livre a ajouter ou modifier
        /// </summary>
        /// <returns></returns>
        private string RemplirCbxNewPublicLivre()
        {
            List<Categorie> LesPublicsLivres = controller.GetAllPublics();
            foreach (Categorie lePublic in LesPublicsLivres)
            {
                cbxNewPublicLivre.Items.Add(lePublic.Libelle);
            }
            if (cbxNewPublicLivre.Items.Count > 0)
            {
                cbxNewPublicLivre.SelectedIndex = 0;
            }
            return cbxNewPublicLivre.SelectedItem?.ToString();
        }
        /// <summary>
        /// filtre sur le public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxNewPublicLivre_SelectedIndexChanged(object sender, EventArgs e)
        {

            RemplirCbxNewPublicLivre();
        }
        /// <summary>
        /// remplir la combobox du rayon du livre a ajouter ou modifier
        /// </summary>
        /// <returns></returns>
        private string RemplirCbxNewRayonLivre()
        {
            List<Categorie> LesRayonsLivres = controller.GetAllRayons();
            foreach (Categorie rayon in LesRayonsLivres)
            {
                cbxNewRayonLivre.Items.Add(rayon.Libelle);
            }
            if (cbxNewRayonLivre.Items.Count > 0)
            {
                cbxNewRayonLivre.SelectedIndex = 0;
            }
            return cbxNewRayonLivre.SelectedItem?.ToString();
        }
        /// <summary>
        /// filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxNewRayonLivre_SelectedIndexChanged(object sender, EventArgs e)
        {
            RemplirCbxNewRayonLivre();

        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }

        /// <summary>
        /// vider les informations d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVideInfoLivres_Click(object sender, EventArgs e)
        {
            VideLivresInfos();

        }
        /// <summary>
        /// Id du genre qui correspond au genre de document
        /// </summary>
        /// <param name="genre">genre de document sélectionné</param>
        /// <returns>id du genre selectionné</returns>
        private string GetIdGenreDocument(string genre)
        {
            List<Categorie> lesGenresDocument = controller.GetAllGenres();
            foreach (Categorie cat in lesGenresDocument)
            {
                if (cat.Libelle == genre)
                {
                    return cat.Id;
                }
            }
            return null;
        }
        /// <summary>
        /// id dupublic qui correspond au public de document
        /// </summary>
        /// <param name="lePublic">lePublic du document sélectionné</param>
        /// <returns>id du public selectionné</returns>
        private string GetIdPublicDocument(string lePublic)
        {
            List<Categorie> lesPublicsDocument = controller.GetAllPublics();
            foreach (Categorie cat in lesPublicsDocument)
            {
                if (cat.Libelle == lePublic)
                {
                    return cat.Id;
                }
            }
            return null;
        }
        /// <summary>
        /// id du rayon qui correspond au rayon de document
        /// </summary>
        /// <param name="rayon">rayon du document sélectionné</param>
        /// <returns>id du public selectionné</returns>
        private string GetIdRayonDocument(string rayon)
        {
            List<Categorie> lesRayonsDocument = controller.GetAllRayons();
            foreach (Categorie cat in lesRayonsDocument)
            {
                if (cat.Libelle == rayon)
                {
                    return cat.Id;
                }
            }
            return null;
        }
        /// <summary>
        /// l'ajout d'un livre dans la bdd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjoutLivre_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumero.Text.Equals(" ") && !txbLivresTitre.Text.Equals(" ") && !txbLivresCollection.Text.Equals(" ") && !cbxNewGenreLivre.Text.Equals(" ") && !cbxNewPublicLivre.Text.Equals(" ") && !cbxNewRayonLivre.Text.Equals(" "))
            {
                try
                {
                    string id = txbLivresNumero.Text;
                    string titre = txbLivresTitre.Text;
                    string image = txbLivresImage.Text;
                    string isbn = txbLivresIsbn.Text;
                    string auteur = txbLivresAuteur.Text;
                    string collection = txbLivresCollection.Text;
                    string idGenre = GetIdGenreDocument(cbxNewGenreLivre.Text);
                    string idPublic = GetIdPublicDocument(cbxNewPublicLivre.Text);
                    string idRayon = GetIdRayonDocument(cbxNewRayonLivre.Text);
                    string genre = txbLivresGenre.Text;
                    string lePublic = txbLivresPublic.Text;
                    string rayon = txbLivresRayon.Text;
                    Document document = new Document(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon);
                    Livre livre = new Livre(id, titre, image, isbn, auteur, collection, idGenre, genre, idPublic, lePublic, idRayon, rayon);
                    var idLivreExist = controller.GetAllDocuments(id);
                    var idLivreNoExist = !idLivreExist.Any();

                    if (idLivreNoExist)
                    {
                        if (controller.CreerDocument(document.Id, document.Titre, document.Image, document.IdGenre, document.IdPublic, document.IdRayon) && controller.CreerLivre(livre.Id, livre.Isbn, livre.Auteur, livre.Collection))
                        {
                            lesLivres = controller.GetAllLivres();
                            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
                            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
                            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);
                            RemplirLivresListeComplete();
                            MessageBox.Show(" Le livre " + titre + " est bien ajouté ");
                        }
                    }
                    else
                    {
                        MessageBox.Show(" Le numéro de document existe déjà! saisir un autre numéro! ", "ERREUR!!");
                    }

                }
                catch
                {
                    MessageBox.Show(" Une erreur s'est produite!!", "ERREUR!!");
                }
            }
            else
            {
                MessageBox.Show(" Veuillez saisir tous les champs!", "INFORMATION!!");
            }
        }
        /// <summary>
        /// modification d'un livre dans la bdd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifLivre_Click(object sender, EventArgs e)
        {
            if (dgvLivresListe.SelectedRows.Count > 0)
            {
                Livre selectLivre = (Livre)dgvLivresListe.SelectedRows[0].DataBoundItem;
                string id = selectLivre.Id;
                string titre = txbLivresTitre.Text;
                string image = txbLivresImage.Text;
                string isbn = txbLivresIsbn.Text;
                string auteur = txbLivresAuteur.Text;
                string collection = txbLivresCollection.Text;
                string idGenre = GetIdGenreDocument(cbxNewGenreLivre.Text);
                string idPublic = GetIdPublicDocument(cbxNewPublicLivre.Text);
                string idRayon = GetIdRayonDocument(cbxNewRayonLivre.Text);
                if (!txbLivresNumero.Text.Equals("") && !txbLivresTitre.Text.Equals("") && !txbLivresCollection.Text.Equals("") && !cbxNewGenreLivre.Text.Equals("") && !cbxNewPublicLivre.Text.Equals("") && !cbxNewRayonLivre.Text.Equals(""))
                {
                    if (controller.EditDocument(id, titre, image, idGenre, idPublic, idRayon) && controller.EditLivre(id, isbn, auteur, collection))
                    {
                        lesLivres = controller.GetAllLivres();
                        RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
                        RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
                        RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);
                        RemplirLivresListeComplete();
                        MessageBox.Show("Le livre " + titre + " est bien modifié!");
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification du livre", "ERREUR!!");

                    }
                }
                else
                {
                    MessageBox.Show("Veuillez saisir tous les champs!!", "INFORMATION!!");
                }

            }
            else
            {
                MessageBox.Show("Veuillez selectioner une ligne!!", "INFORMATION!!");
            }

        }
        /// <summary>
        /// suppression d'un livre dans la bdd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSuppLivre_Click(object sender, EventArgs e)
        {
            Livre livre = (Livre)bdgLivresListe.Current;
            if (MessageBox.Show("êtes vous sûr de vouloir supprimer ce" + livre.Titre + "?", "CONFIRMATION!", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                var exmpLivre = controller.GetExemplairesDocument(livre.Id);
                var noExmpl = !exmpLivre.Any();
                var commandeLivre = controller.GetCommandeDocument(livre.Id);
                var noCommande = !commandeLivre.Any();
                if (noCommande && noExmpl)
                {
                    if (controller.DeleteLivre(livre.Id))
                    {
                        lesLivres = controller.GetAllLivres();
                        RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
                        RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
                        RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);
                        RemplirLivresListeComplete();
                        MessageBox.Show("Le livre " + livre.Titre + " est supprimer!");
                    }
                    else
                    {
                        MessageBox.Show("erreur!!", "ERREUR!");
                    }
                }
                else
                {
                    MessageBox.Show("Impossible de supprimer ce livre car il possède un ou plusieurs exemplaire(s), ou une ou plusieurs commande(s)", "ERREUR!!");
                }
            }
        }

        #endregion

        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirCbxNewGenreDvd();
            RemplirCbxNewPublicDvd();
            RemplirCbxNewRayonDvd();
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }
        /// <summary>
        /// vider les information dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVideDVD_Click(object sender, EventArgs e)
        {
            VideDvdInfos();
        }
        /// <summary>
        /// remplir la combobox du genre du dvd à ajouter ou modifier
        /// </summary>
        /// <returns></returns>
        private string RemplirCbxNewGenreDvd()
        {
            List<Categorie> LesGenresDvd = controller.GetAllGenres();
            foreach (Categorie genre in LesGenresDvd)
            {
                cbxNewGenreDVD.Items.Add(genre.Libelle);
            }
            if (cbxNewGenreDVD.Items.Count > 0)
            {
                cbxNewGenreDVD.SelectedIndex = 0;
            }
            return cbxNewGenreDVD.SelectedItem?.ToString();
        }
        /// <summary>
        /// remplir la combobox du public du dvd a ajouter ou modifier
        /// </summary>
        /// <returns></returns>
        private string RemplirCbxNewPublicDvd()
        {
            List<Categorie> LesPublicsDvd = controller.GetAllPublics();
            foreach (Categorie lePublic in LesPublicsDvd)
            {
                cbxNewPublicDVD.Items.Add(lePublic.Libelle);
            }
            if (cbxNewPublicDVD.Items.Count > 0)
            {
                cbxNewPublicDVD.SelectedIndex = 0;
            }
            return cbxNewPublicDVD.SelectedItem?.ToString();
        }
        /// <summary>
        /// remplir la combobox du rayon du dvd a ajouter ou modifier
        /// </summary>
        /// <returns></returns>
        private string RemplirCbxNewRayonDvd()
        {
            List<Categorie> LesRayonsDvd = controller.GetAllRayons();
            foreach (Categorie rayon in LesRayonsDvd)
            {
                cbxNewRayonDVD.Items.Add(rayon.Libelle);
            }
            if (cbxNewRayonDVD.Items.Count > 0)
            {
                cbxNewRayonDVD.SelectedIndex = 0;
            }
            return cbxNewRayonDVD.SelectedItem?.ToString();
        }
        /// <summary>
        /// ajouter un dvd dans la bdd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjoutDVD_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumero.Text.Equals(" ") && !txbDvdTitre.Text.Equals(" ") && !txbDvdRealisateur.Text.Equals(" ") && !txbDvdSynopsis.Text.Equals("") && !cbxNewGenreDVD.Text.Equals(" ") && !cbxNewPublicDVD.Text.Equals(" ") && !cbxNewRayonDVD.Text.Equals(" "))
            {
                try
                {
                    string id = txbDvdNumero.Text;
                    string titre = txbDvdTitre.Text;
                    string image = txbDvdImage.Text;
                    int duree = int.Parse(txbDvdDuree.Text);
                    string realisateur = txbDvdDuree.Text;
                    string synopsis = txbDvdSynopsis.Text;
                    string idGenre = GetIdGenreDocument(cbxNewGenreDVD.Text);
                    string idPublic = GetIdPublicDocument(cbxNewPublicDVD.Text);
                    string idRayon = GetIdRayonDocument(cbxNewRayonDVD.Text);
                    string genre = txbDvdGenre.Text;
                    string lePublic = txbDvdPublic.Text;
                    string rayon = txbDvdRayon.Text;
                    Document document = new Document(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon);
                    Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis, idGenre, genre, idPublic, lePublic, idRayon, rayon);
                    var idDvdExist = controller.GetAllDocuments(id);
                    var idDvdNoExist = !idDvdExist.Any();

                    if (idDvdNoExist)
                    {
                        if (controller.CreerDocument(document.Id, document.Titre, document.Image, document.IdGenre, document.IdPublic, document.IdRayon) && controller.CreerDvd(dvd.Id, dvd.Duree, dvd.Realisateur, dvd.Synopsis))
                        {
                            lesDvd = controller.GetAllDvd();
                            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
                            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
                            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
                            RemplirDvdListeComplete();
                            MessageBox.Show(" Le dvd " + titre + " est bien ajouté ");
                        }
                    }
                    else
                    {
                        MessageBox.Show(" Le numéro de document existe déjà! saisir un autre numéro! ", "ERREUR!!");
                    }

                }
                catch
                {
                    MessageBox.Show(" Une erreur s'est produite!!", "ERREUR!!");
                }
            }
            else
            {
                MessageBox.Show(" Veuillez saisir tous les champs!", "INFORMATION!!");
            }
        }
        /// <summary>
        /// modifier un dvd dans la bdd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnModifDVD_Click(object sender, EventArgs e)
        {
            if (dgvDvdListe.SelectedRows.Count > 0)
            {
                Dvd selectDvd = (Dvd)dgvDvdListe.SelectedRows[0].DataBoundItem;
                string id = selectDvd.Id;
                string titre = txbDvdTitre.Text;
                string image = txbDvdImage.Text;
                int duree = int.Parse(txbDvdDuree.Text);
                string realisateur = txbDvdRealisateur.Text;
                string synopsis = txbDvdSynopsis.Text;
                string idGenre = GetIdGenreDocument(cbxNewGenreDVD.Text);
                string idPublic = GetIdPublicDocument(cbxNewPublicDVD.Text);
                string idRayon = GetIdRayonDocument(cbxNewRayonDVD.Text);
                if (!txbDvdNumero.Text.Equals("") && !txbDvdTitre.Text.Equals("") && !txbDvdRealisateur.Text.Equals(" ") && !txbDvdSynopsis.Text.Equals("") && !cbxNewGenreDVD.Text.Equals("") && !cbxNewPublicDVD.Text.Equals("") && !cbxNewRayonDVD.Text.Equals(""))
                {
                    if (controller.EditDocument(id, titre, image, idGenre, idPublic, idRayon) && controller.EditDvd(id, duree, realisateur, synopsis))
                    {
                        lesDvd = controller.GetAllDvd();
                        RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
                        RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
                        RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
                        RemplirDvdListeComplete();
                        MessageBox.Show("Le dvd " + titre + " est bien modifié!");
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification du dvd", "ERREUR!!");

                    }
                }
                else
                {
                    MessageBox.Show("Veuillez saisir tous les champs!!", "INFORMATION!!");
                }

            }
            else
            {
                MessageBox.Show("Veuillez selectioner une ligne!!", "INFORMATION!!");
            }

        }
        /// <summary>
        /// supprimer un dvd dans la bdd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnSuppDVD_Click(object sender, EventArgs e)
        {
            Dvd dvd = (Dvd)bdgDvdListe.Current;
            if (MessageBox.Show("êtes vous sûr de vouloir supprimer ce" + dvd.Titre + "?", "CONFIRMATION!", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                var exmpDvd = controller.GetExemplairesDocument(dvd.Id);
                var noExmpl = !exmpDvd.Any();
                var commandeDvd = controller.GetCommandeDocument(dvd.Id);
                var noCommande = !commandeDvd.Any();
                if (noCommande && noExmpl)
                {
                    if (controller.DeleteDvd(dvd.Id))
                    {
                        lesDvd = controller.GetAllDvd();
                        RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
                        RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
                        RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
                        RemplirDvdListeComplete();
                        MessageBox.Show("Le dvd " + dvd.Titre + " est supprimer!");
                    }
                    else
                    {
                        MessageBox.Show("erreur!!", "ERREUR!");
                    }
                }
                else
                {
                    MessageBox.Show("Impossible de supprimer ce dvd car il possède un ou plusieurs exemplaire(s), ou une ou plusieurs commande(s)", "ERREUR!!");
                }
            }


        }
        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirCbxNewPublicRevue();
            RemplirCbxNewRayonRevue();
            RemplirCbxNewGenreRevue();
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }
        /// <summary>
        /// vider revues informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVideRevue_Click(object sender, EventArgs e)
        {
            VideRevuesInfos();
        }
        /// <summary>
        /// remplir la combobox du genre de la revue a ajouter ou a modifier
        /// </summary>
        /// <returns></returns>
        private string RemplirCbxNewGenreRevue()
        {
            List<Categorie> LesGenresRevue = controller.GetAllGenres();
            foreach (Categorie genre in LesGenresRevue)
            {
                cbxNewGenreRevue.Items.Add(genre.Libelle);
            }
            if (cbxNewGenreRevue.Items.Count > 0)
            {
                cbxNewGenreRevue.SelectedIndex = 0;
            }
            return cbxNewGenreRevue.SelectedItem?.ToString();
        }
        /// <summary>
        /// remplir la combobox du public de la revue a ajouter ou modifier
        /// </summary>
        /// <returns></returns>
        private string RemplirCbxNewPublicRevue()
        {
            List<Categorie> LesPublicsRevue = controller.GetAllPublics();
            foreach (Categorie lePublic in LesPublicsRevue)
            {
                cbxNewPublicRevue.Items.Add(lePublic.Libelle);
            }
            if (cbxNewPublicRevue.Items.Count > 0)
            {
                cbxNewPublicRevue.SelectedIndex = 0;
            }
            return cbxNewPublicRevue.SelectedItem?.ToString();
        }
        /// <summary>
        /// remplir la combobox du rayon de la revue a ajouter ou modifier
        /// </summary>
        /// <returns></returns>
        private string RemplirCbxNewRayonRevue()
        {
            List<Categorie> LesRayonsRevue = controller.GetAllRayons();
            foreach (Categorie rayon in LesRayonsRevue)
            {
                cbxNewRayonRevue.Items.Add(rayon.Libelle);
            }
            if (cbxNewRayonRevue.Items.Count > 0)
            {
                cbxNewRayonRevue.SelectedIndex = 0;
            }
            return cbxNewRayonRevue.SelectedItem?.ToString();
        }
        /// <summary>
        /// ajouter une revue 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjoutRevue_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumero.Text.Equals(" ") && !txbRevuesTitre.Text.Equals(" ") && !txbRevuesPeriodicite.Text.Equals(" ") && !txbRevuesDateMiseADispo.Text.Equals("") && !cbxNewGenreRevue.Text.Equals(" ") && !cbxNewPublicRevue.Text.Equals(" ") && !cbxNewRayonRevue.Text.Equals(" "))
            {
                try
                {
                    string id = txbRevuesNumero.Text;
                    string titre = txbRevuesTitre.Text;
                    string image = txbRevuesImage.Text;
                    string periodicite = txbRevuesPeriodicite.Text;
                    int delaiMiseADispo = int.Parse(txbRevuesDateMiseADispo.Text);
                    string idGenre = GetIdGenreDocument(cbxNewGenreRevue.Text);
                    string idPublic = GetIdPublicDocument(cbxNewPublicRevue.Text);
                    string idRayon = GetIdRayonDocument(cbxNewRayonRevue.Text);
                    string genre = txbRevuesGenre.Text;
                    string lePublic = txbRevuesPublic.Text;
                    string rayon = txbRevuesRayon.Text;
                    Document document = new Document(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon);
                    Revue revue = new Revue(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon, periodicite, delaiMiseADispo);
                    var idRevueExist = controller.GetAllDocuments(id);
                    var idRevueNoExist = !idRevueExist.Any();

                    if (idRevueNoExist)
                    {
                        if (controller.CreerDocument(document.Id, document.Titre, document.Image, document.IdGenre, document.IdPublic, document.IdRayon) && controller.CreerRevue(revue.Id, revue.Periodicite, revue.DelaiMiseADispo))
                        {
                            lesRevues = controller.GetAllRevues();
                            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
                            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
                            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
                            RemplirRevuesListeComplete();
                            MessageBox.Show(" La revue " + titre + " est bien ajouté ");
                        }
                    }
                    else
                    {
                        MessageBox.Show(" Le numéro de document existe déjà! saisir un autre numéro! ", "ERREUR!!");
                    }

                }
                catch
                {
                    MessageBox.Show(" Une erreur s'est produite!!", "ERREUR!!");
                }
            }
            else
            {
                MessageBox.Show(" Veuillez saisir tous les champs!", "INFORMATION!!");
            }

        }
        /// <summary>
        /// modifier une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnModifRevue_Click(object sender, EventArgs e)
        {
            if (dgvRevuesListe.SelectedRows.Count > 0)
            {
                Revue selectRevue = (Revue)dgvRevuesListe.SelectedRows[0].DataBoundItem;
                string id = selectRevue.Id;
                string titre = txbRevuesTitre.Text;
                string image = txbRevuesImage.Text;
                string periodidcite = txbRevuesPeriodicite.Text;
                int delaiMiseADispo = int.Parse(txbRevuesDateMiseADispo.Text);
                string idGenre = GetIdGenreDocument(cbxNewGenreRevue.Text);
                string idPublic = GetIdPublicDocument(cbxNewPublicRevue.Text);
                string idRayon = GetIdRayonDocument(cbxNewRayonRevue.Text);
                if (!txbRevuesNumero.Text.Equals("") && !txbRevuesTitre.Text.Equals("") && !txbRevuesPeriodicite.Text.Equals("") && !txbRevuesDateMiseADispo.Text.Equals("") && !cbxNewGenreRevue.Text.Equals("") && !cbxNewPublicRevue.Text.Equals("") && !cbxNewRayonRevue.Text.Equals(""))
                {
                    if (controller.EditDocument(id, titre, image, idGenre, idPublic, idRayon) && controller.EditRevue(id, periodidcite, delaiMiseADispo))
                    {
                        lesRevues = controller.GetAllRevues();
                        RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
                        RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
                        RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
                        RemplirRevuesListeComplete();
                        MessageBox.Show("La revue " + titre + " est bien modifié!");
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification de la revue", "ERREUR!!");

                    }
                }
                else
                {
                    MessageBox.Show("Veuillez saisir tous les champs!!", "INFORMATION!!");
                }

            }
            else
            {
                MessageBox.Show("Veuillez selectioner une ligne!!", "INFORMATION!!");
            }
        }
        /// <summary>
        /// supprimer une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnSuppRevue_Click(object sender, EventArgs e)
        {
            Revue revue = (Revue)bdgRevuesListe.Current;
            if (MessageBox.Show("êtes vous sûr de vouloir supprimer cette" + revue.Titre + "?", "CONFIRMATION!", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                var exmpRevue = controller.GetExemplairesDocument(revue.Id);
                var noExmpl = !exmpRevue.Any();
                var commandeRevue = controller.GetCommandeDocument(revue.Id);
                var noCommande = !commandeRevue.Any();
                if (noCommande && noExmpl)
                {
                    if (controller.DeleteRevue(revue.Id))
                    {
                        lesRevues = controller.GetAllRevues();
                        RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
                        RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
                        RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
                        RemplirRevuesListeComplete();
                        MessageBox.Show("La revue " + revue.Titre + " est supprimer!");
                    }
                    else
                    {
                        MessageBox.Show("erreur!!", "ERREUR!");
                    }
                }
                else
                {
                    MessageBox.Show("Impossible de supprimer cette revue car il possède un ou plusieurs exemplaire(s), ou une ou plusieurs commande(s)", "ERREUR!!");
                }
            }
        }
        #endregion

        #region Onglet Paarutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }








        #endregion
        #region onglet CommandeLivre
        private readonly BindingSource bdgCmdLivresListe = new BindingSource();
        private List<CommandeDocument> lesCommandesDocument = new List<CommandeDocument>();
        private List<Suivi> lesSuivis = new List<Suivi>();



        /// <summary>
        /// Ouverture de l'onglet Commandes de livres : 
        /// appel des méthodes pour remplir le datagrid des commandes de livres et des combos suivi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCmdLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            lesSuivis = controller.GetAllSuivis();
            grpBoxCmdLivreInfos.Enabled = false;
            // RemplirCbxEtapeSuiviCommandeLivre();
        }



        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="lesCommandesDocument"></param>

        private void RemplirCmdLivresListe(List<CommandeDocument> lesCommandesDocument)
        {
            if (lesCommandesDocument != null)
            {

                bdgCmdLivresListe.DataSource = lesCommandesDocument;
                dgvCmdLivre.DataSource = bdgCmdLivresListe;
                dgvCmdLivre.Columns["id"].Visible = false;
                dgvCmdLivre.Columns["idLivreDvd"].Visible = false;
                dgvCmdLivre.Columns["idSuivi"].Visible = false;
                dgvCmdLivre.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvCmdLivre.Columns["dateCommande"].DisplayIndex = 2;
                dgvCmdLivre.Columns["montant"].DisplayIndex = 1;
                dgvCmdLivre.Columns[3].HeaderCell.Value = "Date de commande";
                dgvCmdLivre.Columns[0].HeaderCell.Value = "Nombre d'exemplaires";
                dgvCmdLivre.Columns[5].HeaderCell.Value = "Suivi";
            }
            else
            {
                bdgCmdLivresListe.DataSource = null;
            }

        }
        /// <summary>
        /// affichage des informations du livre selectioné
        /// </summary>
        /// <param name="livre"></param>
        private void AfficheCommandeLivresInfos(Livre livre)
        {
            txbCmdTitreLivre.Text = livre.Titre;
            txbCmdAuteurLivre.Text = livre.Auteur;
            txbCmdCollectionLivre.Text = livre.Collection;
            txbCmdImageLivre.Text = livre.Image;
            txbCmdCodeIsbnLivre.Text = livre.Isbn;
            txbCmdGenreLivre.Text = livre.Genre;
            txbCmdPublicLivre.Text = livre.Public;
            txbCmdRayonLivre.Text = livre.Rayon;
            txbCmdImageLivre.Text = livre.Image;
            string image = livre.Image;
            try
            {
                pctBoxCmdImageLivre.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
            AfficheCommandeLivre();
        }
        private void AfficheCommandeLivre()
        {
            string idDocument = txbNbCommandeLivreRecherche.Text;
            lesCommandesDocument = controller.GetCommandeDocument(idDocument);
            RemplirCmdLivresListe(lesCommandesDocument);
        }
        /// <summary>
        /// La recherche et l'affichage du document de type livre dont ona saisi son numéro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechercheCmdLivre_Click(object sender, EventArgs e)
        {
            if (!txbNbCommandeLivreRecherche.Text.Equals(""))
            {

                Livre livre = lesLivres.Find(x => x.Id.Equals(txbNbCommandeLivreRecherche.Text));
                if (livre != null)
                {
                    AfficheCommandeLivre();
                    grpBoxCmdLivreInfos.Enabled = true;
                    AfficheCommandeLivresInfos(livre);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");

                }
            }
            else
            {
                MessageBox.Show("Veuillez saisir le numéro du document!!", "INFORMATION!");
            }

        }
        /// <summary>
        /// Remplit la comboBox de l'etape suivi selon l'étape de suivi de la commande d'un livre
        /// </summary>
        /// <param name="etapeSuivi"></param>
        private void RemplirCbxEtapeSuiviCommandeLivre(string etapeSuivi)
        {
            cbxEtapSuiviCmdLivre.Items.Clear();

            switch (etapeSuivi)
            {
                case "en cours":
                    cbxEtapSuiviCmdLivre.Items.AddRange(new string[] { "livrée", "relancée" });
                    break;
                case "livrée":
                    cbxEtapSuiviCmdLivre.Items.Add("réglée");
                    break;
                case "relancée":
                    cbxEtapSuiviCmdLivre.Items.AddRange(new string[] { "en cours", "livrée" });
                    break;
            }

        }
        /// <summary>
        /// Récuperer l'id de suivi d'une commande
        /// </summary>
        /// <param name="stade"></param>
        /// <returns></returns>
        private string GetIdSuivi(string stade)
        {
            List<Suivi> lesSuivisCmd = controller.GetAllSuivis();
            foreach (Suivi s in lesSuivisCmd)
            {
                if (s.Stade == stade)
                {
                    return s.Id;
                }
            }
            return null;
        }
        /// <summary>
        /// affichage de l'etape de suivi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labelEtapeSuivi_TextChanged(object sender, EventArgs e)
        {
            string etapeSuivi = labelEtapeSuivi.Text;
            RemplirCbxEtapeSuiviCommandeLivre(etapeSuivi);

        }
        //private void labelEtapSuiviCmdLivre_TextChanged(object sender, EventArgs e)
        //{
        //    string etapeSuivi = labelEtapSuiviCmdLivre.Text;
        //    RemplirCbxEtapeSuiviCommandeLivre(etapeSuivi);
        //}
        private void cbxEtapSuiviCmdLivre_SelectedIndexChanged(object sender, EventArgs e)
        {
            string etapeSuivi = labelEtapeSuivi.Text;

            RemplirCbxEtapeSuiviCommandeLivre(etapeSuivi);

        }

        /// <summary>
        /// tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvCmdLivresListe_ColumnHeaderMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {

            string titreColonne = dgvCmdLivre.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesCommandesDocument.OrderBy(o => o.Id).ToList();
                    break;
                case "Date de la commande":
                    sortedList = lesCommandesDocument.OrderBy(o => o.DateCommande).Reverse().ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesDocument.OrderBy(o => o.Montant).ToList();
                    break;
                case "Nombre d'exemplaires":
                    sortedList = lesCommandesDocument.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "Suivi":
                    sortedList = lesCommandesDocument.OrderBy(o => o.Stade).ToList();
                    break;

            }
            RemplirCmdLivresListe(sortedList);
        }
        /// <summary>
        /// afficher les informations de la commande selectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCmdLivre_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dgvCmdLivre.Rows[e.RowIndex];
            string id = row.Cells["Id"].Value.ToString();
            DateTime dateCommande = (DateTime)row.Cells["DateCommande"].Value;
            double montant = double.Parse(row.Cells["Montant"].Value.ToString());
            int nbExemplaire = int.Parse(row.Cells["NbExemplaire"].Value.ToString());
            string stade = row.Cells["Stade"].Value.ToString();
            txbNewCmdNumLivre.Text = id;
            txbNewCmdNumExmpLivre.Text = nbExemplaire.ToString();
            txbNewMontantCmdLivre.Text = montant.ToString();
            dtimepikDateNewCmdLivre.Value = dateCommande;
            labelEtapeSuivi.Text = stade;
            if (GetIdSuivi(stade) != "002")
            {
                cbxEtapSuiviCmdLivre.Enabled = true;
                btnModifSuiviCmdLivre.Enabled = true;
            }
            else
            {
                cbxEtapSuiviCmdLivre.Enabled = false;
                btnModifSuiviCmdLivre.Enabled = false;
            }


        }
        /// <summary>
        /// Ajouter une commande de livre dans la bdd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjoutNewCmdLivre_Click(object sender, EventArgs e)
        {
            if (!txbNewCmdNumLivre.Text.Equals("") && !txbNewCmdNumExmpLivre.Text.Equals("") && !txbNewMontantCmdLivre.Text.Equals(""))
            {
                string id = txbNewCmdNumLivre.Text;
                int nbExemplaire = int.Parse(txbNewCmdNumExmpLivre.Text);
                double montant = double.Parse(txbNewMontantCmdLivre.Text);
                DateTime dateCommande = dtimepikDateNewCmdLivre.Value;
                string idLivreDvd = txbNbCommandeLivreRecherche.Text;
                string idSuivi = lesSuivis.FirstOrDefault().Id;
                string stade = lesSuivis.FirstOrDefault().Stade;
                Commande commande = new Commande(id, dateCommande, montant);
                if (controller.CreerCommande(commande))
                {
                    controller.CreerCommandeDocument(id, nbExemplaire, idLivreDvd, idSuivi);
                    MessageBox.Show("La commande " + id + "est enregistrée.", "INFORMATION!");
                    AfficheCommandeLivre();
                }
                else
                {
                    MessageBox.Show("Le numéro de la commande existe déjà!!", "ERREUR!");
                }
            }
            else
            {
                MessageBox.Show("Veuillez saisir tous les champs!", "INFORMATION!");
            }

        }
        /// <summary>
        /// modifier l'etape de suivi d'une commande d'un livre dans la bdd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifSuiviCmdLivre_Click(object sender, EventArgs e)
        {

            string id = txbNewCmdNumLivre.Text;
            int nbExemplaire = int.Parse(txbNewCmdNumExmpLivre.Text);
            double montant = double.Parse(txbNewMontantCmdLivre.Text);
            DateTime dateCommande = dtimepikDateNewCmdLivre.Value;
            string idLivreDvd = txbNbCommandeLivreRecherche.Text;
            string idSuivi = GetIdSuivi(cbxEtapSuiviCmdLivre.Text);
            string stade = cbxEtapSuiviCmdLivre.SelectedItem.ToString();
            CommandeDocument commandeDocument = new CommandeDocument(id, dateCommande, montant, idLivreDvd, nbExemplaire, idSuivi, stade);
            controller.EditSuiviCommandeDocument(commandeDocument.Id, commandeDocument.NbExemplaire, commandeDocument.IdLivreDvd, commandeDocument.IdSuivi);
            MessageBox.Show("L'etape de suivi de la commande " + id + "est modifiée.", "INFORMATION!");
            AfficheCommandeLivre();
            cbxEtapSuiviCmdLivre.Items.Clear();
            
        }
        /// <summary>
        /// supprimer la commande d'un livre si elle n'est pas livrée ou réglée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSuppCmdLivre_Click(object sender, EventArgs e)
        {
            if (dgvCmdLivre.SelectedRows.Count > 0)
            {
                CommandeDocument commandeDocument = (CommandeDocument)bdgCmdLivresListe.List[bdgCmdLivresListe.Position];
                if(commandeDocument.Stade == "en cours" || commandeDocument.Stade == "relancée")
                {
                    if(MessageBox.Show("ête-vous sûr de vouloir supprimer la commande" +commandeDocument.Id +"?", "CONFIRMATION!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        controller.DeleteCommandeDocument(commandeDocument);
                        AfficheCommandeLivre();
                    }
                }
                else
                {
                    MessageBox.Show("La commande sélectionée ne peut pas être supprimée car elle est livrée!", "INFORMATION!");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une ligne!", "INFORMATION!");
            }

        }
        private void btnRetourCmdLivre_Click(object sender, EventArgs e)
        {
            grpBoxInfosCommandeLivre.Enabled = false;
        }


        #endregion
        #region Onglet CommandeDvd
        private readonly BindingSource bdgCmdDvdListe = new BindingSource();
     
        /// <summary>
        /// Ouverture de l'onglet Commandes de Dvd : 
        /// appel des méthodes pour remplir le datagrid des commandes de dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCmdDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            lesSuivis = controller.GetAllSuivis();
        
        }
        /// <summary>
        ///  Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="lesCommandesDocument"></param>
        private void RemplirCmdDvdListe(List<CommandeDocument> lesCommandesDocument)
        {
            if (lesCommandesDocument != null)
            {

                bdgCmdDvdListe.DataSource = lesCommandesDocument;
                dgvCmdDvd.DataSource = bdgCmdDvdListe;
                dgvCmdDvd.Columns["id"].Visible = false;
                dgvCmdDvd.Columns["idLivreDvd"].Visible = false;
                dgvCmdDvd.Columns["idSuivi"].Visible = false;
                dgvCmdDvd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvCmdDvd.Columns["dateCommande"].DisplayIndex = 2;
                dgvCmdDvd.Columns["montant"].DisplayIndex = 1;
                dgvCmdDvd.Columns[3].HeaderCell.Value = "Date de commande";
                dgvCmdDvd.Columns[0].HeaderCell.Value = "Nombre d'exemplaires";
                dgvCmdDvd.Columns[5].HeaderCell.Value = "Suivi";
            }
            else
            {
                bdgCmdDvdListe.DataSource = null;
            }

        }
        /// <summary>
        /// affichage les informations de dvd
        /// </summary>
        /// <param name="dvd"></param>
        private void AfficheCommandeDvdInfos(Dvd dvd)
        {
            txbTitreCmdDvd.Text = dvd.Titre;
            txbRealisateurCmdDvd.Text = dvd.Realisateur;
            txbSynopsisCmdDvd.Text = dvd.Synopsis;
            txbDureeCmdDvd.Text = dvd.Duree.ToString();
            txbGenreCmdDvd.Text = dvd.Genre;
            txbPublicCmdDvd.Text = dvd.Public;
            txbRayonCmdDvd.Text = dvd.Rayon;
            txbCheminImageCmdDvd.Text = dvd.Image;
            string image = dvd.Image;
            try
            {
                pictBoxImageCmdDvd.Image = Image.FromFile(image);
            }
            catch
            {
                pictBoxImageCmdDvd.Image = null;
            }
            AfficheCommandeDvd();
        }
        private void AfficheCommandeDvd()
        {
            string idDocument = txbNbDocRechercheCmdDvd.Text;
            lesCommandesDocument = controller.GetCommandeDocument(idDocument);
            RemplirCmdDvdListe(lesCommandesDocument);
        }
        private void btnRechercheCmdDvd_Click(object sender, EventArgs e)
        {

            if (!txbNbDocRechercheCmdDvd.Text.Equals(""))
            {

                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbNbDocRechercheCmdDvd.Text));
                if (dvd != null)
                {
                    AfficheCommandeDvd();
                    grpBoxInfoNewCmdDvd.Enabled = true;
                    AfficheCommandeDvdInfos(dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");

                }
            }
            else
            {
                MessageBox.Show("Veuillez saisir le numéro du document!!", "INFORMATION!");
            }
        }
        /// <summary>
        /// remplir le combobox de l'etape de suivi d'une commande 
        /// </summary>
        /// <param name="etapeSuivi"></param>
        private void RemplirCbxEtapeSuiviCommandeDvd(string etapeSuivi)
        {
            cbxEtapSuiviCmdDvd.Items.Clear();

            switch (etapeSuivi)
            {
                case "en cours":
                    cbxEtapSuiviCmdDvd.Items.AddRange(new string[] { "livrée", "relancée" });
                    break;
                case "livrée":
                    cbxEtapSuiviCmdDvd.Items.Add("réglée");
                    break;
                case "relancée":
                    cbxEtapSuiviCmdDvd.Items.AddRange(new string[] { "en cours", "livrée" });
                    break;
            }

        }
        private void labelEtapeSuiviCmdDvd_TextChanged(object sender, EventArgs e)
        {
            string etapeSuivi = labelEtapeSuiviCmdDvd.Text;
            RemplirCbxEtapeSuiviCommandeDvd(etapeSuivi);

        }
        /// <summary>
        /// tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvCmdDvdListe_ColumnHeaderMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {

            string titreColonne = dgvCmdDvd.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesCommandesDocument.OrderBy(o => o.Id).ToList();
                    break;
                case "Date de la commande":
                    sortedList = lesCommandesDocument.OrderBy(o => o.DateCommande).Reverse().ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesDocument.OrderBy(o => o.Montant).ToList();
                    break;
                case "Nombre d'exemplaires":
                    sortedList = lesCommandesDocument.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "Suivi":
                    sortedList = lesCommandesDocument.OrderBy(o => o.Stade).ToList();
                    break;

            }
            RemplirCmdDvdListe(sortedList);
        }
        /// <summary>
        /// afficher les informations de la commande selectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCmdDvd_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dgvCmdDvd.Rows[e.RowIndex];
            string id = row.Cells["Id"].Value.ToString();
            DateTime dateCommande = (DateTime)row.Cells["DateCommande"].Value;
            double montant = double.Parse(row.Cells["Montant"].Value.ToString());
            int nbExemplaire = int.Parse(row.Cells["NbExemplaire"].Value.ToString());
            string stade = row.Cells["Stade"].Value.ToString();
            txbNewNumCmdDvd.Text = id;
            txbNewNbExmpCmdDvd.Text = nbExemplaire.ToString();
            txbNewMontantCmdDvd.Text = montant.ToString();
            dateTimePickerDateNewCmdDvd.Value = dateCommande;
            labelEtapeSuiviCmdDvd.Text = stade;
            if (GetIdSuivi(stade) != "002")
            {
                cbxEtapSuiviCmdDvd.Enabled = true;
                btnModifEtapSuiviDvd.Enabled = true;
            }
            else
            {
                cbxEtapSuiviCmdDvd.Enabled = false;
                btnModifEtapSuiviDvd.Enabled = false;
            }


        }
        /// <summary>
        /// ajouter une nouvelle commande d'un dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjoutNewCmdDvd_Click(object sender, EventArgs e)
        {
            if (!txbNewNumCmdDvd.Text.Equals("") && !txbNewNbExmpCmdDvd.Text.Equals("") && !txbNewMontantCmdDvd.Text.Equals(""))
            {
                string id = txbNewNumCmdDvd.Text;
                int nbExemplaire = int.Parse(txbNewNbExmpCmdDvd.Text);
                double montant = double.Parse(txbNewMontantCmdDvd.Text);
                DateTime dateCommande = dateTimePickerDateNewCmdDvd.Value;
                string idLivreDvd = txbNbDocRechercheCmdDvd.Text;
                string idSuivi = lesSuivis.FirstOrDefault().Id;
                string stade = lesSuivis.FirstOrDefault().Stade;
                Commande commande = new Commande(id, dateCommande, montant);
                if (controller.CreerCommande(commande))
                {
                    controller.CreerCommandeDocument(id, nbExemplaire, idLivreDvd, idSuivi);
                    MessageBox.Show("La commande " + id + "est enregistrée.", "INFORMATION!");
                    AfficheCommandeDvd();
                }
                else
                {
                    MessageBox.Show("Le numéro de la commande existe déjà!!", "ERREUR!");
                }
            }
            else
            {
                MessageBox.Show("Veuillez saisir tous les champs!", "INFORMATION!");
            }


        }
        /// <summary>
        /// modifier l'etape de suivi de la commande d'un dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifEtapSuiviDvd_Click(object sender, EventArgs e)
        {
            string id = txbNewNumCmdDvd.Text;
            int nbExemplaire = int.Parse(txbNewNbExmpCmdDvd.Text);
            double montant = double.Parse(txbNewMontantCmdDvd.Text);
            DateTime dateCommande = dateTimePickerDateNewCmdDvd.Value;
            string idLivreDvd = txbNbDocRechercheCmdDvd.Text;
            string idSuivi = GetIdSuivi(cbxEtapSuiviCmdDvd.Text);
            string stade = cbxEtapSuiviCmdDvd.SelectedItem.ToString();
            CommandeDocument commandeDocument = new CommandeDocument(id, dateCommande, montant, idLivreDvd, nbExemplaire, idSuivi, stade);
            controller.EditSuiviCommandeDocument(commandeDocument.Id, commandeDocument.NbExemplaire, commandeDocument.IdLivreDvd, commandeDocument.IdSuivi);
            MessageBox.Show("L'etape de suivi de la commande " + id + "est modifiée.", "INFORMATION!");
            AfficheCommandeDvd();
            cbxEtapSuiviCmdDvd.Items.Clear();

        }
        /// <summary>
        /// supprimer la commande d'un dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSuppCmdDvd_Click(object sender, EventArgs e)
        {
            if (dgvCmdDvd.SelectedRows.Count > 0)
            {
                CommandeDocument commandeDocument = (CommandeDocument)bdgCmdDvdListe.List[bdgCmdDvdListe.Position];
                if (commandeDocument.Stade == "en cours" || commandeDocument.Stade == "relancée")
                {
                    if (MessageBox.Show("ête-vous sûr de vouloir supprimer la commande" + commandeDocument.Id + "?", "CONFIRMATION!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        controller.DeleteCommandeDocument(commandeDocument);
                        AfficheCommandeDvd();
                    }
                }
                else
                {
                    MessageBox.Show("La commande sélectionée ne peut pas être supprimée car elle est livrée!", "INFORMATION!");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une ligne!", "INFORMATION!");
            }

        }
        private void btnRetourCmdDvd_Click(object sender, EventArgs e)
        {
            grpBoxInfoNewCmdDvd.Enabled = false;
            

        }
        //private void cbxEtapSuiviCmdDvd_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string selectedItemText = cbxEtapSuiviCmdDvd.SelectedItem.ToString();
        //    labelEtapeSuiviCmdDvd.Text = selectedItemText;
        //}



        #endregion


        private void txbRevuesImage_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvLivresListe_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabLivres_Click(object sender, EventArgs e)
        {

        }

        private void txbLivresNumRecherche_TextChanged(object sender, EventArgs e)
        {

        }



        private void dgvCmdLivre_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grpBoxInfosCommandeLivre_Enter(object sender, EventArgs e)
        {

        }

        private void tabCommandeDvd_Click(object sender, EventArgs e)
        {

        }

        private void labelEtapSuiviCmdLivre_Click(object sender, EventArgs e)
        {

        }

        
    }
}
