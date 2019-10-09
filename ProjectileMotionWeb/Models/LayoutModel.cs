namespace ProjectileMotionWeb.Models
{
    public class LayoutModel
    {
        public LayoutModel (string title)
        {
            Title = title;
            FluidContainer = false;
            Menu = new LayoutMenuModel();
        }


        public LayoutModel(string title, LayoutMenuModel menu)
        {
            Title = title;
            FluidContainer = false;
            Menu = menu;
        }


        public string Title { get; set; }

        public bool FluidContainer { get; set; }

        public LayoutMenuModel Menu { get; set; }
    }
}