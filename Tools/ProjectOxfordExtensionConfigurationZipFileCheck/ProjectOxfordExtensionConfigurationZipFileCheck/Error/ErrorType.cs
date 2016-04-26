using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    public enum ErrorType
    {
        None,
        Common,
        NotFound,
        CanNotConvertToJson,
        CanNotDeserialize,
        LostResource,

    }
}
