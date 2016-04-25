namespace Microsoft.Portal.Extensions.ProjectOxfordExtension.DataModels
{
    using Microsoft.Portal.TypeMetadata;

    [TypeMetadataModel(typeof(Account), "ProjectOxfordExtension.DataModels", true)]
    public class Account : ResourceBase
    {
        public AccountProperties Properties { get; set; }

        public Sku Sku { get; set; }
    }
}
