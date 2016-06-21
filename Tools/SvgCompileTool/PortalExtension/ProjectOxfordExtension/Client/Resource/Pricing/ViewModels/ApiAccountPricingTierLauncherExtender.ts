/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import ResourceArea = require("../../ResourceArea");
import SpecPicker = HubsExtension.Azure.SpecPicker;
import ClientResources = require("ClientResources");
import Constants = require("../../../Shared/Constants");
import Utilities = require("../../../Shared/Utilities");

export class ApiAccountPricingTierV3LauncherExtender implements SpecPicker.PricingTierLauncherExtender {
    public output = ko.observable<SpecPicker.PricingTierLauncherExtenderOutput>();
    public input = ko.observable<SpecPicker.PricingTierLauncherExtenderInput>();

	private _dataContext: ResourceArea.DataContext;
	private _accountView: MsPortalFx.Data.EntityView<ProjectOxfordExtension.DataModels.Account, string>;
	private currentEntity = ko.observable<ProjectOxfordExtension.DataModels.Account>();
	private specView = ko.observable<SpecPicker.SpecData>();
	private incomingEntityId = ko.observable<string>();

    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
		var entityView = dataContext.resourceData.resourceEntities.createView(container);
		this._dataContext = dataContext;
		this._accountView = entityView;
		this.input.subscribe(container, (input) => {
            if (!input) {
                return;
            }

			entityView.fetch(input.entityId).then(() => {
				this.incomingEntityId(input.entityId);
				let specData = dataContext.createData.getSpecData(entityView.item().kind());
				this.specView(specData);
				this.currentEntity(entityView.item());
			});

			ko.reactor(container, () => {
				var entity = this.currentEntity(), // subscribe entity change
					specData = this.specView(); // subscribe spec data change

				if (!entity || !specData) {
					return;
				}

				var pricingTierDisplay = <SpecPicker.PricingTierDisplay>{
                    assetName: ClientResources.CognitiveServices.AssetTypeNames.Resource.singular
				};

				var specPickerBladeParameter = <SpecPicker.SpecPickerBladeParameter>{
					selectedSpecId: this.currentEntity().sku().name(),
					entityId: this.incomingEntityId(),
					recommendedSpecIds: [],
					selectRecommendedView: true,
					subscriptionId: this.incomingEntityId().split("/")[2],
					regionId: this.currentEntity().location(),
					options: { apiType: this.currentEntity().kind() },
					disabledSpecs: [],
					detailBlade: ExtensionDefinition.BladeNames.apiAccountSpecPicker,
					extension: "Microsoft_Azure_ProjectOxford"
				};

				var spec = specData.specs.first(s => s.id == this.currentEntity().sku().name());

				var extenderOutput = <SpecPicker.PricingTierLauncherExtenderOutput>{
					pricingTierDisplay: pricingTierDisplay,
					specPickerBladeParameter: specPickerBladeParameter,
					specData: <SpecPicker.PricingTierLauncherSpecData>{
						features: specData.features,
						spec: spec
					}
				};

				this.output(extenderOutput);
            });
		});
    }

    public saveSelectedSpecAsync(selectedSpecId: string): MsPortalFx.Base.Promise {
		if (this.currentEntity().sku().name() == selectedSpecId) {
			return Q();
		}

        var deferred = $.Deferred();
        window.setTimeout(() => {
			let operationPromise = this._dataContext.resourceData.updateApiSku(this.incomingEntityId(), selectedSpecId);
			var resName = this.currentEntity().name();
			var pricingTierText = Utilities.getPricingText(this.currentEntity().sku().name());

			let notificationSettings = new Utilities.NotificationSettings(
				ClientResources.SettingsBlade.PricingTier.UpdateSuccess.title,
				ClientResources.SettingsBlade.PricingTier.UpdateSuccess.message.format(pricingTierText, resName),
				ClientResources.SettingsBlade.PricingTier.UpdateFail.title,
				ClientResources.SettingsBlade.PricingTier.UpdateFail.message,
				ClientResources.SettingsBlade.PricingTier.UpdateProgress.title,
				ClientResources.SettingsBlade.PricingTier.UpdateProgress.message
			);

			Utilities.trackOperationState(notificationSettings, operationPromise);
			operationPromise.finally(() => this._accountView.refresh().finally(() => {
				deferred.resolve();
			}));

            deferred.resolve();
        }, Constants.SaveSelectedSpecTimeout);

        return deferred.promise();
    }
}