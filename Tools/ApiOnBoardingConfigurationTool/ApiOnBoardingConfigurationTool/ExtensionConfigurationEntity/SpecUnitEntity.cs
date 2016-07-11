
namespace ApiOnBoardingConfigurationTool
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class SpecUnitEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string id { get; set; }

        /// <summary>
        /// Gets or sets the color scheme.
        /// </summary>
        /// <value>
        /// The color scheme.
        /// </value>
        public string colorScheme { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string title { get; set; }

        /// <summary>
        /// Gets or sets the spec code.
        /// </summary>
        /// <value>
        /// The spec code.
        /// </value>
        public string specCode { get; set; }

        /// <summary>
        /// Gets or sets the promoted features.
        /// </summary>
        /// <value>
        /// The promoted features.
        /// </value>
        public List<SpecPromotedFeature> promotedFeatures { get; set; }

        /// <summary>
        /// Gets or sets the features.
        /// </summary>
        /// <value>
        /// The features.
        /// </value>
        public List<SpecFeatureUnit> features { get; set; }

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>
        /// The cost.
        /// </value>
        public SpecCost cost { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SpecPromotedFeature
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string value { get; set; }

        /// <summary>
        /// Gets or sets the unit description.
        /// </summary>
        /// <value>
        /// The unit description.
        /// </value>
        public string unitDescription { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SpecFeatureUnit
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string id { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class SpecCost
    {
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        [DataMember(EmitDefaultValue = false)]
        public float? amount { get; set; }

        /// <summary>
        /// Gets or sets the currency code.
        /// </summary>
        /// <value>
        /// The currency code.
        /// </value>
        [DataMember(EmitDefaultValue = false)]
        public string currencyCode { get; set; }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>
        /// The caption.
        /// </value>
        [DataMember(IsRequired = true)]
        public string caption { get; set; }
    }
}
