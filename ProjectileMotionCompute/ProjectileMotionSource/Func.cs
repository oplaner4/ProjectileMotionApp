using System;
using Utilities.Quantities;
using Utilities.Units;
using Utilities.Files;
using MathNet.Numerics;
using System.IO;
using System.Collections.Generic;
using ProjectileMotionSource.Exceptions;
using System.Linq;
using ProjectileMotionSource.Saving;
using EquationSolver = Solver1D.Solver1D;
using ProjectileMotionSource.Point;
using System.Drawing;

namespace ProjectileMotionSource.Func
{
    /// <summary>
    /// Projectile motion Quantities.
    /// </summary>
    public class ProjectileMotionQuantities
    {
        /// <summary>
        /// 1. Constructor for a projectile motion Quantities.
        /// </summary>
        /// <param name="v">An initial velocity.</param>
        /// <param name="α">An elevation angle.</param>
        /// <param name="h">An initial height.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(InitialVelocity v, ElevationAngle α, InitialHeight h, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            V = v;
            Α = α;
            H = h;
            G = g;

            CheckData();
            UsedAssignmentType = AssignmentsTypes.Basic;
        }

        /// <summary>
        /// 2. Constructor for a projectile motion Quantities. Computes an elevation angle based on the duration.
        /// </summary>
        /// <param name="v">An initial velocity.</param>
        /// <param name="dur">The duration.</param>
        /// <param name="h">An initial height.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(Duration dur, InitialVelocity v, InitialHeight h, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            V = v;
            H = h;
            G = g;
            Α = new ElevationAngle(Math.Asin((Math.Pow(dur.GetBasicVal(), 2.0) * G.GetBasicVal() - 2 * H.GetBasicVal()) / (2.0 * V.GetBasicVal() * dur.GetBasicVal())), UnitAngle.Basic).Convert(Units.Angle);
            CheckData();
            UsedAssignmentType = AssignmentsTypes.ElevationAngleByDuration;
        }

        /// <summary>
        /// 3. Constructor for a projectile motion Quantities. Computes an initial velocity based on the duration.
        /// </summary>
        /// <param name="α">An elevation angle.</param>
        /// <param name="dur">The duration.</param>
        /// <param name="h">An initial height.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(Duration dur, ElevationAngle α, InitialHeight h, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            Α = α;
            H = h;
            G = g;
            V = new InitialVelocity((Math.Pow(dur.GetBasicVal(), 2.0) * G.GetBasicVal() - 2 * H.GetBasicVal()) / (2.0 * Math.Sin(Α.GetBasicVal()) * dur.GetBasicVal()), UnitVelocity.Basic).Convert(Units.Velocity);

            CheckData();
            UsedAssignmentType = AssignmentsTypes.InitialVelocityByDuration;
        }

        /// <summary>
        /// 4. Constructor for a projectile motion Quantities. Computes an initial height based on the duration.
        /// </summary>
        /// <param name="α">An elevation angle.</param>
        /// <param name="dur">The duration.</param>
        /// <param name="v">An initial velocity.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(Duration dur, ElevationAngle α, InitialVelocity v, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            V = v;
            Α = α;
            G = g;
            H = new InitialHeight((Math.Pow(dur.GetBasicVal(), 2.0) * G.GetBasicVal() - 2.0 * Math.Sin(Α.GetBasicVal()) * dur.GetBasicVal() * V.GetBasicVal()) / 2.0, UnitLength.Basic).Convert(Units.Length);

            CheckData();
            UsedAssignmentType = AssignmentsTypes.InitialHeightByDuration;
        }

        /// <summary>
        /// 5. Constructor for a projectile motion Quantities. Computes an initial height based on the length.
        /// </summary>
        /// <param name="α">An elevation angle.</param>
        /// <param name="length">The length.</param>
        /// <param name="v">An initial velocity.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(Length length, ElevationAngle α, InitialVelocity v, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            V = v;
            Α = α;
            G = g;
            H = new InitialHeight((Math.Pow(length.GetBasicVal(), 2.0) * G.GetBasicVal() * 1 / Math.Pow(Math.Cos(Α.GetBasicVal()), 2.0) - 2.0 * length.GetBasicVal() * Math.Pow(V.GetBasicVal(), 2.0) * Math.Tan(Α.GetBasicVal())) / (2.0 * Math.Pow(V.GetBasicVal(), 2.0)), UnitLength.Basic).Convert(Units.Length);


            CheckData();
            UsedAssignmentType = AssignmentsTypes.InitialHeightByLength;
        }

        /// <summary>
        /// 6. Constructor for a projectile motion Quantities. Computes an initial velocity based on the length.
        /// </summary>
        /// <param name="α">An elevation angle.</param>
        /// <param name="length">The length.</param>
        /// <param name="h">An initial height.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(Length length, ElevationAngle α, InitialHeight h, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            Α = α;
            H = h;
            G = g;
            V = new InitialVelocity(length.GetBasicVal() * Math.Sqrt(G.GetBasicVal() * 1 / Math.Cos(Α.GetBasicVal())) / Math.Sqrt(2.0 * length.GetBasicVal() * Math.Sin(Α.GetBasicVal()) + 2.0 * H.GetBasicVal() * Math.Cos(Α.GetBasicVal())), UnitVelocity.Basic).Convert(Units.Velocity);

            CheckData();
            UsedAssignmentType = AssignmentsTypes.InitialVelocityByLength;
        }

        /// <summary>
        /// 7. Constructor for a projectile motion Quantities. Computes an elevation angle based on the length.
        /// </summary>
        /// <param name="v">An initial velocity.</param>
        /// <param name="length">The length.</param>
        /// <param name="h">An initial height.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(Length length, InitialVelocity v, InitialHeight h, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            // TODO chyba
            // solve(d= v*cos(a)*(v*sin(a) + sqrt(v^2*sin(a)^2+2*g*y))/g, a) wolframalpha

            Units = units ?? new ProjectileMotionResultsUnits();

            V = v;
            H = h;
            G = g;

            Α = new ElevationAngle(
                    GetRootWithComputeExpection(EquationSolver.BisectionFindRoot(a => V.GetBasicVal() * Math.Cos(a) * (V.GetBasicVal() * Math.Sin(a) + Math.Sqrt(Math.Pow(V.GetBasicVal() * Math.Sin(a), 2) + 2.0 * G.GetBasicVal() * H.GetBasicVal())) / G.GetBasicVal() - length.GetBasicVal(), 0, new ElevationAngle(ElevationAngle.ElevationAngleTypes.Right).Val, 1E-4)),
                    UnitAngle.Basic
                ).Convert(Units.Angle);

            CheckData();
            UsedAssignmentType = AssignmentsTypes.ElevationAngleByLength;
        }

        /// <summary>
        /// 8. Constructor for a projectile motion Quantities. Computes an initial velocity based on the maximal height.
        /// </summary>
        /// <param name="α">An elevation angle.</param>
        /// <param name="maxHeight">The maximal height.</param>
        /// <param name="h">An initial height.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(MaximalHeight maxHeight, ElevationAngle α, InitialHeight h, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            Α = α;
            H = h;
            G = g;
            V = new InitialVelocity(Math.Sqrt(2.0 * G.GetBasicVal() * (maxHeight.GetBasicVal() - H.GetBasicVal()) / Math.Pow(Math.Sin(Α.GetBasicVal()), 2.0)), UnitVelocity.Basic).Convert(Units.Velocity);

            CheckData();
            UsedAssignmentType = AssignmentsTypes.InitialVelocityByMaxHeight;
        }

        /// <summary>
        /// 9. Constructor for a projectile motion Quantities. Computes an initial height based on the maximal height.
        /// </summary>
        /// <param name="α">An elevation angle.</param>
        /// <param name="maxHeight">The maximal height.</param>
        /// <param name="v">An initial velocity.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(MaximalHeight maxHeight, ElevationAngle α, InitialVelocity v, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            V = v;
            Α = α;
            G = g;
            H = new InitialHeight(maxHeight.GetBasicVal() - Math.Pow(V.GetBasicVal() * Math.Sin(Α.GetBasicVal()), 2.0) / (2.0 * G.GetBasicVal()), UnitLength.Basic).Convert(Units.Length);

            CheckData();
            UsedAssignmentType = AssignmentsTypes.InitialHeightByMaxHeight;
        }

        /// <summary>
        /// 10. Constructor for a projectile motion Quantities. Computes an elevation angle based on the maximal height.
        /// </summary>
        /// <param name="v">An initial velocity.</param>
        /// <param name="dur">The maximal height.</param>
        /// <param name="h">An initial height.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(MaximalHeight maxHeight, InitialVelocity v, InitialHeight h, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            V = v;
            H = h;
            G = g;

            Α = new ElevationAngle(Math.Asin(Math.Sqrt(2.0 * G.GetBasicVal() * maxHeight.GetBasicVal() / Math.Pow(V.GetBasicVal(), 2))), UnitAngle.Basic).Convert(Units.Angle);

            CheckData();
            UsedAssignmentType = AssignmentsTypes.ElevationAngleByMaxHeight;
        }


        /// <summary>
        /// 11. Constructor for a projectile motion Quantities. Computes an elevation angle for motion with maximum range.
        /// </summary>
        /// <param name="v">An initial velocity.</param>
        /// <param name="h">An initial height.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(InitialVelocity v, InitialHeight h, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            V = v;
            H = h;
            G = g;

            Α = new ElevationAngle(Math.Acos(Math.Sqrt((2.0 * G.GetBasicVal() * H.GetBasicVal() + Math.Pow(V.GetBasicVal(), 2.0)) / (2.0 * G.GetBasicVal() * H.GetBasicVal() + 2.0 * Math.Pow(V.GetBasicVal(), 2.0)))), UnitAngle.Basic).Convert(Units.Angle);

            CheckData();
            UsedAssignmentType = AssignmentsTypes.ElevationAngleGetMaxRange;
        }


        public AssignmentsTypes UsedAssignmentType { get; set; }

        public enum AssignmentsTypes
        {
            Basic,
            InitialHeightByDuration,
            ElevationAngleByDuration,
            InitialVelocityByDuration,
            InitialHeightByMaxHeight,
            ElevationAngleByMaxHeight,
            InitialVelocityByMaxHeight,
            InitialHeightByLength,
            ElevationAngleByLength,
            InitialVelocityByLength,
            ElevationAngleGetMaxRange
        }


        public static Dictionary<AssignmentsTypes, string> AssignmentsTypesTranslations = new Dictionary<AssignmentsTypes, string>()
        {
            { AssignmentsTypes.Basic, "Basic quantities" },
            { AssignmentsTypes.InitialHeightByDuration, "An initial height by duration" },
            { AssignmentsTypes.ElevationAngleByDuration, "An elevation angle by duration" },
            { AssignmentsTypes.InitialVelocityByDuration, "An initial velocity by duration" },
            { AssignmentsTypes.InitialHeightByMaxHeight, "An initial height by maximal height" },
            { AssignmentsTypes.ElevationAngleByMaxHeight, "An elevation angle by maximal height" },
            { AssignmentsTypes.InitialVelocityByMaxHeight, "An initial velocity by maximal height" },
            { AssignmentsTypes.InitialHeightByLength, "An initial height by length" },
            { AssignmentsTypes.ElevationAngleByLength, "An elevation angle by length" },
            { AssignmentsTypes.InitialVelocityByLength, "An initial velocity by length" },
            { AssignmentsTypes.ElevationAngleGetMaxRange, "An elevation angle to get maximal range" }
        };


        protected double GetRootWithComputeExpection(double? equationSolverOutput)
        {
            if (equationSolverOutput.HasValue)
            {
                return equationSolverOutput.Value;
            }

            throw new UnableToComputeQuantityException(UNABLETOCOMPUTEQUANTITYTEXT);
        }


        public InitialVelocity V { get; private set; }

        public ElevationAngle Α { get; private set; }

        public InitialHeight H { get; private set; }

        public GravAcceleration G { get; private set; }


        private void CheckData()
        {

            if (double.IsNaN(V.Val) ||
                double.IsNaN(H.Val) ||
                double.IsNaN(G.Val) ||
                double.IsNaN(Α.Val)
            )
            {
                throw new UnableToComputeQuantityException(UNABLETOCOMPUTEQUANTITYTEXT);
            }
        }


        private const string UNABLETOCOMPUTEQUANTITYTEXT = "Unable to compute some quantities. The combination of entered quantities is not valid for any real projectile motion.";
        


        /// <summary>
        /// Units of projectile motion Quantities.
        /// </summary>
        public ProjectileMotionResultsUnits Units { get; private set; }
    }

    /// <summary>
    /// Settings class for projectile motion.
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
                throw new Exception("The number of points to use to draw the function course cannot be smaller than zero");
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
        /// The file in which is function course of projectile motion saved (excel).
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


        private string _HexColorOfFunctionCourse { get; set; }

        /// <summary>
        /// The file in which is function course of projectile motion saved (excel).
        /// </summary>
        public string HexColorOfTrajectory
        {
            get
            {
                return _HexColorOfFunctionCourse;
            }
            set
            {
                try
                {
                    if (!ColorTranslator.FromHtml(value).IsEmpty)
                    {
                        _HexColorOfFunctionCourse = value;
                    }
                    else throw new Exception();
                }
                catch (Exception) {
                    _HexColorOfFunctionCourse = "#6c757d";
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

    /// <summary>
    /// Units for projectile motion results Quantities.
    /// </summary>
    public class ProjectileMotionResultsUnits
    {
        public ProjectileMotionResultsUnits ()
        {
            Velocity = UnitVelocity.Basic;
            Angle = UnitAngle.Basic;
            Length = UnitLength.Basic;
            Time = UnitTime.Basic;
            GravAcceleration = UnitGravAcceleration.Basic;
            Area = UnitArea.Basic;
        }

        /// <summary>
        /// The unit of velocity.
        /// </summary>
        public UnitVelocity Velocity { get; set; }


        /// <summary>
        /// The unit of length.
        /// </summary>
        public UnitLength Length { get; set; }


        /// <summary>
        /// The unit of time.
        /// </summary>
        public UnitTime Time { get; set; }


        /// <summary>
        /// The unit of area.
        /// </summary>
        public UnitArea Area { get; set; }


        /// <summary>
        /// The unit of angle.
        /// </summary>
        public UnitAngle Angle { get; set; }


        /// <summary>
        /// The unit of gravitation acceleration.
        /// </summary>
        public UnitGravAcceleration GravAcceleration { get; set; }
    }

    /// <summary>
    /// Projectile motion features.
    /// </summary>
    public class ProjectileMotion
    {
        /// <summary>
        /// Constructor for a projectile motion.
        /// </summary>
        /// <param name="settings">Settings object for projectile motion. It cannot be null.</param>
        public ProjectileMotion(ProjectileMotionSettings settings)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings), "Settings object cannot be null.");
            Saving = new ProjectileMotionFilesSaving(this);
        }


        /// <summary>
        /// Projectile motion settings.
        /// </summary>
        public ProjectileMotionSettings Settings { get; private set; }

        public ProjectileMotionFilesSaving Saving { get; protected set; }

        public virtual ProjectileMotionPoint GetPoint (Time t)
        {
            return new ProjectileMotionPoint(this, t);
        }

        public virtual ProjectileMotionPoint GetPoint (ProjectileMotionPoint.ProjectileMotionPointTypes type)
        {
            return new ProjectileMotionPoint(this, type);
        }

        /// <summary>
        /// Gets constant horizontal velocity in the set unit for velocity.
        /// </summary>
        public virtual Velocity GetVelocityX()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Initial).Round().Vx.Convert(Settings.Quantities.Units.Velocity);
        }

        /// <summary>
        /// Gets vertical velocity in the set unit for velocity.
        /// </summary>
        /// <param name="t">The time in the set unit.</param>
        public virtual Velocity GetVelocityY(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).Round().Vy.Convert(Settings.Quantities.Units.Velocity);
        }


        /// <summary>
        /// Gets the X coordination of the point in the set unit for length.
        /// </summary>
        /// <param name="t">The time in the set unit.</param>
        public virtual Length GetX(double t)
        {
           return GetPoint(new Time(t, Settings.Quantities.Units.Time)).Round().X.Convert(Settings.Quantities.Units.Length);
        }


        /// <summary>
        /// Gets the velocity in the point in the set unit for velocity.
        /// </summary>
        /// <param name="t">The time in the set unit.</param>
        public virtual Velocity GetVelocity(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).GetVelocity(Settings.Quantities.Units.Velocity).RoundVal(Settings.RoundDigits);
        }


        /// <summary>
        /// Gets the Y coordination of the point in the set unit for length.
        /// </summary>
        /// <param name="t">The time in the set unit.</param>
        public virtual Length GetY(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).Round().Y.Convert(Settings.Quantities.Units.Length);
        }


        /// <summary>
        /// Gets the distance between the point and beginning in the set unit for length.
        /// </summary>
        /// <param name="t">The time in the set unit.</param>
        public virtual Length GetDistance(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).GetDistance(Settings.Quantities.Units.Length).RoundVal(Settings.RoundDigits);
        }


        /// <summary>
        /// Gets the distance between two points.
        /// </summary>
        /// <param name="t1">The time of the first point.</param>
        /// <param name="t2">The time of the second point.</param>
        public virtual Length GetDistanceBetweenTwoPoints(double t1, double t2)
        {
            return GetPoint(new Time(t1, Settings.Quantities.Units.Time)).GetDistanceFromPoint(GetPoint(new Time(t2, Settings.Quantities.Units.Time)), Settings.Quantities.Units.Length).RoundVal(Settings.RoundDigits);
        }


        /// <summary>
        /// Gets the distance between the farthest point (from the beginning) and the beginning in the set unit for length.
        /// </summary>
        public virtual Length GetMaxDistance()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Farthest).GetDistance(Settings.Quantities.Units.Length).RoundVal(Settings.RoundDigits);
        }


        /// <summary>
        /// Gets duration of the motion in the set unit for time.
        /// </summary>
        public virtual Time GetDur()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Final).Round().T.Convert(Settings.Quantities.Units.Time);
        }


        /// <summary>
        /// Gets the length in the set unit for length.
        /// </summary>
        public virtual Length GetLength()
        {
            return GetLength(Settings.Quantities.Units.Length).RoundVal(Settings.RoundDigits);
        }


        protected Length GetLength(UnitLength unitLength)
        {
            return new Length(Settings.Quantities.V.GetBasicVal() * Math.Cos(Settings.Quantities.Α.GetBasicVal()) * (Settings.Quantities.V.GetBasicVal() * Math.Sin(Settings.Quantities.Α.GetBasicVal()) + Math.Sqrt(Math.Pow(Settings.Quantities.V.GetBasicVal() * Math.Sin(Settings.Quantities.Α.GetBasicVal()), 2.0) + 2 * Settings.Quantities.G.GetBasicVal() * Settings.Quantities.H.GetBasicVal())) / Settings.Quantities.G.GetBasicVal(), UnitLength.Basic).Convert(unitLength);
        }


        /// <summary>
        /// Gets the length of the arc elapsed by the projectile (circuit) using definite integral in the set unit for length.
        /// </summary>
        public virtual Length GetArcLength()
        {
            return GetArcLength(Settings.Quantities.Units.Length).RoundVal(Settings.RoundDigits);
        }

        protected Length GetArcLength(UnitLength unitLength)
        {
            return new Length(Integrate.OnClosedInterval(x => Math.Sqrt(1 + Math.Pow(Math.Tan(Settings.Quantities.Α.GetBasicVal()) - (Settings.Quantities.G.GetBasicVal() / Math.Pow(Settings.Quantities.V.GetBasicVal() * Math.Cos(Settings.Quantities.Α.GetBasicVal()), 2.0)) * x, 2.0)), 0, GetLength(UnitLength.Basic).Val), UnitLength.Basic).Convert(unitLength);
        }


        /// <summary>
        /// Gets the area bounded by an arc and X axis in the set unit for area.
        /// </summary>
        public virtual Area GetAreaUnderArc()
        {
            return GetAreaUnderArc(Settings.Quantities.Units.Area).RoundVal(Settings.RoundDigits);
        }

        protected Area GetAreaUnderArc(UnitArea unitArea)
        {
            return new Area(Math.Pow(GetLength(UnitLength.Basic).Val, 2.0) * Math.Tan(Settings.Quantities.Α.GetBasicVal()) / 2.0 - Math.Pow(GetLength(UnitLength.Basic).Val, 3.0) * Settings.Quantities.G.GetBasicVal() / (6.0 * Math.Pow(Settings.Quantities.V.GetBasicVal() * Math.Cos(Settings.Quantities.Α.GetBasicVal()), 2.0)) + GetLength(UnitLength.Basic).Val * Settings.Quantities.H.GetBasicVal(), UnitArea.Basic).Convert(unitArea); 
        }


        /// <summary>
        /// Gets height of the highest point.
        /// </summary>
        public virtual Length GetMaxHeight()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Highest).Round().Y.Convert(Settings.Quantities.Units.Length);
        }


        /// <summary>
        /// Gets coordinates of the point in the set unit for length.
        /// </summary>
        /// <param name="t">the time in the set unit.</param>
        public virtual double[] GetCoords(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).Round().GetCoords(Settings.Quantities.Units.Length);
        }


        /// <summary>
        /// Gets the time of the point when fallen in the set unit for time.
        /// </summary>
        public virtual Time GetTimeFallen()
        {
            return GetDur();
        }

        /// <summary>
        /// Gets coordinates of the farthest point in the set unit for length.
        /// </summary>
        public virtual double[] GetCoordsFarthest()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Farthest).Round().GetCoords(Settings.Quantities.Units.Length);
        }

        /// <summary>
        /// Gets coordinates of the point when fallen in the set unit for length.
        /// </summary>
        public virtual double[] GetCoordsFallen()
        {
            return new double[2] { GetLength(Settings.Quantities.Units.Length).GetRoundedVal(Settings.RoundDigits), 0 };
        }


        /// <summary>
        /// Gets the time in the point that is the farthest point from the beginning.
        /// </summary>
        /// <returns></returns>
        public virtual Time GetTimeFarthest()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Farthest).Round().T.Convert(Settings.Quantities.Units.Time);
        }

        /// <summary>
        /// Gets coordinates of the highest point in the unit set for length.
        /// </summary>
        public virtual double[] GetCoordsHighest()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Highest).Round().GetCoords(Settings.Quantities.Units.Length);
        }


        /// <summary>
        /// Gets the time of the highest point.
        /// </summary>
        public virtual Time GetTimeHighest()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Highest).Round().T.Convert(Settings.Quantities.Units.Time);
        }


        /// <summary>
        /// Gets the course of the function with the set number of points that describe the projectile motion.
        /// </summary>
        /// <returns>An array of the points coordinates.</returns>
        public virtual double[][] GetTrajectory()
        {
            return GetListPointsOfTrajectory().Select(p => p.Round().GetCoords(Settings.Quantities.Units.Length)).ToArray();
        }


        public virtual List<ProjectileMotionPoint> GetListPointsOfTrajectory()
        {
            List<ProjectileMotionPoint> ret = new List<ProjectileMotionPoint>();

            ProjectileMotionPoint finalPoint = GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Final);
            ProjectileMotionPoint highestPoint = GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Highest);
            ProjectileMotionPoint farthestPoint = GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Farthest);

            for (int i = 0; i < Settings.PointsForTrajectory - 1; i++)
            {
                Time now = new Time(finalPoint.T.GetBasicVal() / (Settings.PointsForTrajectory - 1) * i, UnitTime.Basic).RoundVal(Settings.RoundDigits);
                Time next = new Time(finalPoint.T.GetBasicVal() / (Settings.PointsForTrajectory - 1) * (i + 1), UnitTime.Basic).RoundVal(Settings.RoundDigits);

                if (now != highestPoint.T && now != farthestPoint.T)
                {
                    ret.Add(GetPoint(now));
                }
                if (next >= highestPoint.T && now <= highestPoint.T)
                {
                    ret.Add(highestPoint);
                }
                else if (next >= farthestPoint.T && now <= farthestPoint.T)
                {
                    ret.Add(farthestPoint);
                }
            }

            if (ret.Last().Y.Val > 0) ret.Add(finalPoint);

            return ret;
        }


        /// <summary>
        /// Saves basic information to the defined .txt file to the defined folder path.
        /// </summary>
        /// <returns>Projectile Motion instance.</returns>
        public virtual ProjectileMotion SaveInfoToTxt()
        {
            Saving.InfoToTxt();
            return this;
        }

        /// <summary>
        /// Saves data to the defined .csv file to the defined folder path.
        /// </summary>
        /// <returns>Projectile Motion instance.</returns>
        public virtual ProjectileMotion SaveDataToCsv()
        {
            Saving.DataToCsv();
            return this;
        }
    }
}