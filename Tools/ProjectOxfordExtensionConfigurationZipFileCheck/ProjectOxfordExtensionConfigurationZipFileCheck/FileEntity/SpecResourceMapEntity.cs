
namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class SpecResourceMapEntity
    {
        [DataMember(Name = "default")]
        public List<SpecResourceMapDefault> specResourceMapDefault { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SpecResourceMapDefault
    {
        public string id { get; set; }

        public List<SpecFirstParty> firstParty { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SpecFirstParty
    {
        public string resourceId { get; set; }

        public float quantity { get; set; }
    }
}
