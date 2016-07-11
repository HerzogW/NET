
namespace ApiOnBoardingConfigurationTool.MeterConfigurationEntity
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class MeterConfigEntity
    {
        [DataMember(IsRequired = true)]
        public string id { get; set; }

        //可选
        [DataMember(EmitDefaultValue = false)]
        public string callcountPerUnit { get; set; }

        [DataMember(IsRequired = true)]
        public string commerceMeterId { get; set; }

        //可选
        [DataMember(EmitDefaultValue = false)]
        public string Type { get; set; }

        //可选
        [DataMember(EmitDefaultValue = false)]
        public string UOM { get; set; }

        //可选
        [DataMember(EmitDefaultValue = false)]
        public string Cadence { get; set; }

        //可选
        [DataMember(EmitDefaultValue = false)]
        public ApimMeterDefinitionEntity apimMeterDefinition { get; set; }
    }
}
