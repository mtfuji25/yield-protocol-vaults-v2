using DataManager.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace DataManager.Controllers
{
    public class ErrorController : DataManagerController
    {
        #region Methods
        public ErrorController()
        {
            Page.Id = "error";
            Meta.Robots("noarchive, noindex");
            Page.Title = "Whoops, an Unexpected Error Has Occured";
        }

        public ActionResult Http404(Exception ex)
        {
            Page.Title = "We’re Sorry, the Page You Requested Cannot Be Found";
            Response.StatusCode = 404;
            return View();
        }

        public ActionResult Index(Exception ex)
        {
            Response.StatusCode = (ex is HttpException) ? ((HttpException)ex).GetHttpCode() : 500;
            return View();
        }
        #endregion
    }
}