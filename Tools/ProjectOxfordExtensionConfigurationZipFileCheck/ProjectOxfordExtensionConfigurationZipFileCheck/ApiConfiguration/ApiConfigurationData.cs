﻿

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
            NullValueHandling = NullValueHandling.Include,
            MissingMemberHandling = MissingMemberHandling.Error
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
        /// <param name="existResourceFile">if set to <c>true</c> [exist resource file].</param>
        private void HandleApiItem()
        {
            try
            {
                ApiEntity apiEntity = JsonConvert.DeserializeObject<ApiEntity>(this.ApiItem.ToString());

                apiEntity.title = HandleSpecificItemResource("api.json", "title", "title", apiEntity.title);
                apiEntity.subtitle = HandleSpecificItemResource("api.json", "subtitle", "subtitle", apiEntity.subtitle);

                apiEntity.iconData = HandleSpecificItemIcon("api.json", "iconData", string.Format("{0}.svg", apiEntity.item), apiEntity.iconData);

                this.ApiItem = JObject.Parse(JsonConvert.SerializeObject(apiEntity, settingFormat));
            }
            catch (Exception ex)
            {
                ErrorEntity entity = new ErrorEntity();
                entity.errorType = ErrorType.Common;
                entity.exceptionMessage = ex.Message;

                this.listError.Add(entity);
            }
        }

        /// <summary>
        /// Handles the spec item.
        /// </summary>
        /// <param name="existResourceFile">if set to <c>true</c> [exist resource file].</param>
        private void handleSpecItem()
        {
            try
            {
                string propertyDetail = string.Empty;
                SpecsEntity specsEntity = JsonConvert.DeserializeObject<SpecsEntity>(this.Spec.ToString());
                for (int i = 0; i < specsEntity.specs.Count; i++)
                {
                    var entity = specsEntity.specs[i];
                    propertyDetail = string.Format("specs[{0}].title", i);
                    specsEntity.specs[i].title = HandleSpecificItemResource("spec.json", "title", string.Format("spec.{0}.title", entity.id.ToLower()), specsEntity.specs[i].title, true, propertyDetail);

                    for (int j = 0; j < entity.promotedFeatures.Count; j++)
                    {
                        var feature = entity.promotedFeatures[j];
                        propertyDetail = string.Format("specs[{0}].promotedFeatures[{1}].unitDescription", i, j);
                        specsEntity.specs[i].promotedFeatures[j].unitDescription = HandleSpecificItemSpecialResource("spec.json", "unitDescription", "spec.promotedFeature.unitDescription", specsEntity.specs[i].promotedFeatures[j].unitDescription, true, propertyDetail);
                    }
                    propertyDetail = string.Format("specs[{0}].caption", i);
                    specsEntity.specs[i].cost.caption = HandleSpecificItemResource("spec.json", "caption", string.Format("spec.cost.{0}.caption.format", entity.id.ToLower()), specsEntity.specs[i].cost.caption);
                }

                for (int i = 0; i < specsEntity.features.Count; i++)
                {
                    var entity = specsEntity.features[i];
                    propertyDetail = string.Format("features[{0}].displayName", i);
                    specsEntity.features[i].displayName = HandleSpecificItemResource("spec.json", "displayName", string.Format("feature.{0}.displayName", entity.id), specsEntity.features[i].displayName, true, propertyDetail);
                    propertyDetail = string.Format("features[{0}].iconSvgData", i);
                    specsEntity.features[i].iconSvgData = HandleSpecificItemIcon("spec.json", "iconSvgData", string.Format("spec.feature.{0}.svg", entity.id), specsEntity.features[i].iconSvgData, false, propertyDetail);
                }

                this.Spec = JObject.Parse(JsonConvert.SerializeObject(specsEntity, settingFormat));
            }
            catch (Exception ex)
            {
                ErrorEntity entity = new ErrorEntity();
                entity.errorType = ErrorType.Common;
                entity.exceptionMessage = ex.Message;

                this.listError.Add(entity);
            }
        }

        /// <summary>
        /// Handles the quick start item.
        /// </summary>
        /// <param name="existResourceFile">if set to <c>true</c> [exist resource file].</param>
        private void handleQuickStartItem()
        {
            try
            {
                QuickStartsEntity quickStartsEntity = JsonConvert.DeserializeObject<QuickStartsEntity>(this.QuickStart.ToString());
                string propertyDetail = string.Empty;
                for (int i = 0; i < quickStartsEntity.quickStarts.Count; i++)
                {
                    var entity = quickStartsEntity.quickStarts[i];
                    propertyDetail = string.Format("quickStarts[{0}].title", i);
                    quickStartsEntity.quickStarts[i].title = HandleSpecificItemResource("quickStarts.json", "title", string.Format("quickStart{0}.title", i + 1), quickStartsEntity.quickStarts[i].title, true, propertyDetail);

                    propertyDetail = string.Format("quickStarts[{0}].description", i);
                    quickStartsEntity.quickStarts[i].description = HandleSpecificItemResource("quickStarts.json", "description", string.Format("quickStart{0}.des", i + 1), quickStartsEntity.quickStarts[i].description, true, propertyDetail);

                    for (int j = 0; j < entity.links.Count; j++)
                    {
                        var link = entity.links[j];
                        propertyDetail = string.Format("quickStarts[{0}].links[{1}].text", i, j);
                        quickStartsEntity.quickStarts[i].links[j].text = HandleSpecificItemResource("quickStarts.json", "text", string.Format("quickStart{0}.link{1}.text", i + 1, j + 1), quickStartsEntity.quickStarts[i].links[j].text, true, propertyDetail);
                    }
                }

                this.QuickStart = JObject.Parse(JsonConvert.SerializeObject(quickStartsEntity, settingFormat));
            }
            catch (Exception ex)
            {
                ErrorEntity entity = new ErrorEntity();
                entity.errorType = ErrorType.Common;
                entity.exceptionMessage = ex.Message;

                this.listError.Add(entity);
            }
        }



        /// <summary>
        /// Handles the specific item resource.(Check the item\value in resource files and )
        /// </summary>
        /// <param name="fileName">Name of the json file.</param>
        /// <param name="propertyName">Name of the property in json file.</param>
        /// <param name="itemName">Name of the specific item in resources.resjson.</param>
        /// <param name="itemValue">The value of the specific item.</param>
        /// <param name="valueRequired">When itemValue is NUll,if set true,will get an 'NotFixed' error;if set false, will get an 'Ignored' error</param>
        /// <returns>The itemName in resources.resjson start with ResourceToken</returns>
        private string HandleSpecificItemResource(string fileName, string propertyName, string itemName, string itemValue, bool valueRequired = true, string propertyDetail = null)
        {
            ErrorEntity entity = new ErrorEntity();
            entity.jsonFileName = fileName;
            if (string.IsNullOrWhiteSpace(itemValue))
            {
                entity.errorType = ErrorType.NullValue;
                entity.itemName = propertyName;
                entity.errorDetail = propertyDetail;
                if (!valueRequired)
                {
                    entity.errorStatus = ErrorStatus.Ignored;
                }
                listError.Add(entity);
                return itemValue;
            }
            //若指定字段已经被初始化，则检查在所有Resource配置文件中能否找到该字段对应的配置信息，若查找不到，则报错。
            if (itemValue.StartsWith(ResourceToken))
            {
                string tempItemName = itemValue.Replace(ResourceToken, "");
                foreach (var resource in this.Resources)
                {
                    if (!resource.Value.ContainsKey(tempItemName))
                    {
                        entity.errorType = ErrorType.LostResource;
                        entity.jsonFileName = string.Format(@"{0}\resources.resjson", resource.Key);
                        entity.itemName = tempItemName;
                        listError.Add(entity);
                    }
                }
                return itemValue;
            }
            else//若指定字段未被初始化，则先报错提示该字段未被初始化；然后向所有的Resource配置文件中添加关于该字段的配置信息；
            {
                entity.errorType = ErrorType.NotSetAsResourceItem;
                entity.itemName = propertyName;
                entity.errorStatus = ErrorStatus.Fixed;
                listError.Add(entity);

                foreach (var resource in this.Resources)
                {
                    if (resource.Value.ContainsKey(itemName))//若已存在同名的配置信息，进行值覆盖
                    {
                        resource.Value[itemName] = itemValue;
                    }
                    else//添加配置信息
                    {
                        resource.Value.Add(itemName, itemValue);
                    }
                }
                return string.Format("{0}{1}", ResourceToken, itemName);
            }
        }

        /// <summary>
        /// Handles the specific item special resource.
        /// </summary>
        /// <param name="fileName">Name of the json file.</param>
        /// <param name="propertyName">Name of the property in json file.</param>
        /// <param name="itemName">Name of the specific item in resources.resjson.</param>
        /// <param name="itemValue">The value of the specific item.</param>
        /// <param name="valueRequired">When itemValue is NUll,if set true,will get an 'NotFixed' error;if set false, will get an 'Ignored' error</param>
        /// <returns>The itemName in resources.resjson start with ResourceToken</returns>
        private string HandleSpecificItemSpecialResource(string fileName, string propertyName, string itemName, string itemValue, bool valueRequired = true, string propertyDetail = null)
        {
            ErrorEntity entity = new ErrorEntity();
            entity.jsonFileName = fileName;
            if (string.IsNullOrWhiteSpace(itemValue))
            {
                entity.errorType = ErrorType.NullValue;
                entity.itemName = propertyName;
                entity.errorDetail = propertyDetail;
                if (!valueRequired)
                {
                    entity.errorStatus = ErrorStatus.Ignored;
                }
                listError.Add(entity);
                return itemValue;
            }

            if (itemValue.StartsWith(ResourceToken))
            {
                string tempItemName = itemValue.Replace(ResourceToken, "");
                foreach (var resource in this.Resources)
                {
                    if (!resource.Value.ContainsKey(tempItemName))
                    {
                        entity.errorType = ErrorType.LostResource;
                        entity.jsonFileName = string.Format(@"{0}\resources.resjson", resource.Key);
                        entity.itemName = tempItemName;
                        listError.Add(entity);
                    }
                }
                return itemValue;
            }
            else
            {
                int num = 1;
                string newKey = string.Format("{0}{1}", itemName, num);
                bool firstResource = true; ;

                foreach (var resource in this.Resources)
                {
                    if (firstResource)
                    {
                        while (true)
                        {
                            if (!resource.Value.ContainsKey(newKey))
                            {
                                this.Resources["en"].Add(newKey, itemValue);
                                break;
                            }
                            newKey = string.Format("{0}{1}", itemName, ++num);
                        }
                        firstResource = false;
                    }

                    if (resource.Value.ContainsKey(newKey))
                    {
                        resource.Value[newKey] = itemValue;
                    }
                    else
                    {
                        resource.Value.Add(newKey, itemValue);
                    }
                }

                entity.errorType = ErrorType.NotSetAsResourceItem;
                entity.itemName = newKey;
                entity.errorStatus = ErrorStatus.Fixed;
                listError.Add(entity);

                return string.Format("{0}{1}", ResourceToken, newKey);
            }
        }

        /// <summary>
        /// Handles the specific item icon.
        /// </summary>
        /// <param name="fileName">Name of the json file.</param>
        /// <param name="propertyName">Name of the property in json file.</param>
        /// <param name="itemName">Name of the specific item in resources.resjson.</param>
        /// <param name="itemValue">The value of the specific item.</param>
        /// <param name="valueRequired">When itemValue is NUll,if set true,will get an 'NotFixed' error;if set false, will get an 'Ignored' error</param>
        /// <returns>The name of svg file start with IconToken</returns>
        private string HandleSpecificItemIcon(string fileName, string propertyName, string itemName, string itemValue, bool valueRequired = true, string propertyDetail = null)
        {
            ErrorEntity entity = new ErrorEntity();
            entity.jsonFileName = fileName;
            if (string.IsNullOrWhiteSpace(itemValue))
            {
                entity.errorType = ErrorType.NullValue;
                entity.itemName = propertyName;
                entity.errorDetail = propertyDetail;
                if (!valueRequired)
                {
                    entity.errorStatus = ErrorStatus.Ignored;
                }
                listError.Add(entity);
                return itemValue;
            }

            if (itemValue.StartsWith(IconToken))
            {
                string tempItemName = itemValue.Replace(IconToken, "");

                if (!this.Icons.ContainsKey(tempItemName))
                {

                    entity.errorType = ErrorType.LostResource;
                    entity.itemName = tempItemName;
                    listError.Add(entity);
                }
                return itemValue;
            }
            else
            {
                entity.errorType = ErrorType.NotSetAsSvgFile;
                entity.itemName = itemName;
                entity.errorStatus = ErrorStatus.Fixed;
                listError.Add(entity);

                if (this.Icons.ContainsKey(itemName))
                {
                    this.Icons[itemName] = itemValue;
                }
                else
                {
                    this.Icons.Add(itemName, itemValue);
                }
                return string.Format("{0}{1}", IconToken, itemName);
            }
        }
    }
}
