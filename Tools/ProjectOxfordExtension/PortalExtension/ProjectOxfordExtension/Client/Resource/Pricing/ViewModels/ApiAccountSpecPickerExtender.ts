/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import ResourceArea = require("../../ResourceArea");
import ClientResources = require("ClientResources");
import Utilities = require("../../../Shared/Utilities");
import Constants = require("../../../Shared/Constants");
import SpecPicker = HubsExtension.Azure.SpecPicker;

export class ApiAccountSpecPickerExtender implements SpecPicker.SpecPickerExtender {
    public input = ko.observable<SpecPicker.SpecPickerExtenderInput>();
    public output = ko.observable<SpecPicker.SpecPickerExtenderOutput>();
    private _specDataView: MsPortalFx.Data.EntityView<SpecPicker.SpecData, any>;
    private _specData = ko.observable<SpecPicker.SpecData>();
	private _disabledSpecs = new Array<HubsExtension.Azure.SpecPicker.DisabledSpec>();
	private _resourcesPerSubscription: MsPortalFx.Data.EntityView<any, string>;
	private _apiSpecView: MsPortalFx.Data.EntityView<SpecPicker.SpecData, string>;
	private _apiItemView: MsPortalFx.Data.EntityView<any, string>;

    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
		this._resourcesPerSubscription = dataContext.resourceData.apiEntitiesPerSubscription.createView(container, { interceptNotFound: false });
		this._apiSpecView = dataContext.apiData.apiSpecDataEntities.createView(container, { interceptNotFound: false });
		this._apiItemView = dataContext.apiData.apiItemEntities.createView(container, { interceptNotFound: false });
		this.input.subscribe(container, (value) => {
            var apiType = value.options.apiType;
			var subscriptionId = value.options.subscriptionId;
            var specData: SpecPicker.SpecData; 

			this._apiSpecView.fetch(apiType).then(() => {
				if (this._apiSpecView.item()) {
					specData = ko.toJS(this._apiSpecView.item());
				}
			}).then(() => {
			var promise = this._resourcesPerSubscription.fetch(subscriptionId).then(() => {
				if (this._resourcesPerSubscription.item()) {
						this._apiItemView.fetch(apiType).then(() => {
							var apiItem = this._apiItemView.item();
							if (apiItem) {
								var specs = Utilities.getDisabledSpecsByApiType(this._resourcesPerSubscription.item(), apiType, ko.toJS(apiItem).skuQuota);
					var specsToDisable = MsPortalFx.remove(specs, (v: any) => {
						return v.specId == value.selectedSpecId;
					});
					var output = <SpecPicker.SpecPickerExtenderOutput>{ specData: specData, disabledSpecs: specs };
					this.output(output);
							}
							else {
								var output = <SpecPicker.SpecPickerExtenderOutput>{ specData: specData };
								this.output(output);
							}
						}).catch((reason: any) => {
							var output = <SpecPicker.SpecPickerExtenderOutput>{ specData: specData };
							this.output(output);
						});
				}
				else {
					var output = <SpecPicker.SpecPickerExtenderOutput>{ specData: specData };
					this.output(output);
				}
			}).catch((reason) => {
				// when current promise exception, just output original source to unblock UI.
				var output = <SpecPicker.SpecPickerExtenderOutput>{ specData: specData };
				this.output(output);
			});
		});
		});
    }
}
