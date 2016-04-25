﻿

namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The Api configuration data class.
    /// </summary>
    public class ApiConfigurationData
    {
        /// <summary>
        /// The resource token
        /// </summary>
        private const string ResourceToken = "ms-resource:";

        /// <summary>
        /// The icons token
        /// </summary>
        private const string IconToken = "ms-icon:";

        /// <summary>
        /// Gets or sets the locale identifier.
        /// </summary>
        /// <value>
        /// The locale identifier.
        /// </value>
        public string LocaleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the API type.
        /// </summary>
        /// <value>
        /// The name of the API type.
        /// </value>
        [JsonProperty("item")]
        public string ApiTypeName { get; set; }

        /// <summary>
        /// Gets or sets the spec json.
        /// </summary>
        /// <value>
        /// The spec json.
        /// </value>
        public JObject Spec { get; set; }

        /// <summary>
        /// Gets or sets the quick starts json.
        /// </summary>
        /// <value>
        /// The quick starts json.
        /// </value>
        public JObject QuickStart { get; set; }

        /// <summary>
        /// Gets or sets the API item json.
        /// </summary>
        /// <value>
        /// The API item json.
        /// </value>
        public JObject ApiItem { get; set; }

        public List<string> listError = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiConfigurationData"/> class.
        /// </summary>
        public ApiConfigurationData()
        {
        }

        /// <summary>
        /// Gets the localized.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="localeId">The locale identifier.</param>
        /// <param name="resources">The resources.</param>
        /// <returns>The ApiConfigurationData Object.</returns>
        public ApiConfigurationData GetLocalized(ApiConfigurationData data, string localeId, Dictionary<string, string> resources, Dictionary<string, string> defaultResources = null)
        {
            return new ApiConfigurationData()
            {
                LocaleId = localeId,
                ApiTypeName = data.ApiTypeName,
                Spec = ReplaceJObject(ResourceToken, data.Spec, resources, defaultResources),
                ApiItem = ReplaceJObject(ResourceToken, data.ApiItem, resources, defaultResources),
                QuickStart = ReplaceJObject(ResourceToken, data.QuickStart, resources, defaultResources)
            };
        }

        /// <summary>
        /// Repairs the icons.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="icons">The icons.</param>
        /// <param name="localeId">The locale identifier.</param>
        public void ReplaceIcons(ApiConfigurationData data, Dictionary<string, string> icons)
        {
            data.Spec = ReplaceJObject(IconToken, data.Spec, icons);
            data.ApiItem = ReplaceJObject(IconToken, data.ApiItem, icons);
            data.QuickStart = ReplaceJObject(IconToken, data.QuickStart, icons);
        }

        /// <summary>
        /// Finds the resource.
        /// </summary>
        /// <param name="key">The original.</param>
        /// <param name="resources">The resources.</param>
        /// <returns>The resource string.</returns>
        private FindResourceResult FindResource(string key, Dictionary<string, string> resources, Dictionary<string, string> defaultResources = null)
        {
            if (resources.ContainsKey(key))
            {
                return new FindResourceResult { status = FindResourceResultStatus.Ok, resourceStr = resources[key] };
            }
            else if (defaultResources != null && defaultResources.ContainsKey(key))
            {
                return new FindResourceResult { status = FindResourceResultStatus.Ok, resourceStr = defaultResources[key] };
            }
            else
            {
                return new FindResourceResult { status = FindResourceResultStatus.Error, resourceStr = string.Format("{0} not found", key) };
                //throw new KeyNotFoundException(string.Format("{0} not found", key));
            }
        }

        /// <summary>
        /// Replaces the json.
        /// </summary>
        /// <param name="tokenFlag">The token flag.</param>
        /// <param name="original">The original spec.</param>
        /// <param name="resources">The resources.</param>
        /// <returns>The replaced Json.</returns>
        private JObject ReplaceJObject(string tokenFlag, JObject original, Dictionary<string, string> resources, Dictionary<string, string> defaultResources = null)
        {
            var result = JObject.FromObject(original);
            var reader = original.CreateReader();

            FindResourceResult findResourceResult;
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.String)
                {
                    var value = reader.Value as string;
                    if (value.StartsWith(tokenFlag))
                    {
                        findResourceResult = FindResource(value.Split(':')[1], resources, defaultResources);
                        if (findResourceResult.status == FindResourceResultStatus.Ok)
                        {
                            result.SelectToken(reader.Path).Replace(findResourceResult.resourceStr);
                        }
                        else
                        {
                            listError.Add(findResourceResult.resourceStr);
                        }
                    }
                }
            }

            return result;
        }
    }
}
