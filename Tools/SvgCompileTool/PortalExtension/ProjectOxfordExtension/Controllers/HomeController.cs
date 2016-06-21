using Microsoft.Portal.Framework;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Web.Mvc;

namespace Microsoft.Portal.Extensions.ProjectOxfordExtension.Controllers
{
    /// <summary>
    /// Home controller
    /// </summary>
    [ComVisible(false)]
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HomeController : ExtensionControllerBase
    {
        private ApplicationConfiguration settings;
        /// <summary>
        /// Initializes a new instance of the HomeController class.
        /// </summary>
        [ImportingConstructor]
        public HomeController(ExtensionDefinition definition, ApplicationConfiguration settings)
            : base(definition)
        {
            this.settings = settings;
        }
#if DEBUG
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (settings.IsDevelopmentMode && Request.QueryString.Count == 0 && filterContext != null && filterContext.ActionDescriptor.ActionName == "Index")
            {
                // IIS Express and the VS debug are shutdown when using a redirect to side load the extension using Test in Prod (TiP) functionality.
                // Instead of using a redirect keep the original window open on localhost:<port> and open a new one for TiP
                //&microsoft_azure_projectOxford_requestsettingsenabled=false&microsoft_azure_projectOxford_troubleshootsettingsenabled=false
                var keepDebuggerAttached = string.Format("<script>window.open('{0}/?Microsoft_Azure_ProjectOxford=true&microsoft_azure_marketplace_ItemHideKey=Microsoft_Azure_ProjectOxford&feature.customportal=false&clientOptimizations=false&feature.canmodifyextensions=true#?testExtensions={{\"Microsoft_Azure_ProjectOxford\":\"{1}\"}}');</script>", this.settings.TestInProdEnvironment, Request.Url.ToString());
                filterContext.HttpContext.Response.Write(keepDebuggerAttached);
                base.OnActionExecuted(filterContext);
            }
        }
#endif
    }
}