using DataManager.Models;
using System.Web.Mvc;

namespace DataManager.Controllers
{
    [Authorize]
    public class SearchController : DataManagerController
    {
        #region Methods
        public ActionResult Index()
        {
            Page.Title = TempData["Page.Title"] != null ? base.TempData["Page.Title"].ToString() : "Search Index";
            return View();
        }

        [HttpPost]
        public ActionResult Rebuild()
        {
            Search.Index.All();
            TempData["Page.Title"] = "The Search Index Was Queued For Rebuilding Successfully!";
            return RedirectToAction("Index");
        }
        #endregion
    }
}