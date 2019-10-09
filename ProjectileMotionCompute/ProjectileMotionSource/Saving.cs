using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using ProjectileMotionSource.WithRezistance.Func;
using ProjectileMotionSource.Func;

namespace ProjectileMotionSource.Saving
{
    public class ProjectileMotionFilesSaving
    {
        public ProjectileMotionFilesSaving(ProjectileMotion motion)
        {
            Motion = motion;
        }

        public Dictionary<string, string> GetDataToFiles()
        {
            Dictionary<string, string> data = new Dictionary<string, string>() { };

            if (!(Motion is ProjectileMotionWithRezistance))
            {
                data.Add("Assignment type", ProjectileMotionQuantities.AssignmentsTypesTranslations[Motion.Settings.Quantities.UsedAssignmentType]);
            }

            data.Add("An elevation angle (in " + Motion.Settings.Quantities.Α.Unit.Name + "s)", Motion.Settings.Quantities.Α.GetRoundedVal(Motion.Settings.RoundDigits).ToString(CultureInfo.InvariantCulture));
            data.Add("An initial velocity (in " + Motion.Settings.Quantities.V.Unit.Name + ")", Motion.Settings.Quantities.V.GetRoundedVal(Motion.Settings.RoundDigits).ToString(CultureInfo.InvariantCulture));
            data.Add("An initial height (in " + Motion.Settings.Quantities.H.Unit.Name + ")", Motion.Settings.Quantities.H.GetRoundedVal(Motion.Settings.RoundDigits).ToString(CultureInfo.InvariantCulture));
            data.Add("An initial gravitation acceleration (in " + Motion.Settings.Quantities.G.Unit.Name + ")", Motion.Settings.Quantities.G.GetRoundedVal(Motion.Settings.RoundDigits).ToString(CultureInfo.InvariantCulture));

            if (Motion is ProjectileMotionWithRezistance)
            {
                ProjectileMotionWithRezistanceQuantities rezistanceQuantities = (ProjectileMotionWithRezistanceQuantities)Motion.Settings.Quantities;

                data.Add("The mass (in " + rezistanceQuantities.M.Unit.Name + "s)", rezistanceQuantities.M.GetRoundedVal(Motion.Settings.RoundDigits).ToString(CultureInfo.InvariantCulture));
                data.Add("The density (in " + rezistanceQuantities.Ρ.Unit.Name + "s)", rezistanceQuantities.Ρ.GetRoundedVal(Motion.Settings.RoundDigits).ToString(CultureInfo.InvariantCulture));
                data.Add("An area (in " + rezistanceQuantities.A.Unit.Name + "s)", rezistanceQuantities.A.GetRoundedVal(Motion.Settings.RoundDigits).ToString(CultureInfo.InvariantCulture));
                data.Add("The drag coefficient", rezistanceQuantities.C.GetRoundedVal(Motion.Settings.RoundDigits).ToString(CultureInfo.InvariantCulture));
            }

            data.Add("Duration (in " + Motion.Settings.Quantities.Units.Time.Name + "s)", Motion.GetDur().Val.ToString(CultureInfo.InvariantCulture));
            data.Add("Length (in " + Motion.Settings.Quantities.Units.Length.Name + "s)", Motion.GetLength().Val.ToString(CultureInfo.InvariantCulture));
            data.Add("An arc Length (in " + Motion.Settings.Quantities.Units.Length.Name + "s)", Motion.GetArcLength().Val.ToString(CultureInfo.InvariantCulture));
            data.Add("An area under arc (in " + Motion.Settings.Quantities.Units.Area.Name + "s)", Motion.GetAreaUnderArc().Val.ToString(CultureInfo.InvariantCulture));
            data.Add("Max distance from the beginning (in " + Motion.Settings.Quantities.Units.Length.Name + "s)", Motion.GetMaxDistance().Val.ToString(CultureInfo.InvariantCulture));
            data.Add("Max height (in " + Motion.Settings.Quantities.Units.Length.Name + "s)", Motion.GetMaxHeight().Val.ToString(CultureInfo.InvariantCulture));

            data.Add("Coordinates of the farthest point from the beginning (in " + Motion.Settings.Quantities.Units.Length.Name + "s)", "[" + Motion.GetCoordsFarthest()[0].ToString(CultureInfo.InvariantCulture) + ", " + Motion.GetCoordsFarthest()[1].ToString(CultureInfo.InvariantCulture) + "]");
            data.Add("Coordinates of the highest point (in " + Motion.Settings.Quantities.Units.Length.Name + "s)", "[" + Motion.GetCoordsHighest()[0].ToString(CultureInfo.InvariantCulture) + ", " + Motion.GetCoordsHighest()[1].ToString(CultureInfo.InvariantCulture) + "]");


            if (Motion is ProjectileMotionWithRezistance)
            {
                data.Add("The time of the highest point (in " + Motion.Settings.Quantities.Units.Time.Name + "s)", Motion.GetTimeHighest().Val.ToString(CultureInfo.InvariantCulture));
                data.Add("The time of the farthest point (in " + Motion.Settings.Quantities.Units.Time.Name + "s)", Motion.GetTimeFarthest().Val.ToString(CultureInfo.InvariantCulture));
            }

            return data;
        }

        private ProjectileMotion Motion { get; set; }


        public string GetChartCategoryTitle()
        {
            return "Distance (in " + Motion.Settings.Quantities.Units.Length.Name + "s)";
        }

        public string GetChartValueTitle()
        {
            return "Height (in " + Motion.Settings.Quantities.Units.Length.Name + "s) ";
        }

        private string GetFunctionCourseTitle()
        {
            return "Function course (in " + Motion.Settings.Quantities.Units.Length.Name + "s)";
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


                    writer.WriteLine("Function course (in " + Motion.Settings.Quantities.Units.Length.Name + "s)");
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

                    writer.WriteLine(GetFunctionCourseTitle());
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