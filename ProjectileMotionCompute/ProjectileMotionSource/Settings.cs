using System;
using Utilities.Files;
using System.IO;
using System.Drawing;

namespace ProjectileMotionSource.Func
{
    /// <summary>
    /// Settings of projectile motion.
    /// </summary>
    public class ProjectileMotionSettings
    {
        private void SetDefaults()
        {
            RoundDigits = 6;
            PathToFiles = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            PointsForTrajectory = 150;
            TxtInfoFileName = GetDefaultFileName("txt");
            CsvDataFileName = GetDefaultFileName("csv");
            PdfInfoFileName = GetDefaultFileName("pdf");
            HexColorOfTrajectory = "#6c757d";
        }


        /// <summary>
        /// Constructor for projectile motion settings.
        /// </summary>
        /// <param name="quantities">Quantities which can also be computed and their units.</param>
        public ProjectileMotionSettings(ProjectileMotionQuantities quantities)
        {
            Quantities = quantities ?? throw new ArgumentNullException(nameof(quantities), "Quantities object cannot be null.");
            SetDefaults();
        }

        /// <summary>
        /// Projectile motion Quantities.
        /// </summary>
        public ProjectileMotionQuantities Quantities { get; private set; }

        private int _RoundDigits { get; set; }

        /// <summary>
        /// The number of decimal places to round to.
        /// </summary>
        public int RoundDigits {
            get { return _RoundDigits; }
            set {
                if (value < 0)
                {
                    throw new Exception("The number of decimal places to round to cannot be smaller than zero");
                }
                _RoundDigits = value;
            }
        }

        private int _PointsForTrajectory { get; set; }

        protected bool CheckValidPointsForTrajectoryWithException (int val)
        {
            if (val < 0)
            {
                throw new Exception("The number of points to use to draw the trajectory cannot be smaller than zero");
            }

            return true;
        }

        /// <summary>
        /// The number of points to use to draw trajectory.
        /// </summary>
        public virtual int PointsForTrajectory
        {
            get { return _PointsForTrajectory; }
            set
            {
                CheckValidPointsForTrajectoryWithException(value);
                _PointsForTrajectory = value;
            }
        }


        protected string GetDefaultFileName(string extension)
        {
            return string.Format(
                "ProjectileMotion-{0} {1}-{2} {3}-{4} {5}.{6}",
                Quantities.Α.GetRoundedVal(RoundDigits).ToString(),
                Quantities.Α.Unit.Name,
                Quantities.V.GetRoundedVal(RoundDigits).ToString(),
                Quantities.V.Unit.Name,
                Quantities.H.GetRoundedVal(RoundDigits).ToString(),
                Quantities.H.Unit.Name,
                extension
            );
        }


        private bool ShouldBeSetDefaultName (string name) {
            return name == null || name?.Length == 0;
        }


        private string _TxtInfoFileName { get; set; }

        /// <summary>
        /// The file in which is basic information about projectile motion saved.
        /// </summary>
        public string TxtInfoFileName
        {
            get
            {
                return _TxtInfoFileName;
            }
            set
            {
                _TxtInfoFileName = ShouldBeSetDefaultName(value) ? GetDefaultFileName("txt") : FileUtilities.GetFileNameWithExtension(value, "txt");
            }
        }


        private string _CsvDataFileName { get; set; }

        /// <summary>
        /// The file in which are data saved (excel).
        /// </summary>
        public string CsvDataFileName
        {
            get
            {
                return _CsvDataFileName;
            }
            set
            {
                _CsvDataFileName = ShouldBeSetDefaultName(value) ? GetDefaultFileName("csv") : FileUtilities.GetFileNameWithExtension(value, "csv");
            }
        }


        private string _HexColorOfTrajectory { get; set; }

        /// <summary>
        /// The color of the trajectory (hex format)
        /// </summary>
        public string HexColorOfTrajectory
        {
            get
            {
                return _HexColorOfTrajectory;
            }
            set
            {
                try
                {
                    if (!ColorTranslator.FromHtml(value).IsEmpty)
                    {
                        _HexColorOfTrajectory = value;
                    }
                    else throw new Exception();
                }
                catch (Exception) {
                    _HexColorOfTrajectory = "#6c757d";
                }
            }
        }

        private string _PdfDataFileName { get; set; }

        /// <summary>
        /// The file in which is information saved to be printed.
        /// </summary>
        public string PdfInfoFileName
        {
            get
            {
                return _PdfDataFileName;
            }
            set
            {
                _PdfDataFileName = ShouldBeSetDefaultName(value) ? GetDefaultFileName("pdf") : FileUtilities.GetFileNameWithExtension(value, "pdf");
            }
        }


        /// <summary>
        /// Gets the name for chart to be exported with the specified extension.
        /// </summary>
        public virtual string GetChartFileNameForExport (string extension)
        {
            return "chart-" + GetDefaultFileName(extension);
        }


        private string _PathToFiles { get; set; }

        /// <summary>
        /// The folder path in which are files saved or exported.
        /// </summary>
        public string PathToFiles
        {
            get
            {
                return _PathToFiles;
            }
            set
            {
                _PathToFiles = FileUtilities.EndPathWithSlash(value);
                DirectoryInfo dir = new DirectoryInfo(_PathToFiles);
                if (!dir.Exists)
                {
                    if (dir.Parent.Exists)
                    {
                        Directory.CreateDirectory(_PathToFiles);
                    }
                    else
                        throw new DirectoryNotFoundException("Unable to find the folder by this path.");
                }
            }
        }
    }
}