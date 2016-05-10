

namespace ProjectOxfordExtensionConfigurationZipFileCheck
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System;

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
        /// The setting format
        /// </summary>
        private JsonSerializerSettings settingFormat = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

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

        /// <summary>
        /// The icons
        /// </summary>
        public Dictionary<string, string> Icons = new Dictionary<string, string>();

        /// <summary>
        /// The resources
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> Resources = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// The list error
        /// </summary>
        public List<ErrorEntity> listError = new List<ErrorEntity>();

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
                Spec = ReplaceJObject(ResourceToken, data.Spec, resources, defaultResources, localeId),
                ApiItem = ReplaceJObject(ResourceToken, data.ApiItem, resources, defaultResources, localeId),
                QuickStart = ReplaceJObject(ResourceToken, data.QuickStart, resources, defaultResources, localeId)
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
        /// Replaces the json.
        /// </summary>
        /// <param name="tokenFlag">The token flag.</param>
        /// <param name="original">The original spec.</param>
        /// <param name="resources">The resources.</param>
        /// <returns>The replaced Json.</returns>
        private JObject ReplaceJObject(string tokenFlag, JObject original, Dictionary<string, string> resources, Dictionary<string, string> defaultResources = null, string localeId = "")
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
                            result.SelectToken(reader.Path).Replace(findResourceResult.resourceInfo);
                        }
                        else
                        {
                            listError.Add(new ErrorEntity()
                            {
                                errorType = ErrorType.LostResource,
                                jsonFileName = string.Format("{0}/resources.resjson", localeId),
                                resourceName = findResourceResult.resourceInfo,
                            });
                        }
                    }
                }
            }

            return result;
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
                return new FindResourceResult { status = FindResourceResultStatus.Ok, resourceInfo = resources[key] };
            }
            else if (defaultResources != null && defaultResources.ContainsKey(key))
            {
                return new FindResourceResult { status = FindResourceResultStatus.Ok, resourceInfo = defaultResources[key] };
            }
            else
            {
                return new FindResourceResult { status = FindResourceResultStatus.NotFound, resourceInfo = key };
            }
        }

        /// <summary>
        /// Handles the items.
        /// </summary>
        public void HandleItems()
        {
            HandleApiItem();
            handleSpecItem();
            handleQuickStartItem();
        }

        /// <summary>
        /// Handles the API item.
        /// </summary>
        private void HandleApiItem()
        {
            var resourceEn = new Dictionary<string, string>();

            try
            {
                ApiEntity apiEntity = JsonConvert.DeserializeObject<ApiEntity>(this.ApiItem.ToString());
                resourceEn.Add("title", apiEntity.title);
                apiEntity.title = string.Format("{0}title", ResourceToken);

                resourceEn.Add("subtitle", apiEntity.subtitle);
                apiEntity.subtitle = string.Format("{0}subtitle", ResourceToken);

                Icons.Add(string.Format("{0}.svg", apiEntity.item), apiEntity.iconData);
                apiEntity.iconData = string.Format("{0}{1}.svg", IconToken, apiEntity.item);

                this.ApiItem = JObject.Parse(JsonConvert.SerializeObject(apiEntity, settingFormat));
                this.Resources.Add("en", resourceEn);
            }
            catch (Exception ex)
            {
                ErrorEntity entity = new ErrorEntity();
                entity.errorType = ErrorType.Common;
                entity.errorMessage = ex.Message;

                this.listError.Add(entity);
            }
        }

        /// <summary>
        /// Handles the spec item.
        /// </summary>
        private void handleSpecItem()
        {
            var resourceEn = this.Resources["en"];
            try
            {
                SpecsEntity specsEntity = JsonConvert.DeserializeObject<SpecsEntity>(this.Spec.ToString());
                for (int i = 0; i < specsEntity.specs.Count; i++)
                {
                    var entity = specsEntity.specs[i];
                    resourceEn.Add(string.Format("spec.{0}.title", entity.id.ToLower()), entity.title);
                    specsEntity.specs[i].title = string.Format("{0}spec.{1}.title", ResourceToken, entity.id.ToLower());

                    for (int j = 0; j < entity.promotedFeatures.Count; j++)
                    {
                        var feature = entity.promotedFeatures[j];

                        string key = GetKeyByValue(resourceEn, feature.unitDescription);
                        if (!string.IsNullOrWhiteSpace(key))
                        {
                            specsEntity.specs[i].promotedFeatures[j].unitDescription = string.Format("{0}{1}", ResourceToken, key);
                        }
                        else
                        {
                            int num = 1;
                            while (true)
                            {
                                string newKey = string.Format("spec.promotedFeature.unitDescription{0}", num++);
                                if (!ExistKey(resourceEn, newKey))
                                {
                                    resourceEn.Add(newKey, feature.unitDescription);
                                    specsEntity.specs[i].promotedFeatures[j].unitDescription = string.Format("{0}{1}", ResourceToken, newKey);
                                    break;
                                }
                            }
                        }
                    }

                    resourceEn.Add(string.Format("spec.cost.{0}.caption.format", entity.id.ToLower()), entity.cost.caption);
                    specsEntity.specs[i].cost.caption = string.Format("{0}spec.cost.{1}.caption.format", ResourceToken, entity.id.ToLower());
                }

                for (int i = 0; i < specsEntity.features.Count; i++)
                {
                    var entity = specsEntity.features[i];
                    resourceEn.Add(string.Format("feature.{0}.displayName", entity.id), entity.displayName);
                    specsEntity.features[i].displayName = string.Format("{0}feature.{1}.displayName", ResourceToken, entity.id);

                    Icons.Add(string.Format("spec.feature.{0}.svg", entity.id), entity.iconSvgData);
                    specsEntity.features[i].iconSvgData = string.Format("{0}spec.feature.{1}.svg", IconToken, entity.id);
                }

                this.Spec = JObject.Parse(JsonConvert.SerializeObject(specsEntity, settingFormat));
                this.Resources["en"] = resourceEn;
            }
            catch (Exception ex)
            {
                ErrorEntity entity = new ErrorEntity();
                entity.errorType = ErrorType.Common;
                entity.errorMessage = ex.Message;

                this.listError.Add(entity);
            }
        }

        /// <summary>
        /// Handles the quick start item.
        /// </summary>
        private void handleQuickStartItem()
        {
            var resourceEn = this.Resources["en"];
            try
            {
                QuickStartsEntity quickStartsEntity = JsonConvert.DeserializeObject<QuickStartsEntity>(this.QuickStart.ToString());

                for (int i = 0; i < quickStartsEntity.quickStarts.Count; i++)
                {
                    var entity = quickStartsEntity.quickStarts[i];

                    resourceEn.Add(string.Format("quickStart{0}.title", i), entity.title);
                    quickStartsEntity.quickStarts[i].title = string.Format("{0}quickStart{1}.title", ResourceToken, i);

                    resourceEn.Add(string.Format("quickStart{0}.des", i), entity.description);
                    quickStartsEntity.quickStarts[i].description = string.Format("{0}quickStart{1}.des", ResourceToken, i);

                    for (int j = 0; j < entity.links.Count; j++)
                    {
                        var link = entity.links[j];

                        resourceEn.Add(string.Format("quickStart{0}.link{1}.text", i, j), link.text);
                        quickStartsEntity.quickStarts[i].links[j].text = string.Format("{0}quickStart{1}.link{2}.text", ResourceToken, i, j);
                    }
                }

                this.QuickStart = JObject.Parse(JsonConvert.SerializeObject(quickStartsEntity, settingFormat));
                this.Resources["en"] = resourceEn;
            }
            catch (Exception ex)
            {
                ErrorEntity entity = new ErrorEntity();
                entity.errorType = ErrorType.Common;
                entity.errorMessage = ex.Message;

                this.listError.Add(entity);
            }
        }

        /// <summary>
        /// Gets the key by value.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private string GetKeyByValue(Dictionary<string, string> resource, string value)
        {
            foreach (var dic in resource)
            {
                if (dic.Value.Equals(value))
                {
                    return dic.Key;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Exists the key.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private bool ExistKey(Dictionary<string, string> resource, string key)
        {
            foreach (var dic in resource)
            {
                if (dic.Key.Equals(key))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
