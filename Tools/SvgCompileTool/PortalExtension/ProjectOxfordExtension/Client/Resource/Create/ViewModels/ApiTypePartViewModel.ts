/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict"
import ResourceArea = require("../../ResourceArea");
import ClientResources = require("ClientResources");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import Constants = require("../../../Shared/Constants");

import Def = ExtensionDefinition.ViewModels.Resource.ApiTypePartViewModel;
import PickerBase = MsPortalFx.ViewModels.ParameterCollectionV3.Pickers.PickerBase;
import CreateData = require("../Resource/Data/CreateData");

export class ApiInfo {
    name: string;
    title: string;
}

export class ApiTypePartViewModel extends PickerBase<CreateData.ApiItem, ApiInfo> implements Def.Contract {
    constructor(container: MsPortalFx.ViewModels.PartContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        var items = ko.observableArray<CreateData.ApiItem>();

        super(container, initialState, {
            items: items,
            mapIncomingDataToDataModel: (inputDataModel: any) => { return inputDataModel; },
            loadPickerItems: (pickerInputs: any) => {
                var itemsCategory: string = pickerInputs.itemsCategory;
				var itemsToFilter = dataContext.createData.apiTypeList();
				var filteredItems = itemsToFilter.filter((i) => {
					return jQuery.inArray(itemsCategory, i.categories) >= 0; // itemCategory in Array
				});
				items(filteredItems);
            },
            mapItemsToDataModel: (pickerItems: PickerBase.Item[]): ApiInfo => {
                return <ApiInfo>{
                    title: pickerItems.first().title(),
                    name: pickerItems.first().item
                };
            },
            isItemInDataModel: (item: PickerBase.Item, dataModel: ApiInfo): boolean => {
                var apiTypeName = dataModel && dataModel.name;
                return apiTypeName === item.item;
            }
        });

        this.pickerGrid.noRowsMessage(ClientResources.loadingText);
    }
}