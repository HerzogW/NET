
namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class SpecsEntity
    {
        public string specType { get; set; }

        public List<SpecUnitEntity> specs { get; set; }

        public List<SpecFeatureEntity> features { get; set; }



        public List<string> specsToAllowZeroCost { get; set; }
    }
}
