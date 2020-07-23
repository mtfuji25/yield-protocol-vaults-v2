using DataManager.Controllers;
using DataManager.Models;
using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DataManager
{
    public class MvcApplication : HttpApplication
    {
        #region Fields
        private static readonly Regex www = new Regex(@"www\.(?<mainDomain>.*)", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion

        #region Methods
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string host = Request.Headers["x-forwarded-host"];
            host = string.IsNullOrEmpty(host) ? Request.Url.Host : host;
            Match match = www.Match(host);
            if (match.Success)
            {
                string domain = match.Groups["mainDomain"].Value;
                UriBuilder builder = new UriBuilder(Request.Url)
                {
                    Host = domain
                };
                string uri = builder.Uri.ToString();
                Response.Clear();
                Response.StatusCode = 0x12d;
                Response.StatusDescription = "Moved Permanently";
                Response.AddHeader("Location", uri);
                Response.End();
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (!Context.IsDebuggingEnabled)
            {
                Exception lastError = Server.GetLastError();
                Response.Clear();
                Response.TrySkipIisCustomErrors = true;
                Server.ClearError();
                if ((lastError is HttpUnhandledException) && (lastError.InnerException != null))
                {
                    lastError = lastError.GetBaseException();
                }
                if (lastError != null)
                {
                    RouteData routeData = new RouteData();
                    routeData.Values.Add("controller", "Error");
                    if ((lastError is HttpException) && (((HttpException)lastError).GetHttpCode() == 404))
                    {
                        routeData.Values.Add("action", "Http404");
                    }
                    else
                    {
                        routeData.Values.Add("action", "Index");
                    }
                    routeData.Values.Add("ex", lastError);
                    IController controller = new ErrorController();
                    controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                }
            }
        }

        protected void Application_PreSendRequestHeaders()
        {
            // Cleanup unneeded headers to speed up page loading 
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
            Response.Headers.Remove("ETag");
        }

        protected void Application_Start()
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            if (!Context.IsDebuggingEnabled)
            {
                // Only start scheudled jobs if we are not in debug mode.
                JobScheduler.Start();
            }
        }
        #endregion
    }
}
