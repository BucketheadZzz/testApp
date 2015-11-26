using System.Web.Mvc;
using System.Web.Routing;

namespace TestApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "NewsList",
            url: "{controller}/{action}",
            defaults: new { controller = "News", action = "List" }
            );

            routes.MapRoute(
            name: "NewsListAdmin",
            url: "{controller}/{action}",
            defaults: new { controller = "News", action = "ListAdmin" }
            );

            routes.MapRoute(
            name: "NewsItemEdit",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "News", action = "Edit", id = UrlParameter.Optional }
            );


            routes.MapRoute(
            name: "NewsTagsWidget",
            url: "{controller}/{action}",
            defaults: new { controller = "News", action = "NewsTagsWidget" }
            );



         

            routes.MapRoute(
            name: "PlaylistList",
            url: "{controller}/{action}",
            defaults: new { controller = "Playlist", action = "List" }
            );

            routes.MapRoute(
            name: "PlaylistListAdmin",
            url: "{controller}/{action}",
            defaults: new { controller = "Playlist", action = "ListAdmin" }
            );

            routes.MapRoute(
            name: "PlaylistItemEdit",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Playlist", action = "Edit", id = UrlParameter.Optional }
            );



        }
    }
}