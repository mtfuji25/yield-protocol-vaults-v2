using System.Web.Mvc;

namespace DataManager.Controllers
{
    [Authorize]
    public class HomeController : DataManagerController
    {
        #region Methods
        public ActionResult Index()
        {
            return View();
        }
        #endregion
    }
}