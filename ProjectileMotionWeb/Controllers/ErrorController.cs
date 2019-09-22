using System.Web.Mvc;
using ProjectileMotionWeb.Models;

namespace ProjectileMotionWeb.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction(nameof(Default));
        }


        [HttpGet]
        public ActionResult Default(string aspxerrorpath)
        {
            return View(new BaseModel(new LayoutModel("Unexpected error") { }));
        }


        [HttpGet]
        public ActionResult NotFound (string aspxerrorpath)
        {
            return View(new BaseModel(new LayoutModel("Unexpected error - not found") { }));
        }


        [HttpGet]
        public ActionResult Internal (string aspxerrorpath)
        {
            return View(new BaseModel(new LayoutModel("Unexpected error - internal server error") { }));
        }


        [HttpGet]
        public ActionResult Forbidden (string aspxerrorpath)
        {
            return View(new BaseModel(new LayoutModel("Unexpected error - forbidden") { }));
        }
    }
}