using System.Web.Mvc;

namespace DataManager.Controllers
{
    public class SystemController : DataManagerController
    {
        // GET: System
        public ActionResult Index()
        {
            Page.Title = TempData["Page.Title"] != null ? TempData["Page.Title"].ToString() : "System Administration";
            return View();
        }
    }
}