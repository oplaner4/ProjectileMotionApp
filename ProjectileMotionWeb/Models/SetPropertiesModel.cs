using System.ComponentModel.DataAnnotations;

namespace ProjectileMotionWeb.Models
{
    public class SetPropertiesModel : BaseModel
    {
        public SetPropertiesModel()
        { /* POST! */ }

        internal const string REQUIREDTEXT = "* Required";
        internal const string REQUIREDWITHRESISTANCETEXT = REQUIREDTEXT + " for motions with resistance";
        internal const string REQUIREDFORASSIGNMENT = REQUIREDTEXT + " for selected assignment";
        internal const string LARGERTHANZEROTEXT = "* Larger than zero";
        internal const string LARGEROREQUALTOZEROTEXT = "* Larger or equal to zero";

        [Display(Name = "Velocity")]
        public string ResultUnitVelocity { get; set; }

        [Display(Name = "Area")]
        public string ResultUnitArea { get; set; }

        [Display(Name = "Time")]
        public string ResultUnitTime { get; set; }

        [Display(Name = "Gravitation acceleration")]
        public string ResultUnitGravAcceleration { get; set; }

        [Display(Name = "Length")]
        public string ResultUnitLength { get; set; }

        [Display(Name = "Angle")]
        public string ResultUnitAngle { get; set; }

        [Required(ErrorMessage = REQUIREDTEXT)]
        [Range(1, int.MaxValue, ErrorMessage = LARGERTHANZEROTEXT)]
        [Display(Name = "Round to the number of digits")]
        public int RoundDigits { get; set; }

        [Required(ErrorMessage = REQUIREDTEXT)]
        [Range(1, int.MaxValue, ErrorMessage = LARGERTHANZEROTEXT)]
        [Display(Name = "Points to use to draw trajectory")]
        public int PointsForTrajectory { get; set; }

        [Display(Name = ".txt file name")]
        public string TxtInfoFileName { get; set; }

        [Display(Name = ".csv file name")]
        public string CsvDataFileName { get; set; }

        [Display(Name = ".pdf file name")]
        public string PdfInfoFileName { get; set; }

        [Display(Name = "Display also trajectory of the motion neglecting resistance")]
        public bool ShowMotionWithoutResistanceTrajectoryToo { get; set; }

        [Display(Name = "Recalculate")]
        public bool RecalculateOnUnitChange { get; set; }

        [Display(Name = "The color of the trajectory")]
        public string HexColorOfTrajectory { get; set; }

        public SetPropertiesQuantitiesModel Quantities { get; set; }
    }
}