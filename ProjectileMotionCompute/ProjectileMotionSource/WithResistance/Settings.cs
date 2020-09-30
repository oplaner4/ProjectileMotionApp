using ProjectileMotionSource.Func;

namespace ProjectileMotionSource.WithResistance.Func
{
    public class ProjectileMotionWithResistanceSettings : ProjectileMotionSettings
    {
        public ProjectileMotionWithResistanceSettings(ProjectileMotionWithResistanceQuantities quantities) : base(quantities)
        {
            Quantities = quantities;

            TxtInfoFileName = GetDefaultFileName("txt");
            CsvDataFileName = GetDefaultFileName("csv");
            PdfInfoFileName = GetDefaultFileName("pdf");
            ChartFileName = "chart-" + GetDefaultFileName(string.Empty);
            HexColorOfTrajectory = "#007bff";
        }

        private new string GetDefaultFileName(string extension)
        {
            return string.Format(
                "ProjectileMotionWithResistance-{0} {1}-{2} {3}-{4} {5}-{6} {7}-{8} {9} {10} {11}" + (extension.Length == 0 ? "" : ".{12}"),
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

        public bool ShowMotionWithoutResistanceTrajectoryToo { get; set; }

        public new ProjectileMotionWithResistanceQuantities Quantities { get; private set; }
    }
}