using Microsoft.Portal.TypeMetadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Portal.Extensions.AzurePortalExtensionStudy.DataModels
{
    [TypeMetadataModel(typeof(RootResource), "AzurePortalExtensionStudy.DataModels", true)]
    public class RootResource : ResourceBase
    {
        public RootResourceProperties Properties { get; set; }
    }
}
