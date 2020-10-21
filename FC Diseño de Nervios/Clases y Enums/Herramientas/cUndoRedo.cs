using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;


namespace FC_Diseño_de_Nervios
{
    public class cUndoRedo<T>
    {
        private Stack<T> Lista_CtrlZ { get; set; } = new Stack<T>();
        private Stack<T> Lista_CtrlY { get; set; } = new Stack<T>();
        private Stack<int> Lista_Estados { get; set; } = new Stack<int>();

        private bool Bool_CtrlZ { get; set; }
        private bool Bool_CtrlY { get; set; }
        private bool Bool_EstadosActivos { get; set; }
        public void LimpiarEstadosCtrlZyCtrlY()
        {
            Lista_CtrlY.Clear();
            Lista_CtrlZ.Clear();
            SaberCuandoSeHabilitaCrtlZ();
            SaberCuandoSeHabilitaCrtlY();
        }

        public void LimpiarEstados()
        {
            Lista_Estados.Clear();
            SaberCuandoSeHabilitaEstados();
        }
        public bool ObtenerEstadoCtrlZ()
        {
            return Bool_CtrlZ;
        }

        public bool ObtenerEstadoCtrlY()
        {
            return Bool_CtrlY;
        }
        public bool ObtenerEstadoEstados()
        {
           return Bool_EstadosActivos;
        }

        public void EnviarEstadoVacio()
        {
            Lista_Estados.Push(0);
            SaberCuandoSeHabilitaEstados();
        }
        public void EnviarEstado(T Estado)
        {
            Lista_CtrlZ.Push(CloneObject(Estado));
            Lista_Estados.Push(0);
            SaberCuandoSeHabilitaCrtlZ();
            SaberCuandoSeHabilitaCrtlY();
            SaberCuandoSeHabilitaEstados();
        }

        public T Deshacer(T EstadoActual)
        {
            if (Bool_CtrlZ)
            {
                T ObjetoQueQuite = Lista_CtrlZ.Pop();
                Lista_CtrlY.Push(CloneObject(EstadoActual));
                Lista_Estados.Push(0);
                SaberCuandoSeHabilitaCrtlY();
                SaberCuandoSeHabilitaCrtlZ();
                SaberCuandoSeHabilitaEstados();
                return ObjetoQueQuite;
            }
            return default;
        }

        public T Rehacer(T EstadoActual)
        {
            if (Bool_CtrlY)
            {
                T ObjetoQueQuite = Lista_CtrlY.Pop();
                Lista_CtrlZ.Push(CloneObject(EstadoActual));
                Lista_Estados.Push(0);
                SaberCuandoSeHabilitaCrtlY();
                SaberCuandoSeHabilitaCrtlZ();
                SaberCuandoSeHabilitaEstados();
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

        private void SaberCuandoSeHabilitaEstados()
        {
            if (Lista_Estados.Count > 0)
            {
                Bool_EstadosActivos = true;
            }
            else
            {
                Bool_EstadosActivos = false;
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
        
        private T CloneObject(T Obj)
        {
           return Force.DeepCloner.DeepClonerExtensions.DeepClone(Obj);
        }

    }
}