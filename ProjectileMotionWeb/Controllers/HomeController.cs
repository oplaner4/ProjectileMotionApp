using System.Web.Mvc;
using ProjectileMotionWeb.Models;
using ProjectileMotionData;

namespace ProjectileMotionWeb.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction(nameof(Start));
        }


        public ActionResult Start()
        {

            SessionStore session = GetSession();
            BaseModel viewModel = new BaseModel(new LayoutModel("Welcome") { ActiveMenuItem = LayoutModel.ActiveNavItem.Home })
            {
                PagePreviouslyVisited = session.WasActionVisited("Home", nameof(HomeController.Start))
            };

            session.SaveActionVisited("Home", nameof(HomeController.Start));

            return View(viewModel);
        }
    }
}