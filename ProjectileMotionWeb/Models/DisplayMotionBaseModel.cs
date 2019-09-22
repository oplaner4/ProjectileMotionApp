namespace ProjectileMotionWeb.Models
{
    public class DisplayMotionBaseModel : BaseModel
    {
        public DisplayMotionBaseModel(bool showLargerMotionChart)
        {
            ShowLargerMotionChart = showLargerMotionChart;
        }

        public bool ShowLargerMotionChart { get; set; }
    }
}