using FC_Diseño_de_Nervios.Clases_y_Enums.Herramientas;
using FC_Diseño_de_Nervios.Ventana_Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebTools;

namespace FC_Diseño_de_Nervios
{
    static class Program
    {
        public const string _GOOGLEDRIVE = "Google Drive File Stream";
        public const float Version = 1.19f;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new F_Inicio());
        }



    }

}
