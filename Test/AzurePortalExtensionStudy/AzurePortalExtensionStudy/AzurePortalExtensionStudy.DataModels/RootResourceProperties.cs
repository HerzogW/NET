using Microsoft.Portal.TypeMetadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Portal.Extensions.AzurePortalExtensionStudy.DataModels
{
    [TypeMetadataModel(typeof(RootResourceProperties), "AzurePortalExtensionStudy.DataModels")]
    public class RootResourceProperties
    {
        public string CustomProperty { get; set; }
        public string ProvisioningState { get; set; }
    }
}
