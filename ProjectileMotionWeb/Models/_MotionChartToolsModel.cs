namespace ProjectileMotionWeb.Models
{
    public class _MotionChartToolsModel : BaseModel
    {
        public _MotionChartToolsModel(bool showLargerMotionChart, bool showMotionWithoutRezistanceCourseToo, bool motionWithRezistance)
        {
            ShowLargerMotionChart = showLargerMotionChart;
            ShowMotionWithoutRezistanceCourseToo = showMotionWithoutRezistanceCourseToo;
            MotionWithRezistance = motionWithRezistance;
        }

        public bool ShowLargerMotionChart { get; private set; }
        public bool ShowMotionWithoutRezistanceCourseToo { get; private set; }
        public bool MotionWithRezistance { get; private set; }
    }
}