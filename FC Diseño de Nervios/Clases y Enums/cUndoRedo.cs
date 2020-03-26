using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FC_Diseño_de_Nervios
{
    public class cUndoRedo<T>
    {
        private Stack<T> Lista_CtrlZ { get; set; } = new Stack<T>();
        private Stack<T> Lista_CtrlY { get; set; } = new Stack<T>();

        private bool Bool_CtrlZ { get; set; }
        private bool Bool_CtrlY { get; set; }

        public bool ObtenerEstadoCtrlZ()
        {
            return Bool_CtrlZ;
        }

        public bool ObtenerEstadoCtrlY()
        {
            return Bool_CtrlY;
        }

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
                T ObjetoQueQuite = DeepClone(Lista_CtrlZ.Pop());
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
                T ObjetoQueQuite = DeepClone(Lista_CtrlY.Pop());
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