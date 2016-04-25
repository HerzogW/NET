/********************************************************
*                                                        *
*   Copyright (c) Microsoft. All rights reserved.        *
*                                                        *
*********************************************************/

namespace Microsoft.Portal.Extensions.ProjectOxfordExtension.Controllers
{
    using Configuration;
    using DataModels;
    using Microsoft.Portal.Framework;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Http;

    /// <summary>
    /// Api configuration controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class ApiConfigurationController : ApiController
    {
        /// <summary>
        /// The default locale identifier
        /// </summary>
        public const string DefaultLocaleId = "en";

        /// <summary>
        /// Gets the API item.
        /// </summary>
        /// <param name="apiName">Name of the API.</param>
        /// <returns>
        /// The Json Object.
        /// </returns>
        public JObject GetApiItem(string apiName)
        {
            return GetConfigurationDataByApiName(apiName).ApiItem;
        }

        /// <summary>
        /// Gets all API items.
        /// </summary>
        /// <returns>
        /// The Json Object.
        /// </returns>
        public List<JObject> GetAllApiItems()
        {
            var result = new List<JObject>();
            var localeId = Request.Headers.AcceptLanguage.First().Value.ToLower();

            foreach (var config in MvcApplication.ApiConfigurationManager.DataCache)
            {
                ApiConfigurationData data = null;
                if (config.Value.TryGetValue(localeId, out data))
                {
                    result.Add(data.ApiItem);
                }
                else if(config.Value.TryGetValue(ApiConfigurationManager.DefaultLanguage, out data))
                {
                    result.Add(data.ApiItem);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the API specs.
        /// </summary>
        /// <param name="apiName">Name of the API.</param>
        /// <returns>
        /// The Json Object.
        /// </returns>
        public JObject GetApiSpecs(string apiName)
        {
            return GetConfigurationDataByApiName(apiName).Spec;
        }

        /// <summary>
        /// Gets the API quick start.
        /// </summary>
        /// <param name="apiName">Name of the API.</param>
        /// <returns>
        /// The Json Object.
        /// </returns>
        public JObject GetApiQuickStart(string apiName)
        {
            return GetConfigurationDataByApiName(apiName).QuickStart;
        }

        /// <summary>
        /// Gets the name of the configuration data by API.
        /// </summary>
        /// <param name="apiName">Name of the API.</param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        private ApiConfigurationData GetConfigurationDataByApiName(string apiName)
        {
            var localeId = Request.Headers.AcceptLanguage.First().Value.ToLower();
            Dictionary<string, ApiConfigurationData> apiLocaleMap = null;

            if (MvcApplication.ApiConfigurationManager.DataCache.TryGetValue(apiName, out apiLocaleMap))
            {
                ApiConfigurationData data = null;

                if (apiLocaleMap.TryGetValue(localeId, out data))
                {
                    return data;
                }
                else if (apiLocaleMap.TryGetValue(ApiConfigurationManager.DefaultLanguage, out data))
                {
                    return data;
                }
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);           
        }
    }
}