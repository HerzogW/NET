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
		var apiItemsView = dataContext.createData.apiItemsEntities.createView(container, { interceptNotFound: false });
		var itemsToFilter: CreateData.ApiItem[] = new Array<CreateData.ApiItem>();
        super(container, initialState, {
            items: items,
            mapIncomingDataToDataModel: (inputDataModel: any) => { return inputDataModel; },
            loadPickerItems: (pickerInputs: any) => {
				this.fullfillApiItems(apiItemsView, itemsToFilter, pickerInputs, items);
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

	private fullfillApiItems(apiItemsView: any, itemsToFilter: CreateData.ApiItem[], pickerInputs: any, items: any) {
		apiItemsView.fetch(Constants.CurrentLanguage).then(() => {
			var apiItems = apiItemsView.item()
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

			var itemsCategory: string = pickerInputs.itemsCategory;
			var filteredItems = itemsToFilter.filter((i) => {
				return jQuery.inArray(itemsCategory, i.categories) >= 0;
			});
			items(filteredItems);
		}).catch((reason: any) => {
			var itemsCategory: string = pickerInputs.itemsCategory;
			var filteredItems = itemsToFilter.filter((i) => {
				return jQuery.inArray(itemsCategory, i.categories) >= 0; 
			});
			items(filteredItems);
		});
	}
}