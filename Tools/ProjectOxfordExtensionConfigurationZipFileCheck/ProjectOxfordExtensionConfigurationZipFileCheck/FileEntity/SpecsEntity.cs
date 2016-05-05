using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    public class SpecsEntity
    {
        public string specType { get; set; }

        public List<SpecUnitEntity> specs { get; set; }

        public List<SpecFeatureEntity> features { get; set; }



        public List<string> specsToAllowZeroCost { get; set; }
    }
}
