/********************************************************
*                                                        *
*   Copyright (c) Microsoft. All rights reserved.        *
*                                                        *
*********************************************************/

namespace Microsoft.Portal.Extensions.ProjectOxfordExtension.Configuration
{
    using Microsoft.Portal.Extensions.ProjectOxfordExtension.DataModels;
    using System.Collections.Generic;

    /// <summary>
    /// The Api configuration view interface.
    /// </summary>
    public interface IApiConfigurationManager
    {
        /// <summary>
        /// Gets Api configuration data cache. Dictionary[apiName, Dictionary[localeId, ApiConfigurationData]]
        /// </summary>
        /// <value>
        /// The data cache.
        /// </value>
        Dictionary<string, Dictionary<string, ApiConfigurationData>> DataCache { get; }

        /// <summary>
        /// Loads the data to cache.
        /// </summary>
        void LoadDataToCache();
    }
}