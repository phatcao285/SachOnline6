using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace sachOnline_Binh
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                           name: "Trang chu",
                           url: "",
                           defaults: new { controller = "SachOnline", action = "Index", id = UrlParameter.Optional }
                       );

            routes.MapRoute(
                name: "Sach theo Chu de",
                url: "sach-theo-chu-de-{MaCD}",
                defaults: new { controller = "SachOnline", action = "BookByTopic", MaCD = UrlParameter.Optional },
                namespaces: new string[] { "WebExample.Controllers" }
            );

            routes.MapRoute(
               name: "Sach theo NXB",
               url: "sach-theo-nxb-{MaNXB}",
               defaults: new { controller = "SachOnline", action = "BookByPublisher", MaNXB = UrlParameter.Optional },
               namespaces: new string[] { "WebExample.Controllers" }
           );

            routes.MapRoute(
                name: "Chi tiet sach",
                url: "chi-tiet-sach-{MaSach}",
                defaults: new { controller = "SachOnline", action = "BookDetail", MaSach = UrlParameter.Optional },
                namespaces: new string[] { "WebExample.Controllers" }
            );

            routes.MapRoute(
                name: "Dang ky",
                url: "dang-ky",
                defaults: new { controller = "User", action = "Register" },
                namespaces: new string[] { "WebExample.Controllers" }
            );
            routes.MapRoute(
                name: "Dang nhap",
                url: "dang-nhap",
                defaults: new { controller = "User", action = "Login", url = UrlParameter.Optional },
                namespaces: new string[] { "WebExample.Controllers" }
            );

            routes.MapRoute(
                name: "Trang tin",
                url: "{metatitle}",
                defaults: new { controller = "SachOnline", action = "TrangTin", metatitle = UrlParameter.Optional },
                namespaces: new string[] { "WebExample.Controllers" }
            );

            // Đường dẫn mặc định cho controller và action
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
