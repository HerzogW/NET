using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    public class SpecUnitEntity
    {
        public string id { get; set; }
        public string colorScheme { get; set; }

        public string title { get; set; }

        public string specCode { get; set; }

        public List<SpecPromotedFeature> promotedFeatures { get; set; }

        public List<SpecFeatureUnit> features { get; set; }

        public SpecCost cost {get;set;}
    }

    public class SpecPromotedFeature
    {
        public string value { get; set; }

        public string unitDescription { get; set; }
    }

    public class SpecFeatureUnit
    {
        public string id { get; set; }
    }

    public class SpecCost
    {
        public float amount { get; set; }

        public string currencyCode { get; set; }

        public string caption { get; set; }
    }
}
