﻿using System.ComponentModel.Composition;
using Microsoft.Portal.Framework;

namespace Microsoft.Portal.Extensions.AzurePortalExtension
{
    [Export(typeof(ApplicationContext))]
    internal class CustomApplicationContext : ApplicationContext
    {
        private ApplicationConfiguration configuration;

        [ImportingConstructor]
        public CustomApplicationContext(ApplicationConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override bool IsDevelopmentMode
        {
            get
            {
                return this.configuration.IsDevelopmentMode;
            }
        }

        public override string CdnPrefix
        {
            get
            {
                return this.configuration.CdnPrefix;
            }
        }
    }
}
