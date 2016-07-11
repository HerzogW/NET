
namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class ApiEntity
    {
        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public string item { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string title { get; set; }

        /// <summary>
        /// Gets or sets the subtitle.
        /// </summary>
        /// <value>
        /// The subtitle.
        /// </value>
        public string subtitle { get; set; }

        /// <summary>
        /// Gets or sets the icon data.
        /// </summary>
        /// <value>
        /// The icon data.
        /// </value>
        public string iconData { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        public List<string> categories { get; set; }

        /// <summary>
        /// Gets or sets the sku quota.
        /// </summary>
        /// <value>
        /// The sku quota.
        /// </value>
        public List<ApiSkuQuotaEntity> skuQuota { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show legal term].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show legal term]; otherwise, <c>false</c>.
        /// </value>
        public bool showLegalTerm { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ApiSkuQuotaEntity
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string code { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the quota.
        /// </summary>
        /// <value>
        /// The quota.
        /// </value>
        public int quota { get; set; }
    }
}
