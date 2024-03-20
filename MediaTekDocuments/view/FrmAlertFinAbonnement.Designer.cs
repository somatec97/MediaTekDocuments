
namespace MediaTekDocuments.view
{
    partial class FrmAlertFinAbonnement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.dtgFrmAlerteFinAbonnemnet = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtgFrmAlerteFinAbonnemnet)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(525, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = " La liste des revues dont l\'abonnement se termine dans moins de 30 jours";
            // 
            // dtgFrmAlerteFinAbonnemnet
            // 
            this.dtgFrmAlerteFinAbonnemnet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgFrmAlerteFinAbonnemnet.Location = new System.Drawing.Point(32, 84);
            this.dtgFrmAlerteFinAbonnemnet.Name = "dtgFrmAlerteFinAbonnemnet";
            this.dtgFrmAlerteFinAbonnemnet.RowHeadersWidth = 62;
            this.dtgFrmAlerteFinAbonnemnet.RowTemplate.Height = 28;
            this.dtgFrmAlerteFinAbonnemnet.Size = new System.Drawing.Size(521, 201);
            this.dtgFrmAlerteFinAbonnemnet.TabIndex = 1;
            this.dtgFrmAlerteFinAbonnemnet.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgFrmAlerteFinAbonnemnet_CellContentClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(32, 316);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(521, 45);
            this.button1.TabIndex = 2;
            this.button1.Text = "ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmAlertFinAbonnement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 388);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dtgFrmAlerteFinAbonnemnet);
            this.Controls.Add(this.label1);
            this.Name = "FrmAlertFinAbonnement";
            this.Text = "Alerte fin d\'abonnements :";
            this.Load += new System.EventHandler(this.FrmAlertFinAbonnement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgFrmAlerteFinAbonnemnet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dtgFrmAlerteFinAbonnemnet;
        private System.Windows.Forms.Button button1;
    }
}