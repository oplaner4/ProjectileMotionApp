namespace ProjectileMotionWeb.Models
{
    public class LayoutMenuModel
    {
        public LayoutMenuModel () {
            ActiveMenuItem = ActiveNavItem.Home;
        }

        public LayoutMenuModel (ActiveNavItem activeItem)
        {
            ActiveMenuItem = activeItem;
        }

        public bool? SetWithResistance { get; set; }

        public enum ActiveNavItem
        {
            Home,
            Choose,
            Set,
            MotionDropdown
        }

        public ActiveNavItem ActiveMenuItem { get; set; }
    }
}