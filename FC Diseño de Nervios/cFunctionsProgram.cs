using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    public delegate void DelegateNotificadorProgram(string Alert);
    public static class cFunctionsProgram
    {
        public static event DelegateNotificadorProgram Notificador;

        public static cProyecto NuevoProyecto()
        {


            return new cProyecto();


            

        }






















    }
}
