using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HocPhi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void dangnhap_Start()
        {
            Session["Admin"] = null;
            Session["AdHinh"] = null;
            Session["dangnhap"] = null;
            Session["ID"] = null;
            Session["getClass"] = null;
        }
    }
}
