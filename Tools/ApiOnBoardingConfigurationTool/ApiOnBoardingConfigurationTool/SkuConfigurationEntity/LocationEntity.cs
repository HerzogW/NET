

using System.Runtime.Serialization;

namespace ApiOnBoardingConfigurationTool.SkuConfigurationEntity
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class LocationEntity
    {
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        [DataMember(IsRequired = true)]
        public string location { get; set; }

        //可选
        /// <summary>
        /// Gets or sets the apim product identifier.
        /// </summary>
        /// <value>
        /// The apim product identifier.
        /// </value>
        [DataMember(EmitDefaultValue = false)]
        public string apimProductId { get; set; }
    }
}
