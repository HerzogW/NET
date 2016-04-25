using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    public class FindResourceResult
    {
        public FindResourceResultStatus status { get; set; }

        public string resourceInfo { get; set; }
    }

    public enum FindResourceResultStatus
    {
        Ok,
        NotFound
    }
}
