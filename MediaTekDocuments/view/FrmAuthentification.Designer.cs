
namespace MediaTekDocuments.view
{
    partial class FrmAuthentification
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
            this.btnConnexionAuthentificationUser = new System.Windows.Forms.Button();
            this.txbPasswordAuthentUser = new System.Windows.Forms.TextBox();
            this.txbLoginAuthentUser = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnexionAuthentificationUser
            // 
            this.btnConnexionAuthentificationUser.Location = new System.Drawing.Point(43, 258);
            this.btnConnexionAuthentificationUser.Name = "btnConnexionAuthentificationUser";
            this.btnConnexionAuthentificationUser.Size = new System.Drawing.Size(494, 39);
            this.btnConnexionAuthentificationUser.TabIndex = 0;
            this.btnConnexionAuthentificationUser.Text = "Connexion";
            this.btnConnexionAuthentificationUser.UseVisualStyleBackColor = true;
            this.btnConnexionAuthentificationUser.Click += new System.EventHandler(this.btnConnexionAuthentificationUser_Click);
            // 
            // txbPasswordAuthentUser
            // 
            this.txbPasswordAuthentUser.Location = new System.Drawing.Point(43, 180);
            this.txbPasswordAuthentUser.Name = "txbPasswordAuthentUser";
            this.txbPasswordAuthentUser.Size = new System.Drawing.Size(494, 26);
            this.txbPasswordAuthentUser.TabIndex = 1;
            // 
            // txbLoginAuthentUser
            // 
            this.txbLoginAuthentUser.Location = new System.Drawing.Point(43, 82);
            this.txbLoginAuthentUser.Name = "txbLoginAuthentUser";
            this.txbLoginAuthentUser.Size = new System.Drawing.Size(494, 26);
            this.txbLoginAuthentUser.TabIndex = 2;
            this.txbLoginAuthentUser.TextChanged += new System.EventHandler(this.txbLoginAuthentUser_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Login";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Password";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txbLoginAuthentUser);
            this.panel1.Controls.Add(this.txbPasswordAuthentUser);
            this.panel1.Controls.Add(this.btnConnexionAuthentificationUser);
            this.panel1.Location = new System.Drawing.Point(60, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(591, 319);
            this.panel1.TabIndex = 0;
            // 
            // FrmAuthentification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 399);
            this.Controls.Add(this.panel1);
            this.Name = "FrmAuthentification";
            this.Text = "Authentification Utilisateur";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConnexionAuthentificationUser;
        private System.Windows.Forms.TextBox txbPasswordAuthentUser;
        private System.Windows.Forms.TextBox txbLoginAuthentUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
    }
}