
namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class QuickStartsEntity
    {
        public List<QucikStartUnit> quickStarts { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class QucikStartUnit
    {
        public string title { get; set; }

        public string description { get; set; }

        public string icon { get; set; }

        public List<QuickStartLink> links { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class QuickStartLink
    {
        public string text { get; set; }
        public string uri { get; set; }
    }
}
