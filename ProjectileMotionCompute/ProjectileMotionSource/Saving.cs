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
using ProjectileMotionSource.Point;

namespace ProjectileMotionSource.Saving
{
    /// <summary>
    /// Saving to files and exportation.
    /// </summary>
    public class ProjectileMotionFilesSaving
    {
        internal ProjectileMotionFilesSaving(ProjectileMotion motion)
        {
            Motion = motion;
        }

        private void ConstructNewDataRecordQuantity (string quantityName, Quantity quantity)
        {
            DataToFiles.Add(quantityName, quantity.GetRoundedVal(Motion.Settings.RoundDigits).ToString(CultureInfo.InvariantCulture));
        }

        private string ConstructQuantityDescription (string quantityName, string quantityUnit)
        {
            return string.Format("{0} ({1})", quantityName, quantityUnit);
        }

        private void ConstructNewDataRecordQuantity(string quantityName, QuantityWithUnit quantityWithUnit)
        {
            DataToFiles.Add(ConstructQuantityDescription(quantityName, quantityWithUnit.Unit.Name), quantityWithUnit.GetRoundedVal(Motion.Settings.RoundDigits).ToString(CultureInfo.InvariantCulture));
        }

        private string CounstructCoordsFormatted(ProjectileMotionPoint p, string format = "( {0}, {1} )")
        {
            return string.Format(format, p.X.Convert(Motion.Settings.Quantities.Units.Length).GetRoundedVal(Motion.Settings.RoundDigits).ToString(CultureInfo.InvariantCulture), p.Y.Convert(Motion.Settings.Quantities.Units.Length).GetRoundedVal(Motion.Settings.RoundDigits).ToString(CultureInfo.InvariantCulture));
        }

        private readonly Dictionary<string, string> DataToFiles = new Dictionary<string, string>();

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

            ConstructNewDataRecordQuantity("The duration", Motion.GetDur().Convert(Motion.Settings.Quantities.Units.Time));
            ConstructNewDataRecordQuantity("The length", Motion.GetLength().Convert(Motion.Settings.Quantities.Units.Length));
            ConstructNewDataRecordQuantity("The arc length", Motion.Trajectory.GetArcLength().Convert(Motion.Settings.Quantities.Units.Area));
            ConstructNewDataRecordQuantity("The area under arc", Motion.Trajectory.GetAreaUnderArc().Convert(Motion.Settings.Quantities.Units.Area));
            ConstructNewDataRecordQuantity("The max distance from the beginning", Motion.GetMaxDistance().Convert(Motion.Settings.Quantities.Units.Length));
            ConstructNewDataRecordQuantity("The max height", Motion.GetMaxHeight().Convert(Motion.Settings.Quantities.Units.Length));

            DataToFiles.Add(ConstructQuantityDescription("Coordinates of the farthest point from the beginning", Motion.Settings.Quantities.Units.Length.Name), CounstructCoordsFormatted(Motion.Trajectory.GetFarthestPoint()));
            DataToFiles.Add(ConstructQuantityDescription("Coordinates of the highest point from the beginning", Motion.Settings.Quantities.Units.Length.Name), CounstructCoordsFormatted(Motion.Trajectory.GetHighestPoint()));

            if (Motion is ProjectileMotionWithResistance)
            {
                ConstructNewDataRecordQuantity("The time of the highest point", Motion.GetTimeHighest().Convert(Motion.Settings.Quantities.Units.Time));
                ConstructNewDataRecordQuantity("The time of the farthest point", Motion.GetTimeFarthest().Convert(Motion.Settings.Quantities.Units.Time));
            }

            return DataToFiles;
        }

        private ProjectileMotion Motion { get; set; }

        public string GetChartCategoryTitle()
        {
            return ConstructQuantityDescription("The length", Motion.Settings.Quantities.Units.Length.Name);
        }

        public string GetChartValueTitle()
        {
            return ConstructQuantityDescription("The height", Motion.Settings.Quantities.Units.Length.Name);
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
                    foreach (ProjectileMotionPoint p in Motion.Trajectory.GetPointsList())
                    {
                        writer.WriteLine(CounstructCoordsFormatted(p));
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
                    foreach (ProjectileMotionPoint p in Motion.Trajectory.GetPointsList())
                    {
                        writer.WriteLine(CounstructCoordsFormatted(p, "{0};{1}"));
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