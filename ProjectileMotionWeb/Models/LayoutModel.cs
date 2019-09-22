using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectileMotionWeb.Models
{
    public class LayoutModel
    {
        public LayoutModel (string title)
        {
            Title = title;
            ActiveMenuItem = ActiveNavItem.Home;
            FluidContainer = false;
        }


        public enum ActiveNavItem
        {
            Home,
            Choose,
            Set,
            MotionDropdown
        }

        public string Title { get; set; }

        public ActiveNavItem ActiveMenuItem { get; set; }

        public bool FluidContainer { get; set; }
    }
}