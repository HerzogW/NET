/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "ClientResources"], function (require, exports, ClientResources) {
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
            var items = ko.observableArray();
            _super.call(this, container, initialState, {
                items: items,
                mapIncomingDataToDataModel: function (inputDataModel) { return inputDataModel; },
                loadPickerItems: function (pickerInputs) {
                    var itemsCategory = pickerInputs.itemsCategory;
                    var itemsToFilter = dataContext.createData.apiTypeList();
                    var filteredItems = itemsToFilter.filter(function (i) {
                        return jQuery.inArray(itemsCategory, i.categories) >= 0; // itemCategory in Array
                    });
                    items(filteredItems);
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
        return ApiTypePartViewModel;
    })(PickerBase);
    exports.ApiTypePartViewModel = ApiTypePartViewModel;
});
