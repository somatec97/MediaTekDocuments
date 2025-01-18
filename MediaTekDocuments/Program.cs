using MediaTekDocuments.view;
using System;
using System.Windows.Forms;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MediaTekDocuments
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMediatek());
            //Application.Run(new FrmAuthentification());
        }
    }
}
