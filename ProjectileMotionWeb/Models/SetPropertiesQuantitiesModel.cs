using System.ComponentModel.DataAnnotations;
using ProjectileMotionWeb.Helpers;
using ProjectileMotionSource.Func;

namespace ProjectileMotionWeb.Models
{
    public class SetPropertiesQuantitiesModel
    {
        public SetPropertiesQuantitiesModel()
        { /* POST! */ }


        [Display(Name = "With resistance")]
        public bool WithResistance { get; set; }

        [Required(ErrorMessage = SetPropertiesModel.REQUIREDTEXT)]
        [Range(0, double.MaxValue, ErrorMessage = "* Larger or equal to zero")]
        [Display(Name = "Initial height")]
        public double InitialHeight { get; set; }

        public string InitialHeightUnit { get; set; }

        [Required(ErrorMessage = SetPropertiesModel.REQUIREDTEXT)]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = SetPropertiesModel.LARGERTHANZEROTEXT)]
        [Display(Name = "Initial velocity")]
        public double InitialVelocity { get; set; }

        public string InitialVelocityUnit { get; set; }

        [Required(ErrorMessage = SetPropertiesModel.REQUIREDTEXT)]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = SetPropertiesModel.LARGERTHANZEROTEXT)]
        [Display(Name = "Gravitation acceleration")]
        public double GravAcceleration { get; set; }

        public string GravAccelerationUnit { get; set; }

        [Required(ErrorMessage = SetPropertiesModel.REQUIREDTEXT)]
        [MaxRightElevationAngleAttribute]
        [Range(0, double.MaxValue, ErrorMessage = SetPropertiesModel.LARGEROREQUALTOZEROTEXT)]
        [Display(Name = "Elevation angle")]
        public double ElevationAngle { get; set; }

        public string ElevationAngleUnit { get; set; }

        [RequiredIf(nameof(WithResistance), true, ErrorMessage = SetPropertiesModel.REQUIREDWITHRESISTANCETEXT)]
        [RangeIf(nameof(WithResistance), true, double.Epsilon, double.MaxValue, ErrorMessage = SetPropertiesModel.LARGERTHANZEROTEXT)]
        [Display(Name = "Mass")]
        public double? Mass { get; set; }

        public string MassUnit { get; set; }

        [RequiredIf(nameof(WithResistance), true, ErrorMessage = SetPropertiesModel.REQUIREDWITHRESISTANCETEXT)]
        [RangeIf(nameof(WithResistance), true, double.Epsilon, double.MaxValue, ErrorMessage = SetPropertiesModel.LARGERTHANZEROTEXT)]
        [Display(Name = "Density")]
        public double? Density { get; set; }

        public string DensityUnit { get; set; }

        [RequiredIf(nameof(WithResistance), true, ErrorMessage = SetPropertiesModel.REQUIREDWITHRESISTANCETEXT)]
        [RangeIf(nameof(WithResistance), true, double.Epsilon, double.MaxValue, ErrorMessage = SetPropertiesModel.LARGERTHANZEROTEXT)]
        [Display(Name = "Frontal area")]
        public double? FrontalArea { get; set; }

        public string FrontalAreaUnit { get; set; }

        [RequiredIf(nameof(WithResistance), true, ErrorMessage = SetPropertiesModel.REQUIREDWITHRESISTANCETEXT)]
        [RangeIf(nameof(WithResistance), true, double.Epsilon, double.MaxValue, ErrorMessage = SetPropertiesModel.LARGERTHANZEROTEXT)]
        [Display(Name = "Drag coefficient")]
        public double? DragCoefficient { get; set; }

        [RequiredIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.InitialHeightByDuration, ErrorMessage = SetPropertiesModel.REQUIREDFORASSIGNMENT)]
        [RequiredIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.ElevationAngleByDuration, ErrorMessage = SetPropertiesModel.REQUIREDFORASSIGNMENT)]
        [RequiredIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.InitialVelocityByDuration, ErrorMessage = SetPropertiesModel.REQUIREDFORASSIGNMENT)]
        [RequiredIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.ElevationAngleByLengthAndDur, ErrorMessage = SetPropertiesModel.REQUIREDFORASSIGNMENT)]
        [RequiredIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.InitialVelocityByLengthAndDur, ErrorMessage = SetPropertiesModel.REQUIREDFORASSIGNMENT)]

        [RangeIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.InitialHeightByDuration, double.Epsilon, double.MaxValue, ErrorMessage = SetPropertiesModel.LARGERTHANZEROTEXT)]
        [RangeIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.ElevationAngleByDuration, double.Epsilon, double.MaxValue, ErrorMessage = SetPropertiesModel.LARGERTHANZEROTEXT)]
        [RangeIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.InitialVelocityByDuration, double.Epsilon, double.MaxValue, ErrorMessage = SetPropertiesModel.LARGERTHANZEROTEXT)]

        [Display(Name = "Duration")]
        public double? Duration { get; set; }

        public string DurationUnit { get; set; }

        [RequiredIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.ElevationAngleByMaxHeight, ErrorMessage = SetPropertiesModel.REQUIREDFORASSIGNMENT)]
        [RequiredIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.InitialHeightByMaxHeight, ErrorMessage = SetPropertiesModel.REQUIREDFORASSIGNMENT)]
        [RequiredIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.InitialVelocityByMaxHeight, ErrorMessage = SetPropertiesModel.REQUIREDFORASSIGNMENT)]

        [RangeIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.ElevationAngleByMaxHeight, double.Epsilon, double.MaxValue, ErrorMessage = SetPropertiesModel.LARGERTHANZEROTEXT)]
        [RangeIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.InitialHeightByMaxHeight, double.Epsilon, double.MaxValue, ErrorMessage = SetPropertiesModel.LARGERTHANZEROTEXT)]
        [RangeIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.InitialVelocityByMaxHeight, double.Epsilon, double.MaxValue, ErrorMessage = SetPropertiesModel.LARGERTHANZEROTEXT)]

        [Display(Name = "Maximal height")]
        public double? MaxHeight { get; set; }

        public string MaxHeightUnit { get; set; }


        [RequiredIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.InitialHeightByLength, ErrorMessage = SetPropertiesModel.REQUIREDFORASSIGNMENT)]
        [RequiredIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.ElevationAngleByLength, ErrorMessage = SetPropertiesModel.REQUIREDFORASSIGNMENT)]
        [RequiredIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.InitialVelocityByLength, ErrorMessage = SetPropertiesModel.REQUIREDFORASSIGNMENT)]
        [RequiredIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.ElevationAngleByLengthAndDur, ErrorMessage = SetPropertiesModel.REQUIREDFORASSIGNMENT)]
        [RequiredIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.InitialVelocityByLengthAndDur, ErrorMessage = SetPropertiesModel.REQUIREDFORASSIGNMENT)]

        [RangeIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.InitialHeightByLength, 0, double.MaxValue,ErrorMessage = SetPropertiesModel.LARGEROREQUALTOZEROTEXT)]
        [RangeIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.ElevationAngleByLength, 0, double.MaxValue, ErrorMessage = SetPropertiesModel.LARGEROREQUALTOZEROTEXT)]
        [RangeIf(nameof(SelectedAssignmentType), ProjectileMotionQuantities.AssignmentsTypes.InitialVelocityByLength, 0, double.MaxValue, ErrorMessage = SetPropertiesModel.LARGEROREQUALTOZEROTEXT)]

        [Display(Name = "Length")]
        public double? Length { get; set; }

        public string LengthUnit { get; set; }

        public ProjectileMotionQuantities.AssignmentsTypes SelectedAssignmentType { get; set; }
    }
}