using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FC_Diseño_de_Nervios
{
    [Serializable]
    public class cUndoRedo<T>
    {
        public Stack<T> Lista_CtrlZ { get; set; } = new Stack<T>();
        public Stack<T> Lista_CtrlY { get; set; } = new Stack<T>();

        public bool Bool_CtrlZ { get; set; }
        public bool Bool_CtrlY { get; set; }


        public void EnviarEstado(T Estado)
        {
            Lista_CtrlZ.Push(DeepClone(Estado));
            SaberCuandoSeHabilitaCrtlZ();
            SaberCuandoSeHabilitaCrtlY();
        }

        public T Deshacer()
        {
            if (Bool_CtrlZ)
            {
                T ObjetoQueQuite = Lista_CtrlZ.Pop();
                Lista_CtrlY.Push(ObjetoQueQuite);
                SaberCuandoSeHabilitaCrtlY();
                SaberCuandoSeHabilitaCrtlZ();
                return ObjetoQueQuite;
            }
            return default;
        }
        public T Rehacer()
        {
            if (Bool_CtrlY)
            {
                T ObjetoQueQuite = Lista_CtrlY.Pop();
                Lista_CtrlZ.Push(ObjetoQueQuite);
                SaberCuandoSeHabilitaCrtlY();
                SaberCuandoSeHabilitaCrtlZ();
                return ObjetoQueQuite;
            }
            return default;
        }


        private void SaberCuandoSeHabilitaCrtlZ()
        {
            if (Lista_CtrlZ.Count > 0)
            {
                Bool_CtrlZ = true;
            }
            else
            {
                Bool_CtrlZ = false;
            }
        }

        private void SaberCuandoSeHabilitaCrtlY()
        {
            if (Lista_CtrlY.Count > 0)
            {
                Bool_CtrlY = true;
            }
            else
            {
                Bool_CtrlY = false;
            }
        }



        private T DeepClone(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

    }
}
