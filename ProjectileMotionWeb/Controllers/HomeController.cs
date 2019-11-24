using System.Web.Mvc;
using ProjectileMotionWeb.Models;
using ProjectileMotionData;

namespace ProjectileMotionWeb.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index ()
        {
            return RedirectToAction(nameof(Start));
        }


        public ActionResult Start()
        {

            SessionStore session = GetSession();
            BaseModel viewModel = new BaseModel(new LayoutModel("Welcome"))
            {
                PagePreviouslyVisited = session.WasActionVisited("Home", nameof(HomeController.Start))
            };

            viewModel.Layout.Menu.ActiveMenuItem = LayoutMenuModel.ActiveNavItem.Home;

            session.SaveActionVisited("Home", nameof(HomeController.Start));

            return View(viewModel);
        }
    }
}