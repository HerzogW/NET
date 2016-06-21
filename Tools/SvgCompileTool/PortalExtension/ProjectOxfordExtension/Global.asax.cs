//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.Portal.Framework.ExtensionCore;
using System.Runtime.InteropServices;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Microsoft.Portal.Extensions.ProjectOxfordExtension
{
    /// <summary>
    /// An http application
    /// </summary>
    [ComVisible(false)]
    public class MvcApplication : ExtensionApplicationBase
    {
        /// <summary>
        /// Calls when the application starts for the first time
        /// </summary>
        protected override void ApplicationStartHandler()
        {
            // remove the below call to the base method if you do not want to register the <c>PrecompiledMvcViewEngine</c> view engine.
            base.ApplicationStartHandler();

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
