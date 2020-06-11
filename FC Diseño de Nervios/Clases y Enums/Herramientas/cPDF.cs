using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Microsoft.SqlServer.Server;

namespace FC_Diseño_de_Nervios
{
    public static class cPDF
    {


        public static void CrearPDF(string Ruta, List<cNervio> Nervios)
        {
            Document Doc = new Document(PageSize.LETTER, 30, 25, 100, 60);
            PdfWriter Writer = PdfWriter.GetInstance(Doc, new FileStream(Ruta, FileMode.Create));

            Doc.AddTitle("Memorias de Cálculo");
            Doc.AddCreator("efe Prima Ce");
            Doc.Open();

            

            //AddSpaces(4, Doc);



            Nervios.ForEach(Nervio => 
            {
                List<PdfPTable> Tablas = CrearTablasNervio(Nervio);
                foreach (PdfPTable tabla in Tablas)
                {
                    Doc.Add(tabla);
                }
                //AddSpaces(1, Doc);
            });
            
            for(int i=0; i<Doc.PageNumber;i++)
                EncabezadoEfePrimaCe(Doc, Writer);





            Doc.Close();
            Writer.Close();
            System.Diagnostics.Process Proc = new System.Diagnostics.Process();
            Proc.StartInfo.FileName = Ruta;
            Proc.Start();
        }

        private static Font RegistrerFont(string fontname, float Size, int Style, BaseColor Color)
        {
            if (!FontFactory.IsRegistered(fontname))
            {
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), $"{fontname.ToLower()}.ttf");
                FontFactory.Register(fontPath);

            }
            return FontFactory.GetFont(fontname, Size, Style, Color);
        }


        private static void CreateLine(PdfWriter writer, float LineWidth, float x1, float y1, float x2, float y2)
        {
            PdfContentByte Linea = writer.DirectContent;
            Linea.SetLineWidth(LineWidth);
            Linea.MoveTo(x1, y1);
            Linea.LineTo(x2, y2);
            Linea.Stroke();
        }

        private static void AddTextWithCoordinates(PdfWriter writer, string text, float x, float y, float rotation, int alignment, string font, float size, int style, BaseColor color)
        {
            PdfContentByte Text = writer.DirectContent;
            Text.BeginText();
            Font font1 = RegistrerFont(font, size, style, color);
            ColumnText.ShowTextAligned(Text, alignment, new Phrase(text, font1), x, y - font1.Size / 2f, rotation);
            Text.EndText();
        }

        private static float GetWidthText(string text, string font, float size, int style)
        {
            Font font1 = RegistrerFont(font, size, style, BaseColor.BLACK);
            Phrase phrase = new Phrase(text, font1);
            return ColumnText.GetWidth(phrase);
        }


        public static void EncabezadoEfePrimaCe(Document Doc, PdfWriter Writer)
        {
            float MarginTop = 35f; float MarginBottom = 25f; float MarginLeft= 30f; float MarginRight = 25f;


            Image img = Image.GetInstance(Properties.Resources.logoefeprimace, System.Drawing.Imaging.ImageFormat.Png);
            img.ScalePercent(40f);
            img.SetAbsolutePosition(Doc.Left, Doc.PageSize.Top- MarginTop - img.Height / 3.5f);
            Doc.Add(img);
            string text1 = "Memorias de Cálculo"; float widthtext = GetWidthText(text1, "Verdana", 9f, Font.ITALIC);
            string text2 = "Cra. 50 FF 8 Sur 27, Edificio 808 Empresarial, Oficina 308, Medellín, Colombia. Tel: +57(4)4485569";
            string text3 = "fc@efeprimace.co          www.efeprimace.co";
            //Lineas
            #region Encabezado
            CreateLine(Writer, 0.50f, Doc.PageSize.Left + MarginLeft + 160, Doc.PageSize.Top - MarginTop - 5f, Doc.PageSize.Left+MarginLeft +400, Doc.PageSize.Top - MarginTop - 5f); // Linea Horizontal
            CreateLine(Writer, 0.50f, Doc.PageSize.Left + MarginLeft + 410 + widthtext, Doc.PageSize.Top - MarginTop - 5f, Doc.PageSize.Right-MarginRight - 30, Doc.PageSize.Top - MarginTop - 5f); // Linea Horizontal
            CreateLine(Writer, 0.50f, Doc.PageSize.Right - MarginRight - 30, Doc.PageSize.Top - MarginTop-5f, Doc.PageSize.Right - MarginRight - 30, Doc.PageSize.Top - MarginTop - 35f); // Linea Vertical
            #endregion
            #region Pie de Pagina
            CreateLine(Writer, 0.50f, Doc.PageSize.Left+MarginLeft + 20, Doc.PageSize.Bottom+MarginBottom + 60f, Doc.PageSize.Left + MarginLeft + 20, Doc.PageSize.Bottom + MarginBottom + 25f); // Linea Vertical
            CreateLine(Writer, 0.50f, Doc.PageSize.Left + MarginLeft + 20, Doc.PageSize.Bottom + MarginBottom + 25f, Doc.PageSize.Right-MarginRight - 20, Doc.PageSize.Bottom + MarginBottom + 25f); // Linea Horizontal
            #endregion
            AddTextWithCoordinates(Writer, text1, Doc.PageSize.Left + MarginLeft + 450, Doc.PageSize.Top - MarginTop - 5f, 0f, Element.ALIGN_CENTER, "Verdana", 9f, Font.ITALIC, BaseColor.BLACK);
            AddTextWithCoordinates(Writer, text2, Doc.PageSize.Left+MarginLeft + 30, Doc.PageSize.Bottom + MarginBottom+ 15f, 0f, Element.ALIGN_LEFT, "Verdana", 10f, Font.ITALIC, BaseColor.BLACK);
            AddTextWithCoordinates(Writer, text3, Doc.PageSize.Left+MarginLeft + 190, Doc.PageSize.Bottom + MarginBottom, 0f, Element.ALIGN_LEFT, "Verdana", 10f, Font.ITALIC, new BaseColor(255, 102, 0));
        }
        public static void AddSpaces(int CountSpaces, Document Doc)
        {
            PdfPTable Tabla = new PdfPTable(1);
            for (int i = 0; i < CountSpaces; i++)
            {
                PdfPCell Celda = new PdfPCell(new Phrase("          "));
                Celda.Border = 0;
                Celda.HorizontalAlignment = 0;
                Tabla.AddCell(Celda);

            }
            Doc.Add(Tabla);
        }



        public static List<PdfPTable> CrearTablasNervio(cNervio nervio)
        {

            List<PdfPTable> Tablas = new List<PdfPTable>();



            List<cSubTramo> Subtramos = nervio.Lista_Elementos.FindAll(y => y is cSubTramo).ConvertAll(new Converter<IElemento, cSubTramo>(cFunctionsProgram.ElementoASubtramo));

            int Cantidad = Subtramos.Count;


            if (Cantidad > 1)
            {
                PdfPTable Tabla = new PdfPTable(1);
                PdfPCell cell = new PdfPCell(new Phrase(nervio.Nombre));
                Tabla.AddCell(Vcell(nervio.Nombre, BordAb: 0, Colspan: 1));
                Tablas.Add(Tabla);
                for (int i = 0; i <= Cantidad / 2; i += 2)
                {
                    PdfPTable Tabla2 = new PdfPTable(6); 

                    string Text1 = $"Sección (BXH) {Subtramos[i].Seccion.B}x{Subtramos[i].Seccion.H} L={Subtramos[i].Longitud}m";
                    string Text2 = $"Sección (BXH) {Subtramos[i + 1].Seccion.B}x{Subtramos[i + 1].Seccion.H} L={Subtramos[i + 1].Longitud}m";

                    Tabla2.AddCell(Vcell(Text1, BordeD: 0, BordAb: 0, Colspan: 3));
                    Tabla2.AddCell(Vcell(Text2, BordAb: 0, Colspan: 3));

                    object[][][] DatosResumidos1 = cFunctionsProgram.EstacionesResumidas(Subtramos[i].Estaciones);
                    object[][][] DatosResumidos2 = cFunctionsProgram.EstacionesResumidas(Subtramos[i + 1].Estaciones);

                    #region Parte Negativa

                    Tabla2.AddCell(Vcell($"Mu-={string.Format("{0:0.00}", DatosResumidos1[1][1][0])}", BordeD: 0, BordAb: 0));
                    Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordAb: 0));
                    Tabla2.AddCell(Vcell($"Mu-={string.Format("{0:0.00}", DatosResumidos1[1][1][1])}", BordeI: 0, BordeD: 0, BordAb: 0));

                    Tabla2.AddCell(Vcell($"Mu-={string.Format("{0:0.00}", DatosResumidos2[1][1][0])}", BordeD: 0, BordAb: 0));
                    Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordAb: 0));
                    Tabla2.AddCell(Vcell($"Mu-={DatosResumidos2[1][1][1]}", BordeI: 0, BordAb: 0));

                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[1][2][0])}", BordeD: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[1][2][1])}", BordeI: 0, BordeD: 0, BordAb: 0, BordeAr: 0));

                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos2[1][2][0])}", BordeD: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos2[1][2][1])}", BordeI: 0, BordAb: 0, BordeAr: 0));

                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[1][0][0])}", BordeD: 0, BordeAr: 0, BordAb: 0.5f));
                    Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordeAr: 0, BordAb: 0.5f));
                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[1][0][1])}", BordeI: 0, BordeD: 0, BordeAr: 0, BordAb: 0.5f));

                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos2[1][0][0])}", BordeD: 0, BordeAr: 0, BordAb: 0.5f));
                    Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordeAr: 0, BordAb: 0.5f));
                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos2[1][0][1])}", BordeI: 0, BordeAr: 0, BordAb: 0.5f));
                    #endregion

                    #region PartePositiva
                    Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos1[0][1][0])}", BordeD: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos1[0][1][1])}", BordeI: 0, BordeD: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos1[0][1][2])}", BordeI: 0, BordeD: 0, BordAb: 0, BordeAr: 0));

                    Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos2[0][1][0])}", BordeD: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos2[0][1][1])}", BordeI: 0, BordeD: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos2[0][1][2])}", BordeI: 0, BordAb: 0, BordeAr: 0));


                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[0][2][0])}", BordeD: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[0][2][1])}", BordeI: 0, BordeD: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[0][2][2])}", BordeI: 0, BordeD: 0, BordAb: 0, BordeAr: 0));

                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos2[0][2][0])}", BordeD: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos2[0][2][1])}", BordeD: 0, BordeI: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos2[0][2][2])}", BordeI: 0, BordAb: 0, BordeAr: 0));


                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[0][0][0])}", BordeD: 0, BordeAr: 0, BordAb: 0.5f));
                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[0][0][1])}", BordeI: 0, BordeD: 0, BordeAr: 0, BordAb: 0.5f));
                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[0][0][2])}", BordeI: 0, BordeD: 0, BordeAr: 0, BordAb: 0.5f));

                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos2[0][0][0])}", BordeD: 0, BordeAr: 0, BordAb: 0.5f));
                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos2[0][0][1])}", BordeI: 0, BordeD: 0, BordeAr: 0, BordAb: 0.5f));
                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos2[0][0][2])}", BordeI: 0, BordeAr: 0, BordAb: 0.5f));



                    Tabla2.AddCell(Vcell($"V+={string.Format("{0:0.00}", DatosResumidos1[2][0][0])}", BordeD: 0, BordeAr: 0, BordAb: 0));
                    Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordeAr: 0, BordAb: 0));
                    Tabla2.AddCell(Vcell($"V-={string.Format("{0:0.00}", DatosResumidos1[2][0][1])}", BordeI: 0, BordeD: 0, BordeAr: 0, BordAb: 0));

                    Tabla2.AddCell(Vcell($"V+={string.Format("{0:0.00}", DatosResumidos2[2][0][0])}", BordeD: 0, BordeAr: 0, BordAb: 0));
                    Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordeAr: 0, BordAb: 0));
                    Tabla2.AddCell(Vcell($"V-={string.Format("{0:0.00}", DatosResumidos2[2][0][1])}", BordeI: 0, BordeAr: 0, BordAb: 0));




                    Tabla2.AddCell(Vcell($"{DatosResumidos1[2][1][0]}", BordeD: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordeAr: 0, BordAb: 1f));
                    Tabla2.AddCell(Vcell($"{DatosResumidos1[2][1][1]}", BordeI: 0, BordeAr: 0, BordeD: 0));

                    Tabla2.AddCell(Vcell($"{DatosResumidos2[2][1][0]}", BordeD: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordeAr: 0, BordAb: 1f));
                    Tabla2.AddCell(Vcell($"{DatosResumidos2[2][1][1]}", BordeI: 0, BordeAr: 0, BordAb: 1f));


                    #endregion

                    Tabla2.AddCell(Vcell($" ", 0, 0, 0, 0, 6));
                    Tablas.Add(Tabla2);
                }

                if (Cantidad % 2 != 0)
                {
                    cSubTramo SubtramoLast = Subtramos.Last();
                    PdfPTable Tabla2 = new PdfPTable(6);
                    string Text1 = $"Sección (BXH) {SubtramoLast.Seccion.B}x{SubtramoLast.Seccion.H} L={SubtramoLast.Longitud}m";

                    Tabla2.AddCell(Vcell(Text1, BordAb: 0, Colspan: 3)); Tabla2.AddCell(Vcell(" ", 0, 0, 0, 0, 3));

                    object[][][] DatosResumidos1 = cFunctionsProgram.EstacionesResumidas(SubtramoLast.Estaciones);

                    #region Parte Negativa

                    Tabla2.AddCell(Vcell($"Mu-={string.Format("{0:0.00}", DatosResumidos1[1][1][0])}", BordeD: 0, BordAb: 0));
                    Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordAb: 0));
                    Tabla2.AddCell(Vcell($"Mu-={string.Format("{0:0.00}", DatosResumidos1[1][1][1])}", BordeI: 0, BordAb: 0));
                    Tabla2.AddCell(Vcell(" ", 0, 0, 0, 0, 3));

                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[1][2][0])}", BordeD: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[1][2][1])}", BordeI: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell(" ", 0, 0, 0, 0, 3));

                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[1][0][0])}", BordeD: 0, BordeAr: 0, BordAb: 0.5f));
                    Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordeAr: 0, BordAb: 0.5f));
                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[1][0][1])}", BordeI: 0, BordeAr: 0, BordAb: 0.5f));
                    Tabla2.AddCell(Vcell(" ", 0, 0, 0, 0, 3));
                    #endregion

                    #region PartePositiva
                    Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos1[0][1][0])}", BordeD: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos1[0][1][1])}", BordeI: 0, BordeD: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos1[0][1][2])}", BordeI: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell(" ", 0, 0, 0, 0, 3));

                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[0][2][0])}", BordeD: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[0][2][1])}", BordeI: 0, BordeD: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[0][2][2])}", BordeI: 0, BordAb: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell(" ", 0, 0, 0, 0, 3));

                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[0][0][0])}", BordeD: 0, BordeAr: 0, BordAb: 0.5f));
                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[0][0][1])}", BordeI: 0, BordeD: 0, BordeAr: 0, BordAb: 0.5f));
                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[0][0][2])}", BordeI: 0, BordeAr: 0, BordAb: 0.5f));
                    Tabla2.AddCell(Vcell(" ", 0, 0, 0, 0, 3));

                    Tabla2.AddCell(Vcell($"V+={string.Format("{0:0.00}", DatosResumidos1[2][0][0])}", BordeD: 0, BordeAr: 0, BordAb: 0));
                    Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordeAr: 0, BordAb: 0));
                    Tabla2.AddCell(Vcell($"V-={string.Format("{0:0.00}", DatosResumidos1[2][0][1])}", BordeI: 0, BordeAr: 0, BordAb: 0));
                    Tabla2.AddCell(Vcell(" ", 0, 0, 0, 0, 3));

                    Tabla2.AddCell(Vcell($"{DatosResumidos1[2][1][0]}", BordeD: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordeAr: 0, BordAb: 1f));
                    Tabla2.AddCell(Vcell($"{DatosResumidos1[2][1][1]}", BordeI: 0, BordeAr: 0));
                    Tabla2.AddCell(Vcell(" ", 0, 0, 0, 0, 3));

                    #endregion

                    Tabla2.AddCell(Vcell($" ", 0, 0, 0, 0, 6));
                    Tablas.Add(Tabla2);


                }

            }
            else
            {
                PdfPTable Tabla = new PdfPTable(6);
                PdfPCell cell = new PdfPCell(new Phrase(nervio.Nombre));
                Tabla.AddCell(Vcell(nervio.Nombre, BordAb: 0, Colspan:3)); Tabla.AddCell(Vcell(" ", 0, 0, 0, 0, 3));
                Tablas.Add(Tabla);

                cSubTramo SubtramoLast = Subtramos.Last();
                PdfPTable Tabla2 = new PdfPTable(6);

                string Text1 = $"Sección (BXH) {SubtramoLast.Seccion.B}x{SubtramoLast.Seccion.H} L={SubtramoLast.Longitud}m";


                
                Tabla2.AddCell(Vcell(Text1, BordAb: 0, Colspan: 3)); Tabla2.AddCell(Vcell(" ", 0, 0, 0, 0, 3));

                object[][][] DatosResumidos1 = cFunctionsProgram.EstacionesResumidas(SubtramoLast.Estaciones);

                #region Parte Negativa
                
                Tabla2.AddCell(Vcell($"Mu-={string.Format("{0:0.00}", DatosResumidos1[1][1][0])}", BordeD: 0, BordAb: 0));
                Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordAb: 0));
                Tabla2.AddCell(Vcell($"Mu-={string.Format("{0:0.00}", DatosResumidos1[1][1][1])}", BordeI: 0, BordAb: 0));
                Tabla2.AddCell(Vcell(" ", 0, 0, 0, 0, 3));


                
                Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[1][2][0])}", BordeD: 0, BordAb: 0, BordeAr: 0));
                Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordAb: 0, BordeAr: 0));
                Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[1][2][1])}", BordeI: 0, BordAb: 0, BordeAr: 0));
                Tabla2.AddCell(Vcell(" ", 0, 0, 0, 0, 3));

                
                Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[1][0][0])}", BordeD: 0, BordeAr: 0, BordAb: 0.5f));
                Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordeAr: 0, BordAb: 0.5f));
                Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[1][0][1])}", BordeI: 0, BordeAr: 0, BordAb: 0.5f));
                Tabla2.AddCell(Vcell(" ", 0, 0, 0, 0, 3));
                #endregion

                #region PartePositiva
                
                Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos1[0][1][0])}", BordeD: 0, BordAb: 0, BordeAr: 0));
                Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos1[0][1][1])}", BordeI: 0, BordeD: 0, BordAb: 0, BordeAr: 0));
                Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos1[0][1][2])}", BordeI: 0, BordAb: 0, BordeAr: 0));
                Tabla2.AddCell(Vcell(" ", 0, 0, 0, 0, 3));

                Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[0][2][0])}", BordeD: 0, BordAb: 0, BordeAr: 0));
                Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[0][2][1])}", BordeI: 0, BordeD: 0, BordAb: 0, BordeAr: 0));
                Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[0][2][2])}", BordeI: 0, BordAb: 0, BordeAr: 0));
                Tabla2.AddCell(Vcell(" ", 0, 0, 0, 0, 3));

                Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[0][0][0])}", BordeD: 0, BordeAr: 0, BordAb: 0.5f));
                Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[0][0][1])}", BordeI: 0, BordeD: 0, BordeAr: 0, BordAb: 0.5f));
                Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[0][0][2])}", BordeI: 0, BordeAr: 0, BordAb: 0.5f));
                Tabla2.AddCell(Vcell(" ", 0, 0, 0, 0, 3));

                Tabla2.AddCell(Vcell($"V+={string.Format("{0:0.00}", DatosResumidos1[2][0][0])}", BordeD: 0, BordeAr: 0, BordAb: 0));
                Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordeAr: 0, BordAb: 0));
                Tabla2.AddCell(Vcell($"V-={string.Format("{0:0.00}", DatosResumidos1[2][0][1])}", BordeI: 0, BordeAr: 0, BordAb: 0));
                Tabla2.AddCell(Vcell(" ", 0, 0, 0, 0, 3)); 

                Tabla2.AddCell(Vcell($"{DatosResumidos1[2][1][0]}", BordeD: 0, BordeAr: 0));
                Tabla2.AddCell(Vcell($"  ", BordeI: 0, BordeD: 0, BordeAr: 0, BordAb: 1f));
                Tabla2.AddCell(Vcell($"{DatosResumidos1[2][1][1]}", BordeI: 0, BordeAr: 0));
                Tabla2.AddCell(Vcell(" ", 0, 0, 0, 0, 3));

                // 
                //Tabla2.AddCell(Vcell(" ", 0, 0, 0, 0, 3));

                #endregion

                Tabla2.AddCell(Vcell($" ", 0, 0, 0, 0, 6));
                Tablas.Add(Tabla2);

            }


            return Tablas;
        }

        private static PdfPCell Vcell(string Text, float BordeI = 1, float BordeD = 1, float BordeAr = 1, float BordAb = 1,int Colspan=1,int Rowspan=1)
        {
            Font font1 = RegistrerFont("Calibri", 11, Font.NORMAL, BaseColor.BLACK);
            PdfPCell cell = new PdfPCell(new Phrase(Text, font1)); cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidthLeft = BordeI;
            cell.BorderWidthRight = BordeD;
            cell.BorderWidthTop = BordeAr;
            cell.BorderWidthBottom = BordAb;
            cell.Colspan = Colspan;
            cell.Rowspan = Rowspan;
            return cell;
        }







    }

}
