
namespace ApiOnBoardingConfigurationTool.MeterConfigurationEntity
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ApiEntity
    {
        /// <summary>
        /// Gets or sets the API identifier.
        /// </summary>
        /// <value>
        /// The API identifier.
        /// </value>
        [DataMember(IsRequired = true)]
        public string apiId { get; set; }

        //可选
        /// <summary>
        /// Gets or sets the operations.
        /// </summary>
        /// <value>
        /// The operations.
        /// </value>
        [DataMember(EmitDefaultValue = false)]
        public List<string> operations { get; set; }
    }
}
