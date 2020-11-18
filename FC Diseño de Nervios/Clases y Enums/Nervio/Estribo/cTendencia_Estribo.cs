using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios.Clases_y_Enums.Nervio.Estribo
{

    [Serializable]
    public class cTendencia_Estribo
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public cTendencia_Refuerzo Tendencia_Refuerzo_Origen { get; set; }
        public float Peso { get; private set; }
        public List<cBloqueEstribos> BloqueEstribos { get; set; } = new List<cBloqueEstribos>();

        public void CrearGrupoEstribos(cBloqueEstribos grupoEstribos)
        {
            BloqueEstribos.Add(grupoEstribos);
        }

        public cTendencia_Estribo(int ID,cTendencia_Refuerzo Tendencia_Refuerzo_Origen)
        {
            this.ID = ID;
            Nombre = $"Tendencia {ID}";
            this.Tendencia_Refuerzo_Origen = Tendencia_Refuerzo_Origen;
        }

    }
}
