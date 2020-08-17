using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BlogSystem.MVC.Filters
{
    public class BlogSystemAuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // 当用户存储在cookie中 而且session数据为空时
            // 将cookie数据同步到session中
            if(filterContext.HttpContext.Request.Cookies["loginName"] != null
                && filterContext.HttpContext.Session["loginName"] == null)
            {
                filterContext.HttpContext.Session["loginName"] = filterContext.HttpContext.Request.Cookies["loginName"].Value;
                filterContext.HttpContext.Session["userId"] = filterContext.HttpContext.Request.Cookies["userId"].Value;
            };

                //base.OnAuthorization(filterContext);
                if (!(filterContext.HttpContext.Session["loginName"] != null 
                || filterContext.HttpContext.Request.Cookies["loginName"] != null))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary()
                {
                    {"controller", "Home"},
                    {"action", "Login"}
                });
            }
        }
    }
}