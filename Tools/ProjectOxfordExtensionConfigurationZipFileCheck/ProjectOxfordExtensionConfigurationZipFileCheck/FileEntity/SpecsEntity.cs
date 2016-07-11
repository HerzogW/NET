
namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class SpecsEntity
    {
        /// <summary>
        /// Gets or sets the type of the spec.
        /// </summary>
        /// <value>
        /// The type of the spec.
        /// </value>
        public string specType { get; set; }

        /// <summary>
        /// Gets or sets the specs.
        /// </summary>
        /// <value>
        /// The specs.
        /// </value>
        public List<SpecUnitEntity> specs { get; set; }

        /// <summary>
        /// Gets or sets the features.
        /// </summary>
        /// <value>
        /// The features.
        /// </value>
        public List<SpecFeatureEntity> features { get; set; }

        /// <summary>
        /// Gets or sets the resource map.
        /// </summary>
        /// <value>
        /// The resource map.
        /// </value>
        public SpecResourceMapEntity resourceMap { get; set; }

        /// <summary>
        /// Gets or sets the specs to allow zero cost.
        /// </summary>
        /// <value>
        /// The specs to allow zero cost.
        /// </value>
        public List<string> specsToAllowZeroCost { get; set; }
    }
}
