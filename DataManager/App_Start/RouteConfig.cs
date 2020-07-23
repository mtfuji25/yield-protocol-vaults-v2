using System.Web.Mvc;
using System.Web.Routing;

namespace DataManager
{
    public class RouteConfig
    {
        #region Methods
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Clear();
            routes.RouteExistingFiles = false;
            routes.IgnoreRoute("assets/img/{*pathInfo}");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = "(.*/)?favicon.([iI][cC][oO]|[gG][iI][fF])(/.*)?" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{guid}",
                defaults: new { controller = "Home", action = "Index", guid = UrlParameter.Optional }
            );
        }
        #endregion
    }
}
