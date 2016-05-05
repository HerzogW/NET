using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    public class QuickStartsEntity
    {
        public List<QucikStartUnit> quickStarts { get; set; }
    }

    public class QucikStartUnit
    {
        public string title { get; set; }

        public string description { get; set; }

        public string icon { get; set; }

        public List<QuickStartLink> links { get; set; }
    }

    public class QuickStartLink
    {
        public string text { get; set; }
        public string uri { get; set; }
    }
}
