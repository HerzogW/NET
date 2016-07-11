
namespace ApiOnBoardingConfigurationTool.SkuConfigurationEntity
{
    using System.Collections.Generic;

    /// <summary>
    /// The SkuConfigEntity
    /// </summary>
    public class SkuConfigEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string displayName { get; set; }

        /// <summary>
        /// Gets or sets the apim instance.
        /// </summary>
        /// <value>
        /// The apim instance.
        /// </value>
        public string apimInstance { get; set; }

        /// <summary>
        /// Gets or sets the API path.
        /// </summary>
        /// <value>
        /// The API path.
        /// </value>
        public string apiPath { get; set; }

        /// <summary>
        /// Gets or sets the skus.
        /// </summary>
        /// <value>
        /// The skus.
        /// </value>
        public List<SkuEntity> skus { get; set; }
    }
}
