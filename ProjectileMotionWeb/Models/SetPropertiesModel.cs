using System.ComponentModel.DataAnnotations;

namespace ProjectileMotionWeb.Models
{
    public class SetPropertiesModel : BaseModel
    {
        public SetPropertiesModel()
        { /* POST! */ }

        internal const string REQUIREDTEXT = "* Required";
        internal const string REQUIREDWITHREZISTANCETEXT = REQUIREDTEXT + " for motions with rezistance";
        internal const string REQUIREDFORASSIGNMENT = REQUIREDTEXT + " for selected assignment";
        internal const string LARGERTHANZEROTEXT = "* Larger than zero";

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
        [Display(Name = "Round to digits")]
        public int RoundDigits { get; set; }

        [Required(ErrorMessage = REQUIREDTEXT)]
        [Range(1, int.MaxValue, ErrorMessage = LARGERTHANZEROTEXT)]
        [Display(Name = "Points for function course")]
        public int PointsForFunctionCourse { get; set; }

        [Display(Name = ".txt file name")]
        public string TxtInfoFileName { get; set; }

        [Display(Name = ".csv file name")]
        public string CsvDataFileName { get; set; }

        [Display(Name = ".pdf file name")]
        public string PdfInfoFileName { get; set; }

        [Display(Name = "Display also course of motion neglecting rezistance")]
        public bool ShowMotionWithoutRezistanceCourseToo { get; set; }

        public SetPropertiesQuantitiesModel Quantities { get; set; }
    }
}