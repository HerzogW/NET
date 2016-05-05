using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    [DataContract]
    public class SpecResourceMapEntity
    {
        [DataMember(Name = "default")]
        public List<SpecResourceMapDefault> specResourceMapDefault { get; set; }
    }


    public class SpecResourceMapDefault
    {
        public string id { get; set; }

        public SpecFirstParty firstParty { get; set; }
    }

    public class SpecFirstParty
    {
        public string resourceId { get; set; }

        public float quantity { get; set; }
    }
}
