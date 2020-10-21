using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.Interop.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FC_BFunctionsAutoCAD
{
    /// <summary>
    /// Tipo de punta de la cota.
    /// </summary>
    public enum ArrowHeadType
    {
        /// <summary>
        /// Punta en circulo.
        /// </summary>
        ArrowDot,

        /// <summary>
        /// Punta en flecha.
        /// </summary>
        ArrowDefault
    }

    /// <summary>
    ///
    /// </summary>
    public enum PrecisionCota
    {
        /// <summary>
        ///
        /// </summary>
        Cero,

        /// <summary>
        ///
        /// </summary>
        Uno,

        /// <summary>
        ///
        /// </summary>
        Dos,

        /// <summary>
        ///
        /// </summary>
        Tres,

        /// <summary>
        ///
        /// </summary>
        Cuatro,

        /// <summary>
        ///
        /// </summary>
        Cinco,

        /// <summary>
        ///
        /// </summary>
        Seis
    }

    /// <summary>
    /// Justificar Texto.
    /// </summary>
    public enum JustifyText
    {
        /// <summary>
        /// Centro
        /// </summary>
        Center,

        /// <summary>
        /// Izquierda.
        /// </summary>
        Left,

        /// <summary>
        /// Derecha.
        /// </summary>
        Right
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Alert"></param>
    public delegate void DelegateNotificadorErrores(string Alert);



    /// <summary>
    /// Funciones basicas para la manipulación de AutoCAD - efe Prima Ce, Por: José Luis Rosales Meza
    /// </summary>
    public class FunctionsAutoCAD
    {
        private static AcadApplication AcadApp;
        /// <summary>
        /// Dcoumento actual.
        /// </summary>
        public static AcadDocument AcadDoc;

        /// <summary>
        /// Notifica errores en la librería.
        /// </summary>
        public static event DelegateNotificadorErrores NotificadorErrores;

        /// <summary>
        /// 
        /// </summary>
        public static void OpenAutoCAD()
        {
            try
            {
                AcadApp = (AcadApplication)Marshal.GetActiveObject("Autocad.Application");
                AcadDoc = AcadApp.ActiveDocument;
            }
            catch
            {
                try
                {
                    AcadApp = new AcadApplication();
                    AcadApp.Visible = true;
                    OpenFileDialog openFileDialog = new OpenFileDialog()
                    { Multiselect = true, Filter = "Abrir Plantilla|*.dwg", Title = "Archivo dwg" };
                    openFileDialog.ShowDialog();

                    if (openFileDialog != null)
                    {
                        AcadDoc = AcadApp.Documents.Add(openFileDialog.FileName);
                    }
                }
                catch {

                    NotificadorErrores?.Invoke("Error inesperado.");
                }

            }
        }

        /// <summary>
        /// Establece escala especificada al documento actual.
        /// </summary>
        /// <param name="Scale">Escala especificada, ejemplo: "1:25"</param>
        public static void SetScale(string Scale)
        {
            try
            {
                if (AcadDoc != null)
                {
                    AcadDoc.SetVariable("CANNOSCALE", Scale);
                }
            }
            catch
            {
                NotificadorErrores?.Invoke("Error inesperado.");
            }
        }

        /// <summary>
        /// Elimina una seleccion del documento actual.
        /// </summary>
        /// <param name="Selection">Selección específica a eliminar.</param>
        private static void DeleteESelec(string Selection)
        {
            foreach (AcadSelectionSet selectionSet in AcadDoc.SelectionSets)
            {
                if (selectionSet.Name == Selection)
                {
                    selectionSet.Delete();
                    break;
                }
            }
        }

        /// <summary>
        /// Agregar nueva selección al documento actual.
        /// </summary>
        /// <param name="SelectionS">Selección a agregar.</param>
        public static object AddSelection(string SelectionS)
        {
            AcadSelectionSet Selection = null;

            if (AcadDoc != null)
            {
                DeleteESelec(SelectionS);
                AcadDoc.PurgeAll();

                Selection = AcadDoc.SelectionSets.Add(SelectionS);
                Selection.SelectOnScreen();
            }
            return Selection;
        }

        /// <summary>
        /// Obtiene un punto del documento actual.
        /// </summary>
        /// <param name="Msg">Mensaje a mostrar en el documento actual.</param>
        /// <param name="Array">Coordenadas del punto [X,Y,Z].</param>
        public static void GetPoint(ref double[] Array, string Msg = "Posicionar")
        {
            if (AcadDoc != null)
            {
                try
                {
                    Array = AcadDoc.Utility.GetPoint(Prompt: Msg);
                }
                catch  {

                    NotificadorErrores?.Invoke("Error inesperado.");
                }
            }
        }

        /// <summary>
        /// Agrega un nuevo texto al documento actual.
        /// </summary>
        /// <param name="TextString">Texto a mostrar</param>
        /// <param name="P_XYZ">Coordenadas del texto (X,Y,Z).</param>
        /// <param name="Width">Ancho del texto.</param>
        ///  <param name="Height">Alto del texto.</param>
        /// <param name="Layer">Capa del texto.</param>
        /// <param name="Style">Estilo del texto.</param>
        /// <param name="Rotation">Ángulo de rotación del texto en grados.</param>
        /// <param name="LineTypeSacale">Tipo de escala de la linea.</param>
        /// <param name="Width2">Ancho del cuadro de Texto.</param>
        /// <param name="JustifyText">Justificación del Texto.</param>

        public static void AddText(string TextString, double[] P_XYZ, double Width, double Height, string Layer, string Style, float Rotation, double LineTypeSacale = 1, double Width2 = 1.3, JustifyText JustifyText = JustifyText.Left)
        {
            try
            {
                if (AcadDoc != null)
                {

                    AcadMText text = AcadDoc.ModelSpace.AddMText(P_XYZ, Width, TextString);
                    text.Layer = Layer;
                    text.StyleName = Style;
                    text.Height = Height;
                    text.Rotation = Rotation * Math.PI / 180;
                    text.LinetypeScale = LineTypeSacale;
                    text.Width = Width2;
                    text.AttachmentPoint = Clasf_JustyText(JustifyText);
                    text.Update();
                }
            }
            catch
            {
                NotificadorErrores?.Invoke("Error inesperado.");
            }
        }


        /// <summary>
        /// Agrega un nuevo texto al documento actual.
        /// </summary>
        /// <param name="TextString">Texto a mostrar</param>
        /// <param name="P_XY">Coordenadas del texto (X,Y) [System.Drawing].</param>
        /// <param name="Width">Ancho del texto.</param>
        ///  <param name="Height">Alto del texto.</param>
        /// <param name="Layer">Capa del texto.</param>
        /// <param name="Style">Estilo del texto.</param>
        /// <param name="Rotation">Ángulo de rotación del texto en grados.</param>
        /// <param name="LineTypeSacale">Tipo de escala de la linea.</param>
        /// <param name="Width2">Ancho del cuadro de Texto.</param>
        /// <param name="JustifyText">Justificación del Texto.</param>

        public static void AddText(string TextString, PointF P_XY, double Width, double Height, string Layer, string Style, float Rotation, double LineTypeSacale = 1, double Width2 = 1.3, JustifyText JustifyText = JustifyText.Left)
        {
            if (AcadDoc != null)
            {
                try
                {

                    AcadMText text = AcadDoc.ModelSpace.AddMText(ConvertirPuntoEnDobules3D(P_XY), Width, TextString);
                    text.Layer = Layer;
                    text.StyleName = Style;
                    text.Height = Height;
                    text.Rotation = Rotation * Math.PI / 180;
                    text.LinetypeScale = LineTypeSacale;
                    text.Width = Width2;
                    text.AttachmentPoint = Clasf_JustyText(JustifyText);
                    text.Update();

                }
                catch 
                {
                    NotificadorErrores?.Invoke("Error inesperado.");
                }
            }
        }



        /// <summary>
        /// Añadir nueva cota al documento actual.
        /// </summary>
        /// <param name="P1_XYZ">Punto Inicial (X,Y,Z)</param>
        /// <param name="P2_XYZ">Punto Final (X,Y,Z)</param>
        /// <param name="Layer">Capa de la cota.</param>
        /// <param name="Style">Estilo de la cota.</param>
        /// <param name="DesplazCota">Desplazamiento de la Cota perpendicularmente a la línea formada.</param>
        /// <param name="DeplazaTextX">Desplazamiento en X del texto asociado a la cota</param>
        /// <param name="DeplazaTextY">Desplazamiento en Y del texto asociado a la cota</param>
        /// <param name="Precision">Cantidad de decimales de la cota. Ejemplo: (0,0.0,0.00,0.000)</param>
        /// <param name="Text">Cambia la medición original al texto establecido.</param>
        /// <param name="TextHeight">Alto del texto.</param>
        /// <param name="ArrowheadSize">Tamaño de las puntas de la cota.</param>
        /// <param name="headType1">Tipo de cabeza de la cota en la punta inicial.</param>
        /// <param name="headType2">Tipo de cabeza de la cota en la punta final.</param>
        /// <param name="TextRotation">Angulo de rotación en grados del texto.</param>
        public static void AddCota(double[] P1_XYZ, double[] P2_XYZ, string Layer, string Style, float DesplazCota = 0, float DeplazaTextX = 0, float DeplazaTextY = 0, PrecisionCota Precision = PrecisionCota.Dos, string Text = "", double TextHeight = 0.0015,
            double ArrowheadSize = 0.001, ArrowHeadType headType1 = ArrowHeadType.ArrowDot, ArrowHeadType headType2 = ArrowHeadType.ArrowDot, double TextRotation = 0)
        {
            try
            {
                if (AcadDoc != null && P1_XYZ.Length == 3 && P2_XYZ.Length == 3)
                {
                    double Rotation_Rad;
                    double X = P2_XYZ[0] - P1_XYZ[0];
                    double Y = P2_XYZ[1] - P1_XYZ[1];
                    //Encontrar Angulo
                    Rotation_Rad = Math.Abs(Math.Atan(Y / X));
                    double DesX = (Distancia(P1_XYZ, P2_XYZ) / 2) * Math.Cos(Rotation_Rad);
                    double DesY = (Distancia(P1_XYZ, P2_XYZ) / 2) * Math.Sin(Rotation_Rad);
                    double DesplazarXCota = DesplazCota * Math.Sin(Rotation_Rad);
                    double DesplazarYCota = DesplazCota * Math.Cos(Rotation_Rad);

                    double[] LocationText = new double[] { P1_XYZ[0] + DesX + DesplazarXCota, P1_XYZ[1] + DesY + DesplazarYCota, P1_XYZ[2] + (P2_XYZ[2] - P1_XYZ[2]) / 2 };
                    double[] TextPosition = new double[] { LocationText[0] + DeplazaTextX, LocationText[1] + DeplazaTextY, LocationText[2] };

                    AcadDimRotated cota = AcadDoc.ModelSpace.AddDimRotated(P1_XYZ, P2_XYZ, LocationText, Rotation_Rad);

                    //Cotas siempre con Puntas Redondas

                    cota.Layer = Layer;
                    cota.StyleName = Layer;
                    cota.TextStyle = Style;
                    cota.TextHeight = TextHeight;
                    cota.ArrowheadSize = ArrowheadSize;
                    cota.PrimaryUnitsPrecision = Clasf_precision(Precision);
                    cota.TextPosition = TextPosition;
                    cota.Arrowhead1Type = Clasf_ArrowHeadType(headType1);
                    cota.Arrowhead2Type = Clasf_ArrowHeadType(headType2);
                    cota.TextRotation = TextRotation * Math.PI / 180;
                    if (Text != "")
                    {
                        cota.TextOverride = Text;
                    }

                    cota.Update();
                }
            }
            catch
            {
                NotificadorErrores?.Invoke("Error inesperado.");
            }
        }

        /// <summary>
        /// Añadir nueva cota al documento actual.
        /// </summary>
        /// <param name="P1_XY">Punto Inicial [System.Drawing]</param>
        /// <param name="P2_XY">Punto Final [System.Drawing]</param>
        /// <param name="Layer">Capa de la cota.</param>
        /// <param name="Style">Estilo de la cota.</param>
        /// <param name="DesplazCota">Desplazamiento de la Cota perpendicularmente a la línea formada.</param>
        /// <param name="DeplazaTextX">Desplazamiento en X del texto asociado a la cota</param>
        /// <param name="DeplazaTextY">Desplazamiento en Y del texto asociado a la cota</param>
        /// <param name="Precision">Cantidad de decimales de la cota. Ejemplo: (0,0.0,0.00,0.000)</param>
        /// <param name="Text">Cambia la medición original al texto establecido.</param>
        /// <param name="TextHeight">Alto del texto.</param>
        /// <param name="ArrowheadSize">Tamaño de las puntas de la cota.</param>
        /// <param name="headType1">Tipo de cabeza de la cota en la punta inicial.</param>
        /// <param name="headType2">Tipo de cabeza de la cota en la punta final.</param>
        /// <param name="TextRotation">Angulo de rotación en grados del texto.</param>
        /// <param name="RA">Aplicar rotación automatica.</param>
        public static void AddCota(PointF P1_XY, PointF P2_XY, string Layer, string Style, float DesplazCota = 0, float DeplazaTextX = 0, float DeplazaTextY = 0, PrecisionCota Precision = PrecisionCota.Dos, string Text = "", double TextHeight = 0.0015,
            double ArrowheadSize = 0.001, ArrowHeadType headType1 = ArrowHeadType.ArrowDot, ArrowHeadType headType2 = ArrowHeadType.ArrowDot, double TextRotation = 0, bool RA = true)
        {
            try
            {
                double[] P1_XYZ = ConvertirPuntoEnDobules3D(P1_XY);
                double[] P2_XYZ = ConvertirPuntoEnDobules3D(P2_XY);
                if (AcadDoc != null && P1_XYZ.Length == 3 && P2_XYZ.Length == 3)
                {
                    double Rotation_Rad;
                    double X = P2_XYZ[0] - P1_XYZ[0];
                    double Y = P2_XYZ[1] - P1_XYZ[1];
                    //Encontrar Angulo
                    Rotation_Rad = Math.Abs(Math.Atan(Y / X));
                    double DesX = (Distancia(P1_XYZ, P2_XYZ) / 2) * Math.Cos(Rotation_Rad);
                    double DesY = (Distancia(P1_XYZ, P2_XYZ) / 2) * Math.Sin(Rotation_Rad);
                    double DesplazarXCota = DesplazCota * Math.Sin(Rotation_Rad);
                    double DesplazarYCota = DesplazCota * Math.Cos(Rotation_Rad);

                    double[] LocationText = new double[] { P1_XYZ[0] + DesX + DesplazarXCota, P1_XYZ[1] + DesY + DesplazarYCota, P1_XYZ[2] + (P2_XYZ[2] - P1_XYZ[2]) / 2 };
                    double[] TextPosition = new double[] { LocationText[0] + DeplazaTextX, LocationText[1] + DeplazaTextY, LocationText[2] };
                    if (!RA)
                        Rotation_Rad = 0;
                    AcadDimRotated cota = AcadDoc.ModelSpace.AddDimRotated(P1_XYZ, P2_XYZ, LocationText, Rotation_Rad);

                    //Cotas siempre con Puntas Redondas

                    cota.Layer = Layer;
                    cota.StyleName = Layer;
                    cota.TextStyle = Style;
                    cota.TextHeight = TextHeight;
                    cota.ArrowheadSize = ArrowheadSize;
                    cota.PrimaryUnitsPrecision = Clasf_precision(Precision);
                    cota.TextPosition = TextPosition;
                    cota.Arrowhead1Type = Clasf_ArrowHeadType(headType1);
                    cota.Arrowhead2Type = Clasf_ArrowHeadType(headType2);
                    cota.TextRotation = TextRotation * Math.PI / 180;
                    if (Text != "")
                    {
                        cota.TextOverride = Text;
                    }

                    cota.Update();
                }
            }
            catch
            {
                NotificadorErrores?.Invoke("Error inesperado.");
            }
        }










        /// <summary>
        /// Crear un nuevo Polígono en 2D.
        /// </summary>
        /// <param name="VerticesList">Vertices del polígono. Ejemplo: (X1,Y1, X2,Y2, X3,Y3 X4,Y4)</param>
        /// <param name="Layer">Capa del Polígono</param>
        /// <param name="Closed">Establece si el polígono es cerrado</param>
        public static void AddPolyline2D(double[] VerticesList, string Layer, bool Closed = true)
        {
            try
            {
                if (AcadDoc != null)
                {
                    AcadLWPolyline polyline = AcadDoc.ModelSpace.AddLightWeightPolyline(VerticesList);
                    polyline.Layer = Layer;
                    polyline.Closed = Closed;
                    polyline.Update();
                }
            }
            catch
            {
                NotificadorErrores?.Invoke("Error inesperado.");
            }
        }


        /// <summary>
        /// Crear un nuevo Polígono en 2D.
        /// </summary>
        /// <param name="VerticesList">Vertices del polígono. [System.Drawing]</param>
        /// <param name="Layer">Capa del Polígono</param>
        /// <param name="Closed">Establece si el polígono es cerrado</param>
        public static void AddPolyline2D(PointF[] VerticesList, string Layer, bool Closed = true)
        {
            try
            {
                if (AcadDoc != null)
                {
                    AcadLWPolyline polyline = AcadDoc.ModelSpace.AddLightWeightPolyline(ConvertirPuntosEnDoubles(VerticesList));
                    polyline.Layer = Layer;
                    polyline.Closed = Closed;
                    polyline.Update();
                }
            }
            catch
            {
                NotificadorErrores?.Invoke("Error inesperado.");
            }
        }


        /// <summary>
        /// Crear una circunferencia
        /// </summary>
        /// <param name="Center">Insertion point del cirvulo. Ejemplo: (x,y,z)</param>
        /// <param name="Layer">Capa del Polígono</param>
        /// <param name="Radio">Radio de la circunferencia</param>
        public static void AddCircle(double[] Center, double Radio, string Layer)
        {
            try
            {
                if (AcadDoc != null)
                {
                    AcadCircle acadCircle = AcadDoc.ModelSpace.AddCircle(Center, Radio);
                    acadCircle.Layer = Layer;
                    acadCircle.Update();
                }
            }
            catch
            {
                NotificadorErrores?.Invoke("Error inesperado.");
            }
        }



        /// <summary>
        /// Crear un nuevo Polígono en 2D con su respectivo Relleno (Hatch).
        /// </summary>
        /// <param name="VerticesList">Vértices del Polígono</param>
        /// <param name="Layer">Capa del Polígono.</param>
        /// <param name="Pattern">Tipo de Hatch (SOILD,ANGLE,ANS31,...)</param>
        /// <param name="LayerHatch">Capa del Hatch</param>
        /// <param name="ScaleHacth">Escala del Hatch</param>
        /// <param name="PatternScale">Factor de escala del tipo de Hatch</param>
        /// <param name="PatternAngle">Angulo del tipo de Hatch</param>
        public static void AddPolyline2D(double[] VerticesList, string Layer, string Pattern, string LayerHatch, double ScaleHacth, double PatternScale = 0.009, float PatternAngle = 45)
        {
            try
            {
                if (AcadDoc != null)
                {
                    AcadLWPolyline polyline = AcadDoc.ModelSpace.AddLightWeightPolyline(VerticesList);
                    polyline.Layer = Layer;
                    polyline.Closed = true;
                    AddHatch((AcadEntity)polyline, Pattern, LayerHatch, ScaleHacth, PatternScale, PatternAngle);
                    polyline.Update();
                }
            }
            catch
            {
                NotificadorErrores?.Invoke("Error inesperado.");
            }
        }


        /// <summary>
        /// Crear un nuevo Polígono en 2D con su respectivo Relleno (Hatch).
        /// </summary>
        /// <param name="VerticesList">Vértices del Polígono [System.Drawing]</param>
        /// <param name="Layer">Capa del Polígono.</param>
        /// <param name="Pattern">Tipo de Hatch (SOILD,ANGLE,ANS31,...)</param>
        /// <param name="LayerHatch">Capa del Hatch</param>
        /// <param name="ScaleHacth">Escala del Hatch</param>
        /// <param name="PatternScale">Factor de escala del tipo de Hatch</param>
        /// <param name="PatternAngle">Angulo del tipo de Hatch</param>
        public static void AddPolyline2D(PointF[] VerticesList, string Layer, string Pattern, string LayerHatch, double ScaleHacth, double PatternScale = 0.009, float PatternAngle = 45)
        {
            try
            {
                if (AcadDoc != null)
                {
                    AddPolyline2D(ConvertirPuntosEnDoubles(VerticesList), Layer, Pattern, LayerHatch, ScaleHacth, PatternScale, PatternAngle);
                }
            }
            catch
            {
                NotificadorErrores?.Invoke("Error inesperado.");
            }
        }









        /// <summary>
        ///  Añadir un nuevo Poligono y un texto representado el largo de este.
        /// </summary>
        /// <param name="VerticesPolyline">Vertices del polígono. Ejemplo: (X1,Y1, X2,Y2, X3,Y3 X4,Y4)</param>
        /// <param name="LayerPolyline">Capa del Polígono</param>
        /// <param name="TextString">Texto adicional al largo del Polígono</param>
        /// <param name="PText_XYZ">Coordenadas del texto (X,Y,Z).</param>
        /// <param name="Width">Alto del Texto.</param>
        /// <param name="Height">Ancho del Texto.</param>
        /// <param name="LayerText">Capa del Texto.</param>
        /// <param name="StyleText">Estilo del Texto</param>
        /// <param name="Rotation">Ángulo de rotación del texto en grados </param>
        /// <param name="Width2">Ancho del cuadro de Texto.</param>

        public static void AddPolyline2DWithLengthText(double[] VerticesPolyline, string LayerPolyline, string TextString, double[] PText_XYZ, double Width, double Height, string LayerText, string StyleText, float Rotation, double Width2 = 1.3)
        {
            try
            {
                if (AcadDoc != null)
                {
                    AcadLWPolyline polyline = AcadDoc.ModelSpace.AddLightWeightPolyline(VerticesPolyline);
                    polyline.Layer = LayerPolyline;
                    polyline.Update();
                    TextString += @"%<\AcObjProp Object(%<\_ObjId " + polyline.ObjectID + @">%).Length \f " + (char)(34) + "%lu2%pr2" + (char)(34) + ">%";
                    AddText(TextString, PText_XYZ, Width, Height, LayerText, StyleText, Rotation, Width2: Width2);
                }
            }
            catch
            {
                NotificadorErrores?.Invoke("Error inesperado.");
            }
        }



        /// <summary>
        ///  Añadir un nuevo Poligono y un texto representado el largo de este.
        /// </summary>
        /// <param name="VerticesPolyline">Vertices del polígono. [System.Drawing]</param>
        /// <param name="LayerPolyline">Capa del Polígono</param>
        /// <param name="TextString">Texto adicional al largo del Polígono</param>
        /// <param name="PText_XYZ">Coordenadas del texto (X,Y,Z).</param>
        /// <param name="Width">Alto del Texto.</param>
        /// <param name="Height">Ancho del Texto.</param>
        /// <param name="LayerText">Capa del Texto.</param>
        /// <param name="StyleText">Estilo del Texto</param>
        /// <param name="Rotation">Ángulo de rotación del texto en grados </param>
        /// <param name="Width2">Ancho del cuadro de Texto.</param>
        /// <param name="JustifyText">Justificación del Texto.</param>
        public static void AddPolyline2DWithLengthText(PointF[] VerticesPolyline, string LayerPolyline, string TextString, double[] PText_XYZ, double Width, double Height, string LayerText, string StyleText, float Rotation, double Width2 = 1.3, JustifyText JustifyText = JustifyText.Left)
        {
            try {
                if (AcadDoc != null)
                {
                    AcadLWPolyline polyline = AcadDoc.ModelSpace.AddLightWeightPolyline(ConvertirPuntosEnDoubles(VerticesPolyline));
                    polyline.Layer = LayerPolyline;
                    polyline.Update();
                    TextString += @"%<\AcObjProp Object(%<\_ObjId " + polyline.ObjectID + @">%).Length \f " + (char)(34) + "%lu2%pr2" + (char)(34) + ">%";
                    AddText(TextString, PText_XYZ, Width, Height, LayerText, StyleText, Rotation, Width2: Width2, JustifyText: JustifyText);
                }
            }
            catch
            {
                NotificadorErrores?.Invoke("Error inesperado.");
            }
        }


        //BLOQUES ESPECIFICOS DE EFE PRIMA CE

        /// <summary>
        /// Bloque: Nivel de Sección - Efe Prima Ce
        /// </summary>
        /// <param name="P_XYZ">Coordenadas del Bloque.</param>
        /// <param name="Nivel">Texto correspondiente al nivel especificado, se recomienda asignar el texto así:  N+/PLosa xx, “/P” es para agregar un espacio.</param>
        /// <param name="Distance1">Largo de la línea superior del bloque, en Escalas 1:75 se recomienda: 1.08 </param>
        /// <param name="Distance2">Largo de la línea inferior del bloque medido desde el <paramref name="P_XYZ"/>, en Escalas 1:75 se recomienda: 1.90</param>
        /// <param name="FlipSate1">Dirección del Bloque, False: Izquierda, True: Derecha</param>
        /// <param name="Layer">Capa del Bloque.</param>
        /// <param name="Xscale">Escala en X del Bloque.</param>
        /// <param name="Yscale">Escala en Y del Bloque.</param>
        /// <param name="Zscale">Escala en Z del Bloque.</param>
        /// <param name="Rotation">Ángulo de rotación en grados del Bloque.</param>

        public static void B_NivelSeccion(double[] P_XYZ, string Nivel, double Distance1, double Distance2, bool FlipSate1, string Layer, double Xscale, double Yscale, double Zscale, float Rotation)
        {

            try
            {
                if (AcadDoc != null)
                {
                    AcadBlockReference blockReference = AcadDoc.ModelSpace.InsertBlock(P_XYZ, "FC_B_Nivel seccion", Xscale, Yscale, Zscale, Rotation);
                    blockReference.Layer = Layer;

                    var referenceProperty = blockReference.GetDynamicBlockProperties();
                    var attributeReference = blockReference.GetAttributes();

                    referenceProperty[0].Value = Distance1;
                    referenceProperty[2].Value = Distance2;

                    if (FlipSate1 == true)
                    {
                        referenceProperty[4].Value = Convert.ToInt16(1);
                    }

                    attributeReference[0].TextString = Nivel;
                    blockReference.Update();
                }
            }
            catch (Exception ex)
            {
                NotificadorErrores?.Invoke(ex.Message);
            }
        }

        /// <summary>
        /// Bloque: Nombre de Sección - Efe Prima Ce
        /// </summary>
        public static void B_NombreSeccion(double[] P_XYZ, string Seccion, string Escala, string Layer, double Xscale, double Yscale, double Zscale, float Rotation)
        {

            try
            {
                if (AcadDoc != null)
                {
                    AcadBlockReference blockReference = AcadDoc.ModelSpace.InsertBlock(P_XYZ, "FC_B_Titulo 1", Xscale, Yscale, Zscale, Rotation);
                    blockReference.Layer = Layer;


                    var attributeReference = blockReference.GetAttributes();

                    double Distance1 = (Seccion.Length - 3) * 0.05f;

                    attributeReference[0].TextString = Seccion;
                    attributeReference[1].TextString = Escala;



                    var referenceProperty = blockReference.GetDynamicBlockProperties();
                    referenceProperty[0].Value = Distance1;

                    blockReference.Update();
                }
            }
            catch (Exception ex)
            {
                NotificadorErrores?.Invoke(ex.Message);
            }
        }

        /// <summary>
        /// Bloque: Corte de Sección - Efe Prima Ce
        /// </summary>
        /// <param name="P_XYZ">Coordenadas del Bloque</param>
        /// <param name="X1">Texto del corte, punta inicial.</param>
        /// <param name="X2">Texto del corte, punta final.</param>
        /// <param name="Distance1">Distancia medida desde el <paramref name="P_XYZ"/>, usualmente se asigna el espesor del elemento hacer el corte.</param>
        /// <param name="Position1X">Posición en X del Texto ubicado en la punta inicial, se recomienda en Columnas,Escala 1:75 0.1.</param>
        /// <param name="Position1Y">Posición en Y del Texto ubicado en la punta inicial, se recomienda en Columnas,Escala 1:75 -0.1688.</param>
        /// <param name="Position2X">Posición en X del Texto ubicado en la punta final, se recomienda en Columnas,Escala 1:75 <paramref name="Distance1"/> + 1.0  - <paramref name="Position1X"/></param>
        /// <param name="Position2Y">Posición en Y del Texto ubicado en la punta final, se recomienda en Columnas,Escala 1:75 -0.1688.</param>
        /// <param name="FlipSate1">Dirección del Bloque: True: Abajo, False: Arriba</param>
        /// <param name="Layer">Capa del Bloque.</param>
        /// <param name="Xscale">Escala en X del Bloque.</param>
        /// <param name="Yscale">Escala en Y del Bloque.</param>
        /// <param name="Zscale">Escala en Z del Bloque.</param>
        /// <param name="Rotation">Ángulo de rotación en grados del Bloque.</param>
        public static void B_Seccion(double[] P_XYZ, string X1, string X2, double Distance1, double Position1X, double Position2X, double Position1Y, double Position2Y, bool FlipSate1, string Layer, double Xscale, double Yscale, double Zscale, float Rotation)
        {
            if (AcadDoc != null)
            {
                AcadBlockReference blockReference = AcadDoc.ModelSpace.InsertBlock(P_XYZ, "FC_B_Seccion", Xscale, Yscale, Zscale, Rotation);
                blockReference.Layer = Layer;

                var referenceProperty = blockReference.GetDynamicBlockProperties();
                var attributeReference = blockReference.GetAttributes();

                referenceProperty[0].Value = Position1X;
                referenceProperty[1].Value = Position1Y;
                referenceProperty[4].Value = Distance1;
                referenceProperty[2].Value = Position2X;
                referenceProperty[3].Value = Position2Y;

                if (FlipSate1 == true)
                {
                    referenceProperty[6].Value = Convert.ToInt16(1);
                }

                attributeReference[0].TextString = X1;
                attributeReference[1].TextString = X2;
                blockReference.Update();
            }
        }


        /// <summary>
        /// Bloque: Eje en Vigas y Nervios - efe Prima Ce
        /// </summary>
        /// <param name="P_XY">Coordenadas del Bloque [Sytem.Drawing]</param>
        /// <param name="Texto">Texto correspondiente al eje.</param>
        /// <param name="Layer">Capa del Bloque.</param>
        /// <param name="Xscale">Escala en X del Bloque.</param>
        /// <param name="Yscale">Escala en Y del Bloque.</param>
        /// <param name="Zscale">Escala en Z del Bloque.</param>
        /// <param name="Rotation">Ángulo de rotación en grados del Bloque.</param>
        public static void B_Ejes(PointF P_XY, string Texto, string Layer, double Xscale, double Yscale, double Zscale, double Rotation)
        {
            try
            {
                if (AcadDoc != null)
                {
                    double[] P_XYZ = ConvertirPuntoEnDobules3D(P_XY);
                    AcadBlockReference blockReference = AcadDoc.ModelSpace.InsertBlock(P_XYZ, "FC_B_Eje", Xscale, Yscale, Zscale, Rotation);
                    blockReference.Layer = Layer;
                    var attributeReference = blockReference.GetAttributes();
                    attributeReference[0].TextString = Texto;
                    blockReference.Update();
                }
            }
            catch (Exception ex)
            {
                NotificadorErrores?.Invoke(ex.Message);
            }
        }


        /// <summary>
        /// Bloque: Estibo de Sección - Efe Prima Ce
        /// </summary>
        public static void B_Estribo(double[] P_XYZ, string Layer, double Base,double Altura,double Xscale, double Yscale, double Zscale, float Rotation)
        {
            if (AcadDoc != null)
            {
                AcadBlockReference blockReference = AcadDoc.ModelSpace.InsertBlock(P_XYZ, "FC_B_Estribo tipo 1", Xscale, Yscale, Zscale, Rotation);

                var referenceProperty = blockReference.GetDynamicBlockProperties();

                referenceProperty[0].Value = Base;
                referenceProperty[2].Value = Altura;

                blockReference.Layer = Layer;
                blockReference.Update();
            }
        }



        /// <summary>
        /// Bloque de Estribo Tipo 6, normalmente se usa para Tabla de Estribos en Muros [efe Prima Ce]
        /// </summary>
        /// <param name="P_XYZ">Coordenadas del Bloque</param>
        /// <param name="Layer">Capa del Bloque.</param>
        /// <param name="Xscale">Escala en X del Bloque.</param>
        /// <param name="Yscale">Escala en Y del Bloque.</param>
        /// <param name="Zscale">Escala en Z del Bloque.</param>
        /// <param name="Rotation">Ángulo de rotación en grados del Bloque.</param>
        /// <param name="Distancia">Distancia recta del Estribo</param>
        /// <param name="EspesorReal">Espesor Real.</param>
        /// <param name="EspesorDoble">Espesor Doble.</param>
        public static void B_Estribo(double[] P_XYZ, string Layer, double Xscale, double Yscale, double Zscale, double Rotation, double Distancia, double EspesorReal, double EspesorDoble, double GanchoEstribo=0.09)
        {

            if (AcadDoc != null)
            {

                AcadBlockReference blockReference = AcadDoc.ModelSpace.InsertBlock(P_XYZ, "FC_B_Estribo tipo 6", Xscale, Yscale, Zscale, Rotation);
                blockReference.Layer = Layer;

                var referenceProperty = blockReference.GetDynamicBlockProperties();

                referenceProperty[0].Value = Distancia;
                referenceProperty[2].Value = EspesorDoble;
                referenceProperty[4].Value = EspesorReal;
                referenceProperty[6].Value = GanchoEstribo;

            }



        }


        /// <summary>
        /// Bloque de Estribo Tipo 6, normalmente se usa para Tabla de Estribos en Muros [efe Prima Ce]
        /// </summary>
        /// <param name="P_XY">Coordenadas del Bloque. [System.Drawing]</param>
        /// <param name="Layer">Capa del Bloque.</param>
        /// <param name="Xscale">Escala en X del Bloque.</param>
        /// <param name="Yscale">Escala en Y del Bloque.</param>
        /// <param name="Zscale">Escala en Z del Bloque.</param>
        /// <param name="Rotation">Ángulo de rotación en grados del Bloque.</param>
        /// <param name="Distancia">Distancia recta del Estribo</param>
        /// <param name="EspesorReal">Espesor Real.</param>
        /// <param name="EspesorDoble">Espesor Doble.</param>
        public static void B_Estribo(PointF P_XY, string Layer, double Xscale, double Yscale, double Zscale, double Rotation, double Distancia, double EspesorReal, double EspesorDoble, double GanchoEstribo = 0.09)
        {
            if (AcadDoc != null)
            {

                AcadBlockReference blockReference = AcadDoc.ModelSpace.InsertBlock(ConvertirPuntoEnDobules3D(P_XY), "FC_B_Estribo tipo 6", Xscale, Yscale, Zscale, Rotation);
                blockReference.Layer = Layer;

                var referenceProperty = blockReference.GetDynamicBlockProperties();

                referenceProperty[0].Value = Distancia;
                referenceProperty[2].Value = EspesorDoble;
                referenceProperty[4].Value = EspesorReal;
                referenceProperty[6].Value = GanchoEstribo;

            }
        }



        /// <summary>
        /// Bloque: Estibo para secciones circulares - Efe Prima Ce
        /// </summary>
        public static void B_Estribo_Circular(double[] P_XYZ, string Layer, double Radio, double Xscale, double Yscale, double Zscale, float Rotation)
        {
            if (AcadDoc != null)
            {
                AcadBlockReference blockReference = AcadDoc.ModelSpace.InsertBlock(P_XYZ, "FC_B_Estribo circular", Xscale, Yscale, Zscale, Rotation);
            }
        }


        /// <summary>
        /// Bloque: Ganchos Sección - Efe Prima Ce
        /// </summary>
        public static void B_Gancho(double[] P_XYZ, string Layer, double Longitud, double Xscale, double Yscale, double Zscale, float Rotation,short FlipState)
        {
            if (AcadDoc != null)
            {
                AcadBlockReference blockReference = AcadDoc.ModelSpace.InsertBlock(P_XYZ, "FC_B_Gancho tipo 1", Xscale, Yscale, Zscale, 0);

                var referenceProperty = blockReference.GetDynamicBlockProperties();
                referenceProperty[2].Value = Longitud;
                referenceProperty[4].Value = FlipState;
                referenceProperty[5].Value = Math.PI * Rotation / 180;

                blockReference.Layer = Layer;
                blockReference.Update();
            }
        }



        /// <summary>
        /// Bloque: Conveciones de refuerzo - Efe Prima Ce
        /// </summary>
        /// <param name="P_XYZ">Coordenadas del Bloque</param>
        ///<param name="NombreRefuerzo">Numero de refuerzo.</param>
        /// <param name="Layer">Capa del Bloque.</param>
        /// <param name="Xscale">Escala en X del Bloque.</param>
        /// <param name="Yscale">Escala en Y del Bloque.</param>
        /// <param name="Zscale">Escala en Z del Bloque.</param>
        /// <param name="Rotation">Ángulo de rotación en grados del Bloque.</param>
        public static void B_CirculoRefuerzo(double[] P_XYZ,string NombreRefuerzo, string Layer, double Xscale, double Yscale, double Zscale, float Rotation)
        {
            if (AcadDoc != null)
            {
                AcadBlockReference blockReference = AcadDoc.ModelSpace.InsertBlock(P_XYZ, "FC_B_Convenciones refuerzoV2", Xscale, Yscale, Zscale, Rotation);
                blockReference.Layer = Layer;

                var referenceProperty = blockReference.GetDynamicBlockProperties();

                referenceProperty[0].Value = NombreRefuerzo;
                blockReference.Update();
            }
        }

        /// <summary>
        /// Bloque: Conveciones de refuerzo - Efe Prima Ce
        /// </summary>
        /// <param name="P_XY">Coordenadas del Bloque [System.Drawing]</param>
        ///<param name="NombreRefuerzo">Numero de refuerzo.</param>
        /// <param name="Layer">Capa del Bloque.</param>
        /// <param name="Xscale">Escala en X del Bloque.</param>
        /// <param name="Yscale">Escala en Y del Bloque.</param>
        /// <param name="Zscale">Escala en Z del Bloque.</param>
        /// <param name="Rotation">Ángulo de rotación en grados del Bloque.</param>
        public static void B_CirculoRefuerzo(PointF P_XY, string NombreRefuerzo, string Layer, double Xscale, double Yscale, double Zscale, float Rotation)
        {
            if (AcadDoc != null)
            {
                double[] P_XYZ = ConvertirPuntoEnDobules3D(P_XY);
                AcadBlockReference blockReference = AcadDoc.ModelSpace.InsertBlock(P_XYZ, "FC_B_Convenciones refuerzoV2", Xscale, Yscale, Zscale, Rotation);
                blockReference.Layer = Layer;

                var referenceProperty = blockReference.GetDynamicBlockProperties();

                referenceProperty[0].Value = NombreRefuerzo;
                blockReference.Update();
            }
        }












        /// <summary>
        /// Bloque: Refuerzo i en seccion - Efe Prima Ce
        /// </summary>
        public static void Add_ref(double[] P_XYZ, string Layer,int Alzado, double Xscale, double Yscale, double Zscale, float Rotation)
        {
            AcadBlockReference blockReference = AcadDoc.ModelSpace.InsertBlock(P_XYZ, "FC_B_Convenciones refuerzoV2", Xscale, Yscale, Zscale, Rotation);

            var referenceProperty = blockReference.GetDynamicBlockProperties();
            referenceProperty[0].Value = Alzado.ToString();

            blockReference.Layer = Layer;
            blockReference.Update();
        }

        private static void AddHatch(AcadEntity entity, string Pattern, string LayerHatch, double ScaleHacth, double PatternScale, double PatternAngle)
        {
            AcadHatch hatch = AcadDoc.ModelSpace.AddHatch(0, Pattern, true);
            AcadEntity[] entities = new AcadEntity[] { entity };
            hatch.AppendInnerLoop(entities);
            hatch.Layer = LayerHatch;
            hatch.LinetypeScale = ScaleHacth;
            hatch.PatternAngle = PatternAngle;
            hatch.PatternScale = PatternScale;
            BackHatch((AcadObject)hatch);
            hatch.Update();
        }

        private static void BackHatch(AcadObject AcadObject)
        {
            AcadDictionary dictionary = AcadDoc.ModelSpace.GetExtensionDictionary();
            AcadSortentsTable sentityObj = (AcadSortentsTable)dictionary.GetObject("ACAD_SORTENTS");
            AcadObject[] entities = new AcadObject[] { AcadObject };
            sentityObj.MoveToBottom(entities);
        }

        private static AcDimPrecision Clasf_precision(PrecisionCota presicionCota)
        {
            if (presicionCota == PrecisionCota.Cero)
            { return AcDimPrecision.acDimPrecisionZero; }
            else if (presicionCota == PrecisionCota.Uno)
            { return AcDimPrecision.acDimPrecisionOne; }
            else if (presicionCota == PrecisionCota.Dos)
            { return AcDimPrecision.acDimPrecisionTwo; }
            else if (presicionCota == PrecisionCota.Tres)
            { return AcDimPrecision.acDimPrecisionThree; }
            else if (presicionCota == PrecisionCota.Cuatro)
            { return AcDimPrecision.acDimPrecisionFour; }
            else if (presicionCota == PrecisionCota.Cinco)
            { return AcDimPrecision.acDimPrecisionFive; }
            else if (presicionCota == PrecisionCota.Seis)
            { return AcDimPrecision.acDimPrecisionSix; }
            else { return AcDimPrecision.acDimPrecisionTwo; }
        }

        private static AcAttachmentPoint Clasf_JustyText(JustifyText justify)
        {
            if (justify == JustifyText.Center)
            {
                return AcAttachmentPoint.acAttachmentPointMiddleCenter;
            }
            else if (justify == JustifyText.Right)
            {
                return AcAttachmentPoint.acAttachmentPointBottomRight;
            }
            else
            {
                return AcAttachmentPoint.acAttachmentPointBottomLeft;
            }
        }

        private static AcDimArrowheadType Clasf_ArrowHeadType(ArrowHeadType headType)
        {
            if (headType == ArrowHeadType.ArrowDefault)
            {
                return AcDimArrowheadType.acArrowDefault;
            }
            else if (headType == ArrowHeadType.ArrowDot)
            {
                return AcDimArrowheadType.acArrowDot;
            }
            else
            {
                return AcDimArrowheadType.acArrowDot;
            }
        }

        private static double Distancia(double[] P1_XYZ, double[] P2_XYZ)
        {
            if (P1_XYZ.Length == 3 & P2_XYZ.Length == 3)
            {
                return Math.Sqrt(Math.Pow(P1_XYZ[0] - P2_XYZ[0], 2) + Math.Pow(P1_XYZ[1] - P2_XYZ[1], 2) + Math.Pow(P1_XYZ[2] - P2_XYZ[2], 2));
            }
            else
            {
                return 0;
            }
        }

        private static double[] ConvertirPuntosEnDoubles(PointF[] Puntos)
        {
            List<double> PuntosF = new List<double>();
            foreach (PointF Punto in Puntos)
            {
                PuntosF.Add(Punto.X); PuntosF.Add(Punto.Y);
            }
            return PuntosF.ToArray();
        }

        private static double[] ConvertirPuntoEnDobules3D(PointF XY)
        {
            return new double[] { XY.X, XY.Y,0 };
        }


    }
}