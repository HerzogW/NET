using Microsoft.Portal.Framework;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Microsoft.Portal.Extensions.ProjectOxfordExtension.Controllers
{
    [Export(typeof(ExtensionDefinition))]
    internal class Definition : ExtensionDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Definition"/> class.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration.</param>
        [ImportingConstructor]
        public Definition(ApplicationConfiguration applicationConfiguration)
        {
            this.ExtensionConfiguration = new Dictionary<string, object>()
            {
                { "armEndpoint", applicationConfiguration.ArmEndpoint }
            };
        }

        public override bool TraceAjaxErrors
        {
            get
            {
                return true;
            }
        }

        public override string GetTitle(PortalRequestContext context)
        {
            return Client.ClientResources.ExtensionName;
        }
    }
}