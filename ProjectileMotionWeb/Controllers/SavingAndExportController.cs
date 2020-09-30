using ProjectileMotionWeb.Helpers;
using System.Web.Mvc;
using ProjectileMotionWeb.Models;
using ProjectileMotionSource.Func;
using Utilities.Files;

namespace ProjectileMotionWeb.Controllers
{
    public class SavingAndExportController : BaseController
    {
        public ActionResult Index()
        {
            if (!CheckForExistingMotion(out ActionResult redir))
            {
                return redir;
            }

            return RedirectToAction(nameof(Information));
        }

        public ActionResult Information()
        {
            if (!CheckForExistingMotion(out ActionResult redir))
            {
                return redir;
            }

            return View(
                new BaseModel()
                {
                    Layout = new LayoutModel("Saving and export")
                    {
                       Menu = new LayoutMenuModel ()
                       {
                           ActiveMenuItem = LayoutMenuModel.ActiveNavItem.MotionDropdown
                       }
                    }
                }
            );
        }

        public FileContentResult InfoToTxt()
        {
            if (!CheckForExistingMotion())
            {
                return null;
            }

            ProjectileMotion SavedMotion = GetSession().GetSavedProjectileMotionWithOrWithoutResistance();
            return new ExportHelper(SavedMotion.Settings.TxtInfoFileName, "text/plain", SavedMotion.Saving.InfoToTxtGetMemoryStream()).GetResultAsContentType();
        }


        public FileContentResult DataToCsv()
        {
            if (!CheckForExistingMotion())
            {
                return null;
            }

            ProjectileMotion SavedMotion = GetSession().GetSavedProjectileMotionWithOrWithoutResistance();
            return new ExportHelper(SavedMotion.Settings.CsvDataFileName, "application/CSV", SavedMotion.Saving.DataToCsvGetMemoryStream()).GetResultAsContentType();
        }


        public FileContentResult InfoToPdf()
        {
            if (!CheckForExistingMotion())
            {
                return null;
            }

            ProjectileMotion SavedMotion = GetSession().GetSavedProjectileMotionWithOrWithoutResistance();
            return new ExportHelper(SavedMotion.Settings.PdfInfoFileName, "application/pdf", SavedMotion.Saving.InfoToPdfGetMemoryStream()).GetResultAsContentType();
        }


        public enum ChartExportContentTypes
        {
            Pdf,
            Png,
            Jpg
        }


        [HttpPost]
        public FileContentResult ChartAsContentType (string canvasbase64ImageUrl, ChartExportContentTypes contentType)
        {
            if (!CheckForExistingMotion())
            {
                return null;
            }

            switch (contentType)
            {
                case ChartExportContentTypes.Png:
                    return new ExportHelperBase64Image(canvasbase64ImageUrl, FileUtilities.GetFileName(GetSession().GetSavedProjectileMotionWithOrWithoutResistance().Settings.ChartFileName, "png"), "image/png").GetResultAsContentType();
                case ChartExportContentTypes.Pdf:
                    return new ExportHelperBase64Image(canvasbase64ImageUrl, FileUtilities.GetFileName(GetSession().GetSavedProjectileMotionWithOrWithoutResistance().Settings.ChartFileName, "pdf")).GetResultAsPdf();
                case ChartExportContentTypes.Jpg:
                    return new ExportHelperBase64Image(canvasbase64ImageUrl, FileUtilities.GetFileName(GetSession().GetSavedProjectileMotionWithOrWithoutResistance().Settings.ChartFileName, "jpg")).GetResultAsJpg();
                default:
                    return null;
            }
        }

    }
}