
namespace ApiOnBoardingConfigurationTool
{
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class SpecFeatureEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember(IsRequired = true)]
        public string id { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        [DataMember(IsRequired = true)]
        public string displayName { get; set; }

        /// <summary>
        /// Gets or sets the icon SVG data.
        /// </summary>
        /// <value>
        /// The icon SVG data.
        /// </value>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string iconSvgData { get; set; }

        /// <summary>
        /// Gets or sets the name of the icon.
        /// </summary>
        /// <value>
        /// The name of the icon.
        /// </value>
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string iconName { get; set; }
    }
}
