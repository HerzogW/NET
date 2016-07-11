
namespace ApiOnBoardingConfigurationTool.MeterConfigurationEntity
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class ApimMeterDefinitionEntity
    {
        //可选
        [DataMember(EmitDefaultValue = false)]
        public List<ApiEntity> apis { get; set; }
    }
}
