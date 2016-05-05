using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    public class ApiEntity
    {
        public string item { get; set; }

        public string title { get; set; }

        public string subtitle { get; set; }

        public string iconData { get; set; }

        public List<string> categories { get; set; }

        public List<ApiSkuQuotaEntity> skuQuota { get; set; }

        public bool ApiSkuQuotaEntity { get; set; }
    }

    public class ApiSkuQuotaEntity
    {
        public string code { get; set; }

        public string name { get; set; }

        public int quota { get; set; }
    }
}
