namespace Microsoft.Portal.Extensions.ProjectOxfordExtension.DataModels
{
    using Microsoft.Portal.TypeMetadata;

    [TypeMetadataModel(typeof(AccountProperties), "ProjectOxfordExtension.DataModels")]
    public class AccountProperties
    {
        public string ProvisioningState { get; set; }

        public string Endpoint { get; set; }
    }

    [TypeMetadataModel(typeof(Sku), "ProjectOxfordExtension.DataModels")]
    public class Sku
    {
        public string Name { get; set; }
    }
}
