using sep4.IoTSimulator.WebSocket;
using sep4.Models.Stage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace sep4
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            WebSocketClient client = new WebSocketClient();
            WebSocketThread thread = new WebSocketThread();

            //Stage stage = new Stage();
            //stage.RemoveStage();
            //stage.RemoveDim();

            //stage.InitInsertIntoStage();
            //stage.initTransform();
            //stage.initLoad();
            //stage.initLoadFact();

            //stage.IncInsertIntoStage();
            //stage.incrTransform();
            //stage.IncrLoad();
            //stage.IncrLoadFact();

        }
    }
}
