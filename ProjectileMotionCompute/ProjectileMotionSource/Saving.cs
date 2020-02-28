using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using ProjectileMotionSource.WithResistance.Func;
using ProjectileMotionSource.Func;
using Utilities.Quantities;

namespace ProjectileMotionSource.Saving
{
    /// <summary>
    /// Saving to files and exportation.
    /// </summary>
    public class ProjectileMotionFilesSaving
    {
        public ProjectileMotionFilesSaving(ProjectileMotion motion)
        {
            Motion = motion;
        }


        private void ConstructNewDataRecordQuantity (string quantityName, Quantity quantity)
        {
            DataToFiles.Add(quantityName, quantity.GetRoundedVal(Motion.Settings.RoundDigits).ToString(CultureInfo.InvariantCulture));
        }

        private string ConstructQuantityDescription (string quantityName, string quantityUnit)
        {
            return string.Format("{0} (in {1}s)", quantityName, quantityUnit);
        }

        private void ConstructNewDataRecordQuantity(string quantityName, QuantityWithUnit quantityWithUnit)
        {
            DataToFiles.Add(ConstructQuantityDescription(quantityName, quantityWithUnit.Unit.Name), quantityWithUnit.GetRoundedVal(Motion.Settings.RoundDigits).ToString(CultureInfo.InvariantCulture));
        }


        private Dictionary<string, string> DataToFiles = new Dictionary<string, string>();

        public Dictionary<string, string> GetDataToFiles()
        {
            DataToFiles.Clear();

            if (!(Motion is ProjectileMotionWithResistance))
            {
                DataToFiles.Add("The assignment type", ProjectileMotionQuantities.AssignmentsTypesTranslations[Motion.Settings.Quantities.UsedAssignmentType]);
            }

            ConstructNewDataRecordQuantity("The elevation angle", Motion.Settings.Quantities.Α);
            ConstructNewDataRecordQuantity("The initial velocity", Motion.Settings.Quantities.V);
            ConstructNewDataRecordQuantity("The initial height", Motion.Settings.Quantities.H);
            ConstructNewDataRecordQuantity("The initial gravitation acceleration", Motion.Settings.Quantities.G);

            if (Motion is ProjectileMotionWithResistance)
            {
                ProjectileMotionWithResistanceQuantities resistanceQuantities = (ProjectileMotionWithResistanceQuantities)Motion.Settings.Quantities;

                ConstructNewDataRecordQuantity("The mass", resistanceQuantities.M);
                ConstructNewDataRecordQuantity("The density", resistanceQuantities.Ρ);
                ConstructNewDataRecordQuantity("The frontal area", resistanceQuantities.A);
                ConstructNewDataRecordQuantity("The drag coefficient", resistanceQuantities.C);
            }

            ConstructNewDataRecordQuantity("The duration", Motion.GetDur());
            ConstructNewDataRecordQuantity("The length", Motion.GetLength());
            ConstructNewDataRecordQuantity("The arc length", Motion.GetArcLength());
            ConstructNewDataRecordQuantity("The area under arc", Motion.GetAreaUnderArc());
            ConstructNewDataRecordQuantity("The max distance from the beginning", Motion.GetMaxDistance());
            ConstructNewDataRecordQuantity("The max height", Motion.GetMaxHeight());

            DataToFiles.Add("Coordinates of the farthest point from the beginning (in " + Motion.Settings.Quantities.Units.Length.Name + "s)", "[" + Motion.GetCoordsFarthest()[0].ToString(CultureInfo.InvariantCulture) + ", " + Motion.GetCoordsFarthest()[1].ToString(CultureInfo.InvariantCulture) + "]");
            DataToFiles.Add("Coordinates of the highest point (in " + Motion.Settings.Quantities.Units.Length.Name + "s)", "[" + Motion.GetCoordsHighest()[0].ToString(CultureInfo.InvariantCulture) + ", " + Motion.GetCoordsHighest()[1].ToString(CultureInfo.InvariantCulture) + "]");

            if (Motion is ProjectileMotionWithResistance)
            {
                ConstructNewDataRecordQuantity("The time of the highest point", Motion.GetTimeHighest());
                ConstructNewDataRecordQuantity("The time of the farthest point", Motion.GetTimeFarthest());
            }

            return DataToFiles;
        }

        private ProjectileMotion Motion { get; set; }


        public string GetChartCategoryTitle()
        {
            return ConstructQuantityDescription("Distance", Motion.Settings.Quantities.Units.Length.Name);
        }

        public string GetChartValueTitle()
        {
            return ConstructQuantityDescription("Height", Motion.Settings.Quantities.Units.Length.Name);
        }

        private string GetTrajectoryTitle()
        {
            return ConstructQuantityDescription("The trajectory", Motion.Settings.Quantities.Units.Length.Name);
        }

        private string SuccessfullySavedMessage(string fileName)
        {
            return string.Format("The file {0} has been successfully saved to {1}", fileName, Motion.Settings.PathToFiles);
        }


        private delegate void ActionWithWriter(StreamWriter writer);

        private MemoryStream GetMemoryStreamFromStreamWriter (ActionWithWriter action)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(ms, Encoding.UTF8))
                {
                    action(writer);
                    writer.Flush();
                }

                return ms;
            }
        }

        public MemoryStream InfoToTxtGetMemoryStream()
        {
            return GetMemoryStreamFromStreamWriter(
                writer =>
                {
                    foreach (KeyValuePair<string, string> dataPair in GetDataToFiles())
                    {
                        writer.WriteLine(dataPair.Key);
                        writer.WriteLine(dataPair.Value);
                        writer.WriteLine();
                    }


                    writer.WriteLine(GetTrajectoryTitle());
                    foreach (double[] coords in Motion.GetTrajectory())
                    {
                        writer.WriteLine("( " + coords[0].ToString(CultureInfo.InvariantCulture) + ", " + coords[1].ToString(CultureInfo.InvariantCulture) + " )");
                    }
                }
            );
        }

        private string FormatForCsv(string[] values)
        {
            return string.Join(";", values);
        }

        public MemoryStream DataToCsvGetMemoryStream ()
        {
            return GetMemoryStreamFromStreamWriter(
                writer =>
                {
                    foreach (KeyValuePair<string, string> pair in GetDataToFiles())
                    {
                        writer.WriteLine(FormatForCsv(new string[] { pair.Key, pair.Value }));
                    }

                    writer.WriteLine();

                    writer.WriteLine(GetTrajectoryTitle());
                    writer.WriteLine(FormatForCsv(new string[] { GetChartCategoryTitle(), GetChartValueTitle() }));
                    foreach (double[] coords in Motion.GetTrajectory())
                    {
                        writer.WriteLine(FormatForCsv(new string[] { coords[0].ToString(), coords[1].ToString() }));
                    }
                }
            );
        }

        public delegate void ActionWithPdfDocument (PdfDocument document, PdfPage page);

        public static MemoryStream GetMemoryStreamFromPdfDocument (ActionWithPdfDocument action)
        {
            using (PdfDocument document = new PdfDocument())
            {
                PdfPage page = document.AddPage();

                action(document, page);

                using (MemoryStream pdfStream = new MemoryStream())
                {
                    document.Save(pdfStream);
                    pdfStream.Position = 0;
                    return pdfStream;
                }
            };
        }


        public MemoryStream InfoToPdfGetMemoryStream()
        {
            return GetMemoryStreamFromPdfDocument(
                ( document, page ) =>
                {
                    page.Orientation = PageOrientation.Landscape;
                    page.TrimMargins = new TrimMargins()
                    {
                        All = new XUnit(2, XGraphicsUnit.Centimeter)
                    };

                    using (XGraphics gfx = XGraphics.FromPdfPage(page, XPageDirection.Downwards))
                    {
                        XFont basic = new XFont("Calibri", 14, XFontStyle.Regular);
                        XFont bold = new XFont("Calibri", 14, XFontStyle.Bold);

                        int i = 0;
                        foreach (KeyValuePair<string, string> info in GetDataToFiles())
                        {
                            string lineKey = info.Key + ":";

                            gfx.DrawString(lineKey,
                                basic,
                                XBrushes.Black,
                                new XRect(0, i * gfx.MeasureString(lineKey, basic).Height, page.Width, page.Height),
                                XStringFormats.TopLeft
                            );

                            gfx.DrawString(info.Value, bold,
                                XBrushes.Black,
                                new XRect(gfx.MeasureString(lineKey, bold).Width + 5, i++ * gfx.MeasureString(lineKey, bold).Height, page.Width, page.Height),
                                XStringFormats.TopLeft
                            );

                            gfx.DrawString("", basic,
                                XBrushes.Black,
                                new XRect(0, i++ * gfx.MeasureString(lineKey, basic).Height, page.Width, page.Height),
                                XStringFormats.TopLeft
                            );
                        }
                    }
                }
            );
        }


        public void InfoToTxt()
        {
            File.WriteAllBytes(Motion.Settings.PathToFiles + Motion.Settings.TxtInfoFileName, InfoToTxtGetMemoryStream().ToArray());
            Console.WriteLine(SuccessfullySavedMessage(Motion.Settings.TxtInfoFileName));
        }


        public void DataToCsv()
        {
            File.WriteAllBytes(Motion.Settings.PathToFiles + Motion.Settings.CsvDataFileName, DataToCsvGetMemoryStream().ToArray());
            Console.WriteLine(SuccessfullySavedMessage(Motion.Settings.CsvDataFileName));
        }


        public void InfoToPdf()
        {
            File.WriteAllBytes(Motion.Settings.PathToFiles + Motion.Settings.PdfInfoFileName, InfoToPdfGetMemoryStream().ToArray());
            Console.WriteLine(SuccessfullySavedMessage(Motion.Settings.PdfInfoFileName));
        }
    }
}