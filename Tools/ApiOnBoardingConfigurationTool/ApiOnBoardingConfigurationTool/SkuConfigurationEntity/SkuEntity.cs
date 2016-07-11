
namespace ApiOnBoardingConfigurationTool.SkuConfigurationEntity
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The SkuEntity
    /// </summary>
    [DataContract]
    public class SkuEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember(IsRequired = true)]
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the tier.
        /// </summary>
        /// <value>
        /// The tier.
        /// </value>
        [DataMember(IsRequired = true)]
        public string tier { get; set; }

        //可选
        /// <summary>
        /// Gets or sets the skutype.
        /// </summary>
        /// <value>
        /// The skutype.
        /// </value>
        [DataMember(EmitDefaultValue = false)]
        public string skutype { get; set; }

        //可选
        /// <summary>
        /// Gets or sets the skuquota.
        /// </summary>
        /// <value>
        /// The skuquota.
        /// </value>
        [DataMember(EmitDefaultValue = false)]
        public string skuquota { get; set; }

        //可选
        /// <summary>
        /// Gets or sets the apim product identifier.
        /// </summary>
        /// <value>
        /// The apim product identifier.
        /// </value>
        [DataMember(EmitDefaultValue = false)]
        public string apimProductId { get; set; }

        /// <summary>
        /// Gets or sets the locations.
        /// </summary>
        /// <value>
        /// The locations.
        /// </value>
        [DataMember(IsRequired = true)]
        public List<LocationEntity> locations { get; set; }

        /// <summary>
        /// Gets or sets the meter ids.
        /// </summary>
        /// <value>
        /// The meter ids.
        /// </value>
        [DataMember(IsRequired = true)]
        public List<string> meterIds { get; set; }

        //可选
        /// <summary>
        /// Gets or sets the required features.
        /// </summary>
        /// <value>
        /// The required features.
        /// </value>
        [DataMember(EmitDefaultValue = false)]
        public List<string> requiredFeatures { get; set; }
    }
}
