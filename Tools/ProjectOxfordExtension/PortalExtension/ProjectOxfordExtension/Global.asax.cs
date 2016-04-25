/********************************************************
*                                                        *
*   Copyright (c) Microsoft. All rights reserved.        *
*                                                        *
*********************************************************/

namespace Microsoft.Portal.Extensions.ProjectOxfordExtension
{
    using Microsoft.Portal.Extensions.ProjectOxfordExtension.Configuration;
    using Microsoft.Portal.Framework.ExtensionCore;
    using Microsoft.WindowsAzure;
    using System;
    using System.Runtime.InteropServices;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// An http application
    /// </summary>
    [ComVisible(false)]
    public class MvcApplication : ExtensionApplicationBase
    {
        /// <summary>
        /// The API configuration manager
        /// </summary>
        public static ApiConfigurationManager ApiConfigurationManager;

        /// <summary>
        /// The API configuration storage container
        /// </summary>
        public static string ApiConfigurationStorageContainer;

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

            InitializeExtension();
        }

        /// <summary>
        /// Extensions the initialize.
        /// </summary>
        private static void InitializeExtension()
        {
            ApiConfigurationStorageContainer = CloudConfigurationManager.GetSetting("ApiConfigurationContainer");
            var apiConfigurationPublicAccessUrl = CloudConfigurationManager.GetSetting("ApiConfigurationPublicAccessUrl");
            var apiConfigCacheRefreshInterval = TimeSpan.FromMinutes(int.Parse(CloudConfigurationManager.GetSetting("ApiConfigCacheRefreashTimeIntervalInMin")));

            ApiConfigurationManager = new ApiConfigurationManager(apiConfigurationPublicAccessUrl, ApiConfigurationStorageContainer, apiConfigCacheRefreshInterval);

            ApiConfigurationManager.LoadDataToCache();
        }
    }
}
