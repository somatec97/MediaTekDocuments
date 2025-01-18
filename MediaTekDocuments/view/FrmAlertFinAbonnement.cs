using System;
using System.Collections.Generic;
using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    public partial class FrmAlertFinAbonnement : Form
    {
        private readonly BindingSource bdgAbonnementsExpires = new BindingSource();
        private readonly List<Abonnement> lesAbonnementsExpires = new List<Abonnement>();
        public FrmAlertFinAbonnement(FrmMediatekController controller)
        {
            InitializeComponent();
            lesAbonnementsExpires = controller.GetAbonnementsExpires();
            if (lesAbonnementsExpires == null || !lesAbonnementsExpires.Any())
            {
                Console.WriteLine("Aucun abonnement expiré trouvé");
            }
            else
            {
                Console.WriteLine($"{lesAbonnementsExpires.Count} abonnements expirés trouvés");
            }
            RemplirAbonnementsExpires(lesAbonnementsExpires);
        }

        //public FrmAlertFinAbonnement(FrmMediatekController controller)
        //{
        //    InitializeComponent();
        //    lesAbonnementsExpires = controller.GetAbonnementsExpires();
        //    RemplirAbonnementsExpires(lesAbonnementsExpires);
        //}
        /// <summary>
        /// Remplissage de la grille des abonnements qui expirent dans 30 jours
        /// </summary>
        /// <param name="lesAbonnementsExpires"></param>
        private void RemplirAbonnementsExpires(List<Abonnement> lesAbonnementsExpires)
        {
            bdgAbonnementsExpires.DataSource = lesAbonnementsExpires;
            dtgFrmAlerteFinAbonnemnet.DataSource = bdgAbonnementsExpires;
            dtgFrmAlerteFinAbonnemnet.Columns["dateCommande"].Visible = false;
            dtgFrmAlerteFinAbonnemnet.Columns["id"].Visible = false;
            dtgFrmAlerteFinAbonnemnet.Columns["montant"].Visible = false;
            dtgFrmAlerteFinAbonnemnet.Columns["idRevue"].Visible = false;
            dtgFrmAlerteFinAbonnemnet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dtgFrmAlerteFinAbonnemnet.Columns[0].HeaderCell.Value = "Titre";
            dtgFrmAlerteFinAbonnemnet.Columns[1].HeaderCell.Value = "Date fin abonnement";

        }
        /// <summary>
        /// tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAbonnementsExpires_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            string titreColonne = dtgFrmAlerteFinAbonnemnet.Columns[e.ColumnIndex].HeaderText;
            List<Abonnement> sortedList = new List<Abonnement>();
            switch (titreColonne)
            {
                case "Titre":
                    sortedList = lesAbonnementsExpires.OrderBy(o => o.Titre).ToList();
                    break;
                case "Date fin abonnement":
                    sortedList = lesAbonnementsExpires.OrderBy(o => o.DateFinAbonnement).Reverse().ToList();
                    break;
               

            }
            RemplirAbonnementsExpires(sortedList);
        }

        private void FrmAlertFinAbonnement_Load(object sender, EventArgs e)
        {

        }

        private void dtgFrmAlerteFinAbonnemnet_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
