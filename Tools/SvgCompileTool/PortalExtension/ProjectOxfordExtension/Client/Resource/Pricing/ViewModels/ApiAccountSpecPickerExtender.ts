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

    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
		this._resourcesPerSubscription = dataContext.resourceData.apiEntitiesPerSubscription.createView(container, { interceptNotFound: false });
		this.input.subscribe(container, (value) => {
            var apiType = value.options.apiType;
			var subscriptionId = value.options.subscriptionId;
            var specData = dataContext.createData.getSpecData(apiType);

			var promise = this._resourcesPerSubscription.fetch(subscriptionId).then(() => {
				if (this._resourcesPerSubscription.item()) {
					var specs = Utilities.getDisabledSpecsByApiType(this._resourcesPerSubscription.item(), apiType);
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
			}).catch((reason) => {
				// when current promise exception, just output original source to unblock UI.
				var output = <SpecPicker.SpecPickerExtenderOutput>{ specData: specData };
				this.output(output);
			});
		});
    }
}
