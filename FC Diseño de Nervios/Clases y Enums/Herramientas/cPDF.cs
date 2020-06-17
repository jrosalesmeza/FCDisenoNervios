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

        public static void CrearPDF(string RutaInicial,string RutaFinal, List<cNervio> Nervios)
        {
            try
            {
                Document Doc = new Document(PageSize.LETTER, 30, 25, 90, 50);
                FileStream stream = new FileStream(RutaInicial, FileMode.Create);
                PdfWriter Writer = PdfWriter.GetInstance(Doc, stream);

                Doc.AddTitle("Memorias de Cálculo");
                Doc.AddCreator("efe Prima Ce");
                Doc.Open();


                EncabezadoEfePrimaCe(Doc, Writer);

                List<PdfPTable> Tablas = new List<PdfPTable>();
                Nervios.ForEach(Nervio =>
                {
                    Tablas.AddRange(CrearTablasNervio(Nervio));

                //AddSpaces(1, Doc);
            });

                int Contador = 0;
                for (int i = 0; i < Tablas.Count; i++)
                {

                    if (Contador == 5)
                    {
                        Doc.NewPage();
                        EncabezadoEfePrimaCe(Doc, Writer);
                        Contador = 0;
                    }
                    Doc.Add(Tablas[i]);
                    Contador += 1;

                }
                Doc.Close();
                Writer.Close();



                #region Eliminar Hojas en Blanco Del PDF
                int EnBlanco = 100;
                PdfReader reader = new PdfReader(RutaInicial);
                var raf = new RandomAccessFileOrArray(RutaInicial);
                Document Doc2 = new Document(reader.GetPageSizeWithRotation(1));
                PdfCopy Writer2 = new PdfCopy(Doc2, new FileStream(RutaFinal, FileMode.Create)); Doc2.Open();

                int n = reader.NumberOfPages;
                for (int i = 1; i <= n; i++)
                {
                    byte[] Bites = reader.GetPageContent(i, raf);
                    if (Bites.Length > EnBlanco)
                    {
                        PdfImportedPage page = Writer2.GetImportedPage(reader, i);
                        Writer2.AddPage(page);
                    }

                }
                Doc2.Close(); Writer2.Close(); raf.Close(); reader.Close();
                #endregion
                System.Diagnostics.Process Proc = new System.Diagnostics.Process();
                Proc.StartInfo.FileName = RutaFinal;
                Proc.Start();
            }
            catch
            {
                cFunctionsProgram.VentanaEmergenteExclamacion("La acción no se puede completar porque el archivo esta abierto.");
                return;
            }
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


            if (Cantidad >=3)
            {
                int Contador = 0;

                for (int i = 0; i <= Cantidad / 3; i += 3)
                {

                    PdfPTable Tabla2 = new PdfPTable(9);

                    string Text1 = $"Sección (BXH) {Subtramos[i].Seccion.B}x{Subtramos[i].Seccion.H} L={Subtramos[i].Longitud}m";
                    string Text2 = $"Sección (BXH) {Subtramos[i + 1].Seccion.B}x{Subtramos[i + 1].Seccion.H} L={Subtramos[i + 1].Longitud}m";
                    string Text3 = $"Sección (BXH) {Subtramos[i + 2].Seccion.B}x{Subtramos[i + 2].Seccion.H} L={Subtramos[i + 2].Longitud}m";

                    if (i == 0)
                    {
                        Tabla2.AddCell(Vcell(nervio.Nombre, BordAb: 0, Colspan: 9, IsBold: true));
                    }

                    Tabla2.AddCell(Vcell(Text1, BordeD: 0, Colspan: 3));
                    Tabla2.AddCell(Vcell(Text2, BordeD: 0, Colspan: 3));
                    Tabla2.AddCell(Vcell(Text3, Colspan: 3));

                    object[][][] DatosResumidos1 = cFunctionsProgram.EstacionesResumidas(Subtramos[i].Estaciones);
                    object[][][] DatosResumidos2 = cFunctionsProgram.EstacionesResumidas(Subtramos[i + 1].Estaciones);
                    object[][][] DatosResumidos3 = cFunctionsProgram.EstacionesResumidas(Subtramos[i + 2].Estaciones);

                    #region Parte Negativa

                    Tabla2.AddCell(Vcell($"Mu-={string.Format("{0:0.00}", DatosResumidos1[1][1][0])}", 1, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"Mu-={string.Format("{0:0.00}", DatosResumidos1[1][1][1])}", 0, 0, 0, 0));

                    Tabla2.AddCell(Vcell($"Mu-={string.Format("{0:0.00}", DatosResumidos2[1][1][0])}", 1, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"Mu-={string.Format("{0:0.00}", DatosResumidos2[1][1][1])}", 0, 0, 0, 0));

                    Tabla2.AddCell(Vcell($"Mu-={string.Format("{0:0.00}", DatosResumidos3[1][1][0])}", 1, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"Mu-={string.Format("{0:0.00}", DatosResumidos3[1][1][1])}", 0, 1, 0, 0));


                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[1][2][0])}", 1, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[1][2][1])}", 0, 0, 0, 0));

                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos2[1][2][0])}", 1, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos2[1][2][1])}", 0, 0, 0, 0));

                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos3[1][2][0])}", 1, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos3[1][2][1])}", 0, 1, 0, 0));


                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[1][0][0])}", 1, 0, 0, 0.5f));
                    Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0.5f));
                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[1][0][1])}", 0, 0, 0, 0.5f));

                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos2[1][0][0])}", 1, 0, 0, 0.5f));
                    Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0.5f));
                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos2[1][0][1])}", 0, 0, 0, 0.5f));

                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos3[1][0][0])}", 1, 0, 0, 0.5f));
                    Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0.5f));
                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos3[1][0][1])}", 0, 1, 0, 0.5f));


                    #endregion

                    #region PartePositiva
                    Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos1[0][1][0])}", 1, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos1[0][1][1])}", 0, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos1[0][1][2])}", 0, 0, 0, 0));

                    Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos2[0][1][0])}", 1, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos2[0][1][1])}", 0, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos2[0][1][2])}", 0, 0, 0, 0));

                    Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos3[0][1][0])}", 1, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos3[0][1][1])}", 0, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos3[0][1][2])}", 0, 1, 0, 0));


                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[0][2][0])}", 1, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[0][2][1])}", 0, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[0][2][2])}", 0, 0, 0, 0));

                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos2[0][2][0])}", 1, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos2[0][2][1])}", 0, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos2[0][2][2])}", 0, 0, 0, 0));

                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos3[0][2][0])}", 1, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos3[0][2][1])}", 0, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos3[0][2][2])}", 0, 1, 0, 0));


                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[0][0][0])}", 1, 0, 0, 0.5f));
                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[0][0][1])}", 0, 0, 0, 0.5f));
                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[0][0][2])}", 0, 0, 0, 0.5f));

                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos2[0][0][0])}", 1, 0, 0, 0.5f));
                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos2[0][0][1])}", 0, 0, 0, 0.5f));
                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos2[0][0][2])}", 0, 0, 0, 0.5f));

                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos3[0][0][0])}", 1, 0, 0, 0.5f));
                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos3[0][0][1])}", 0, 0, 0, 0.5f));
                    Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos3[0][0][2])}", 0, 1, 0, 0.5f));


                    #endregion

                    #region Estribos
                    Tabla2.AddCell(Vcell($"V+={string.Format("{0:0.00}", DatosResumidos1[2][0][0])}", 1, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"V-={string.Format("{0:0.00}", DatosResumidos1[2][0][1])}", 0, 0, 0, 0));

                    Tabla2.AddCell(Vcell($"V+={string.Format("{0:0.00}", DatosResumidos2[2][0][0])}", 1, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"V-={string.Format("{0:0.00}", DatosResumidos2[2][0][1])}", 0, 0, 0, 0));

                    Tabla2.AddCell(Vcell($"V+={string.Format("{0:0.00}", DatosResumidos3[2][0][0])}", 1, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0));
                    Tabla2.AddCell(Vcell($"V-={string.Format("{0:0.00}", DatosResumidos3[2][0][1])}", 0, 1, 0, 0));


                    Tabla2.AddCell(Vcell($"{DatosResumidos1[2][1][0]}", 1, 0, 0, 1));
                    Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 1));
                    Tabla2.AddCell(Vcell($"{DatosResumidos1[2][1][1]}", 0, 0, 0, 1));

                    Tabla2.AddCell(Vcell($"{DatosResumidos2[2][1][0]}", 1, 0, 0, 1));
                    Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 1));
                    Tabla2.AddCell(Vcell($"{DatosResumidos2[2][1][1]}", 0, 0, 0, 1));

                    Tabla2.AddCell(Vcell($"{DatosResumidos3[2][1][0]}", 1, 0, 0, 1));
                    Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 1));
                    Tabla2.AddCell(Vcell($"{DatosResumidos3[2][1][1]}", 0, 1, 0, 1));

                    Tabla2.AddCell(Vcell($" ", 0, 0, 0, 0, 9));
                    Tablas.Add(Tabla2);
                    #endregion
                    Contador += 1;
                }

                int Restante = Cantidad - 3 * Contador;

                if (Restante == 2)
                {
                    ParaDosSubTramos(ref Tablas, Subtramos[Restante - 2], Subtramos.Last());
                }else if (Restante==1)
                {
                    ParaUnTramo(ref Tablas, Subtramos.Last());
                }
            }
            else if(Cantidad==2)
            {
                ParaDosSubTramos(ref Tablas, Subtramos.First(), Subtramos.Last(),nervio);

            }else if (Cantidad==1)
            {
                ParaUnTramo(ref Tablas, Subtramos.First(),nervio);
            }


            return Tablas;
        }

        private static PdfPCell Vcell(string Text, float BordeI = 1, float BordeD = 1, float BordeAr = 1, float BordAb = 1,int Colspan=1,int Rowspan=1,bool IsBold=false)
        {
            int Style = Font.NORMAL;
            if (IsBold)
                Style = Font.BOLD;

            Font font1 = RegistrerFont("Calibri", 8, Style, BaseColor.BLACK);
            PdfPCell cell = new PdfPCell(new Phrase(Text, font1)); cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BorderWidthLeft = BordeI;
            cell.BorderWidthRight = BordeD;
            cell.BorderWidthTop = BordeAr;
            cell.BorderWidthBottom = BordAb;
            cell.Colspan = Colspan;
            cell.Rowspan = Rowspan;
            return cell;
        }



        private static void ParaDosSubTramos(ref List<PdfPTable> Tablas, cSubTramo S1, cSubTramo S2,cNervio nervio=null)
        {

            PdfPTable Tabla2 = new PdfPTable(9);
            
            if (nervio != null)
            {
                Tabla2.AddCell(Vcell(nervio.Nombre, BordAb: 0, Colspan: 6, IsBold: true)); 
                Tabla2.AddCell(Vcell("", 0, 0, 0, 0, 3));
            }
            string Text1 = $"Sección (BXH) {S1.Seccion.B}x{S1.Seccion.H} L={S1.Longitud}m";
            string Text2 = $"Sección (BXH) {S2.Seccion.B}x{S2.Seccion.H} L={S2.Longitud}m";


            Tabla2.AddCell(Vcell(Text1, BordeD: 0, Colspan: 3));
            Tabla2.AddCell(Vcell(Text2, Colspan: 3));
            Tabla2.AddCell(Vcell("", 0, 0, 0, 0, 3, 9));

            object[][][] DatosResumidos1 = cFunctionsProgram.EstacionesResumidas(S1.Estaciones);
            object[][][] DatosResumidos2 = cFunctionsProgram.EstacionesResumidas(S2.Estaciones);

            #region Parte Negativa

            Tabla2.AddCell(Vcell($"Mu-={string.Format("{0:0.00}", DatosResumidos1[1][1][0])}", 1, 0, 0, 0));
            Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0));
            Tabla2.AddCell(Vcell($"Mu-={string.Format("{0:0.00}", DatosResumidos1[1][1][1])}", 0, 0, 0, 0));

            Tabla2.AddCell(Vcell($"Mu-={string.Format("{0:0.00}", DatosResumidos2[1][1][0])}", 1, 0, 0, 0));
            Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0));
            Tabla2.AddCell(Vcell($"Mu-={string.Format("{0:0.00}", DatosResumidos2[1][1][1])}", 0, 1, 0, 0));


            Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[1][2][0])}", 1, 0, 0, 0));
            Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0));
            Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[1][2][1])}", 0, 0, 0, 0));

            Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos2[1][2][0])}", 1, 0, 0, 0));
            Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0));
            Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos2[1][2][1])}", 0, 1, 0, 0));


            Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[1][0][0])}", 1, 0, 0, 0.5f));
            Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0.5f));
            Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[1][0][1])}", 0, 0, 0, 0.5f));

            Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos2[1][0][0])}", 1, 0, 0, 0.5f));
            Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0.5f));
            Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos2[1][0][1])}", 0, 1, 0, 0.5f));

            #endregion

            #region PartePositiva
            Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos1[0][1][0])}", 1, 0, 0, 0));
            Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos1[0][1][1])}", 0, 0, 0, 0));
            Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos1[0][1][2])}", 0, 0, 0, 0));

            Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos2[0][1][0])}", 1, 0, 0, 0));
            Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos2[0][1][1])}", 0, 0, 0, 0));
            Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos2[0][1][2])}", 0, 1, 0, 0));


            Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[0][2][0])}", 1, 0, 0, 0));
            Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[0][2][1])}", 0, 0, 0, 0));
            Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[0][2][2])}", 0, 0, 0, 0));

            Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos2[0][2][0])}", 1, 0, 0, 0));
            Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos2[0][2][1])}", 0, 0, 0, 0));
            Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos2[0][2][2])}", 0, 1, 0, 0));


            Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[0][0][0])}", 1, 0, 0, 0.5f));
            Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[0][0][1])}", 0, 0, 0, 0.5f));
            Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[0][0][2])}", 0, 0, 0, 0.5f));

            Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos2[0][0][0])}", 1, 0, 0, 0.5f));
            Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos2[0][0][1])}", 0, 0, 0, 0.5f));
            Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos2[0][0][2])}", 0, 1, 0, 0.5f));

            #endregion

            #region Estribos
            Tabla2.AddCell(Vcell($"V+={string.Format("{0:0.00}", DatosResumidos1[2][0][0])}", 1, 0, 0, 0));
            Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0));
            Tabla2.AddCell(Vcell($"V-={string.Format("{0:0.00}", DatosResumidos1[2][0][1])}", 0, 0, 0, 0));

            Tabla2.AddCell(Vcell($"V+={string.Format("{0:0.00}", DatosResumidos2[2][0][0])}", 1, 0, 0, 0));
            Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0));
            Tabla2.AddCell(Vcell($"V-={string.Format("{0:0.00}", DatosResumidos2[2][0][1])}", 0, 1, 0, 0));

      
            Tabla2.AddCell(Vcell($"{DatosResumidos1[2][1][0]}", 1, 0, 0, 1));
            Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 1));
            Tabla2.AddCell(Vcell($"{DatosResumidos1[2][1][1]}", 0, 0, 0, 1));

            Tabla2.AddCell(Vcell($"{DatosResumidos2[2][1][0]}", 1, 0, 0, 1));
            Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 1));
            Tabla2.AddCell(Vcell($"{DatosResumidos2[2][1][1]}", 0, 1, 0, 1));


            Tabla2.AddCell(Vcell($" ", 0, 0, 0, 0,9));
            Tablas.Add(Tabla2);
            #endregion




        }

        private static void ParaUnTramo(ref List<PdfPTable> Tablas, cSubTramo S1, cNervio nervio = null)
        {
            PdfPTable Tabla2 = new PdfPTable(9);
            string Text1 = $"Sección (BXH) {S1.Seccion.B}x{S1.Seccion.H} L={S1.Longitud}m";

            if (nervio != null)
            {
                Tabla2.AddCell(Vcell(nervio.Nombre, BordAb: 0, Colspan: 3,IsBold:true));
                Tabla2.AddCell(Vcell("", 0, 0, 0, 0, 6));
            }
            Tabla2.AddCell(Vcell(Text1, Colspan: 3));
            Tabla2.AddCell(Vcell("", 0, 0, 0, 0, 6, 9));

            object[][][] DatosResumidos1 = cFunctionsProgram.EstacionesResumidas(S1.Estaciones);

            #region Parte Negativa

            Tabla2.AddCell(Vcell($"Mu-={string.Format("{0:0.00}", DatosResumidos1[1][1][0])}", 1, 0, 0, 0));
            Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0));
            Tabla2.AddCell(Vcell($"Mu-={string.Format("{0:0.00}", DatosResumidos1[1][1][1])}", 0, 1, 0, 0));

            Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[1][2][0])}", 1, 0, 0, 0));
            Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0));
            Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[1][2][1])}", 0, 1, 0, 0));

            Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[1][0][0])}", 1, 0, 0, 0.5f));
            Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0.5f));
            Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[1][0][1])}", 0, 1, 0, 0.5f));

            #endregion

            #region PartePositiva
            Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos1[0][1][0])}", 1, 0, 0, 0));
            Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos1[0][1][1])}", 0, 0, 0, 0));
            Tabla2.AddCell(Vcell($"Mu+={string.Format("{0:0.00}", DatosResumidos1[0][1][2])}", 0, 1, 0, 0));

            Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[0][2][0])}", 1, 0, 0, 0));
            Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[0][2][1])}", 0, 0, 0, 0));
            Tabla2.AddCell(Vcell($"Asr={string.Format("{0:0.00}", DatosResumidos1[0][2][2])}", 0, 1, 0, 0));

            Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[0][0][0])}", 1, 0, 0, 0.5f));
            Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[0][0][1])}", 0, 0, 0, 0.5f));
            Tabla2.AddCell(Vcell($"As={string.Format("{0:0.00}", DatosResumidos1[0][0][2])}", 0, 1, 0, 0.5f));
            #endregion

            #region Estribos
            Tabla2.AddCell(Vcell($"V+={string.Format("{0:0.00}", DatosResumidos1[2][0][0])}", 1, 0, 0, 0));
            Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 0));
            Tabla2.AddCell(Vcell($"V-={string.Format("{0:0.00}", DatosResumidos1[2][0][1])}", 0, 1, 0, 0));


            Tabla2.AddCell(Vcell($"{DatosResumidos1[2][1][0]}", 1, 0, 0, 1));
            Tabla2.AddCell(Vcell($"  ", 0, 0, 0, 1));
            Tabla2.AddCell(Vcell($"{DatosResumidos1[2][1][1]}", 0, 1, 0, 1));


            Tabla2.AddCell(Vcell($" ", 0, 0, 0, 0, 9));
            Tablas.Add(Tabla2);
            #endregion
        }






    }

}
