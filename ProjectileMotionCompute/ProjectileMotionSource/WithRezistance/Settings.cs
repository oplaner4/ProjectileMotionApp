using ProjectileMotionSource.Func;

namespace ProjectileMotionSource.WithRezistance.Func
{
    public class ProjectileMotionWithRezistanceSettings : ProjectileMotionSettings
    {
        public ProjectileMotionWithRezistanceSettings(ProjectileMotionWithRezistanceQuantities quantities) : base(quantities)
        {
            Quantities = quantities;

            TxtInfoFileName = GetDefaultFileName("txt");
            CsvDataFileName = GetDefaultFileName("csv");
            PdfInfoFileName = GetDefaultFileName("pdf");
            HexColorOfTrajectory = "#007bff";
        }

        private new string GetDefaultFileName(string extension)
        {
            return string.Format(
                "ProjectileMotionWithRezistance-{0} {1}-{2} {3}-{4} {5}-{6} {7}-{8} {9} {10} {11}.{12}",
                Quantities.Α.GetRoundedVal(RoundDigits).ToString(),
                Quantities.Α.Unit.Name,
                Quantities.V.GetRoundedVal(RoundDigits).ToString(),
                Quantities.V.Unit.Name,
                Quantities.H.GetRoundedVal(RoundDigits).ToString(),
                Quantities.H.Unit.Name,
                Quantities.A.GetRoundedVal(RoundDigits).ToString(),
                Quantities.A.Unit.Name,
                Quantities.M.GetRoundedVal(RoundDigits).ToString(),
                Quantities.M.Unit.Name,
                Quantities.Ρ.GetRoundedVal(RoundDigits).ToString(),
                Quantities.Ρ.Unit.Name,
                extension
            );
        }


        /// <summary>
        /// Gets the name for chart to be exported with the specified extension.
        /// </summary>
        public override string GetChartFileNameForExport(string extension)
        {
            return "Chart-" + GetDefaultFileName(extension);
        }


        public bool ShowMotionWithoutRezistanceTrajectoryToo { get; set; }

        public new ProjectileMotionWithRezistanceQuantities Quantities { get; private set; }
    }
}