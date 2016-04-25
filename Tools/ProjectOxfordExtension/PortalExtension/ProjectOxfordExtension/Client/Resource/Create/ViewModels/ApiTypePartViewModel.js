/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "ClientResources", "../../../Shared/Constants"], function (require, exports, ClientResources, Constants) {
    var PickerBase = MsPortalFx.ViewModels.ParameterCollectionV3.Pickers.PickerBase;
    var ApiInfo = (function () {
        function ApiInfo() {
        }
        return ApiInfo;
    })();
    exports.ApiInfo = ApiInfo;
    var ApiTypePartViewModel = (function (_super) {
        __extends(ApiTypePartViewModel, _super);
        function ApiTypePartViewModel(container, initialState, dataContext) {
            var _this = this;
            var items = ko.observableArray();
            var apiItemsView = dataContext.createData.apiItemsEntities.createView(container, { interceptNotFound: false });
            var itemsToFilter = new Array();
            _super.call(this, container, initialState, {
                items: items,
                mapIncomingDataToDataModel: function (inputDataModel) { return inputDataModel; },
                loadPickerItems: function (pickerInputs) {
                    _this.fullfillApiItems(apiItemsView, itemsToFilter, pickerInputs, items);
                },
                mapItemsToDataModel: function (pickerItems) {
                    return {
                        title: pickerItems.first().title(),
                        name: pickerItems.first().item
                    };
                },
                isItemInDataModel: function (item, dataModel) {
                    var apiTypeName = dataModel && dataModel.name;
                    return apiTypeName === item.item;
                }
            });
            this.pickerGrid.noRowsMessage(ClientResources.loadingText);
        }
        ApiTypePartViewModel.prototype.fullfillApiItems = function (apiItemsView, itemsToFilter, pickerInputs, items) {
            apiItemsView.fetch(Constants.CurrentLanguage).then(function () {
                var apiItems = apiItemsView.item();
                if (apiItems || apiItems.length == 0) {
                    for (var i = 0; i < apiItems.length; i++) {
                        var cats = new Array();
                        cats.push(apiItems[i].categories);
                        itemsToFilter.push({
                            title: apiItems[i].title,
                            subtitle: apiItems[i].subtitle,
                            item: apiItems[i].item(),
                            icon: ko.observable(Constants.createDynamicSvgImage(apiItems[i].iconData(), true)),
                            categories: ko.toJS(apiItems[i].categories)
                        });
                    }
                }
                var itemsCategory = pickerInputs.itemsCategory;
                var filteredItems = itemsToFilter.filter(function (i) {
                    return jQuery.inArray(itemsCategory, i.categories) >= 0;
                });
                items(filteredItems);
            }).catch(function (reason) {
                var itemsCategory = pickerInputs.itemsCategory;
                var filteredItems = itemsToFilter.filter(function (i) {
                    return jQuery.inArray(itemsCategory, i.categories) >= 0;
                });
                items(filteredItems);
            });
        };
        return ApiTypePartViewModel;
    })(PickerBase);
    exports.ApiTypePartViewModel = ApiTypePartViewModel;
});
