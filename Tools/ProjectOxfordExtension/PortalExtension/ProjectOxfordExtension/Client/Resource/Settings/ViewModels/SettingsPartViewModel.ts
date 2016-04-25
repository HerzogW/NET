/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ResourceArea = require("../../ResourceArea");
import ClientResources = require("ClientResources");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import Utilities = require("../../../Shared/Utilities");

import Def = ExtensionDefinition.ViewModels.Resource.SettingsPartViewModel;
import SettingList = MsPortalFx.ViewModels.Parts.SettingList;
import PolychromaticImages = MsPortalFx.Base.Images.Polychromatic;
import SpecPickerParameterCollectionV3 = HubsExtension.Azure.SpecPicker.ParameterCollectionV3;
import SpecPicker = MsPortalFx.Azure.ResourceManager.Pickers.Specs;
import Forms = MsPortalFx.ViewModels.Forms;

export class SettingsPartViewModel extends MsPortalFx.ViewModels.Parts.SettingList.ViewModelV2 implements Def.Contract {
    private _propertiesSettingBladeInputs: KnockoutObservable<any>;
	private _keysSettingBladeInputs: KnockoutObservable<any>;
	private _pricingTierBladeInputs: KnockoutObservable<any>;
	private _alertRulesBladeInputs: KnockoutObservable<any>;
	private _auditLogsBladeInputs: KnockoutObservable<any>;
	private _diagnosticsBladeInputs: KnockoutObservable<any>;
	private _quickStartBladeInputs: KnockoutObservable<any>;

	private _pricingSpecId: KnockoutObservableBase<string>;
	private _dataContext: ResourceArea.DataContext;
	public specSelector: SpecPicker.Selector;
	private _accountView: MsPortalFx.Data.EntityView<ProjectOxfordExtension.DataModels.Account, any>;

    constructor(container: MsPortalFx.ViewModels.PartContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        var options: SettingList.Options = {
            enableRbac: true,
            enableTags: true,
			enableSupportHelpRequest: false,
			enableSupportResourceHealth: false,
			enableSupportTroubleshoot: false,
			groupable: true
        };

		this._dataContext = dataContext;
		this._accountView = dataContext.resourceData.resourceEntities.createView(container);
        this._propertiesSettingBladeInputs = ko.observable({});
		this._keysSettingBladeInputs = ko.observable({});
		this._pricingTierBladeInputs = ko.observable({});
		this._alertRulesBladeInputs = ko.observable({});
		this._auditLogsBladeInputs = ko.observable({});
        this._diagnosticsBladeInputs = ko.observable({});
        this._quickStartBladeInputs = ko.observable({});

        super(container, initialState, this._getSettings(), options);
    }

    public onInputsSet(inputs: Def.InputsContract): MsPortalFx.Base.Promise {
        this.resourceId(inputs.id);

        this._propertiesSettingBladeInputs({ id: inputs.id });
		this._keysSettingBladeInputs({ id: inputs.id });
        this._pricingTierBladeInputs({ id: inputs.id });
		this._alertRulesBladeInputs({
			targetResourceIds: [
                inputs.id
            ],
            options: {
                enableEvents: false
            }
		});
		this._auditLogsBladeInputs({
			defaultFilter:
            {
                resourceId: inputs.id,
                timespan: {
                    duration: 7 * 24 * 60 // default value for most services on azure.
                },
                level: null
            },
            options: null
		});
        this._diagnosticsBladeInputs({
			resourceId: inputs.id,
			options: {
				isDisabled: false,
				isDiagnosticsEnabled: true,
				isStorageAccountSelectorDisabled: false,
				location: "WestUS",
				isShoeboxV1: false
			}
        });
        this._quickStartBladeInputs({
            id: inputs.id,
        });

        return this._accountView.fetch(inputs.id);
    }

    private _getSettings(): SettingList.Setting[] {
        var pricingSettingOptions: SettingList.SettingOptions<SpecPickerParameterCollectionV3.SpecPickerProviderCollectorParameter> = {
			parameterCollector: {
				supplyInitialData: () => {
					var subId = this.resourceId().split("/")[2];
					var accountItem = this._accountView.item();
					var locationName = accountItem.location();

					return <SpecPickerParameterCollectionV3.SpecPickerProviderCollectorParameter>{
						fromCollectorToProvider: <SpecPickerParameterCollectionV3.SpecPickerProviderCollectorParameterFromCollector>{
							entityId: "",
							subscriptionId: subId,
							regionId: locationName,
							selectedSpecId: accountItem.sku().name(),
							recommendedSpecIds: [],
							selectRecommendedView: true,
							disabledSpecs: [],
							options: {
								apiType: accountItem.kind(),
								subscriptionId: subId,
							}
						},
						fromProviderToCollector: <SpecPickerParameterCollectionV3.SpecPickerProviderCollectorParameterFromProvider>null
					};
				},
				receiveResult: (result: any) => {
					return this._saveSelectedPricingTier(result.fromProviderToCollector.selectedSpecId);
				}
			}
		};

        var propertiesSetting = new SettingList.Setting("properties", ExtensionDefinition.BladeNames.propertiesBlade, this._propertiesSettingBladeInputs);
		var keysSetting = new SettingList.Setting("keys", ExtensionDefinition.BladeNames.keySettingBlade, this._keysSettingBladeInputs);
		var pricingSetting = new SettingList.Setting(
			"pricingTiers",
			ExtensionDefinition.BladeNames.apiAccountSpecPicker,
			ko.observable({}),
			ExtensionDefinition.definitionName,
			pricingSettingOptions);

		var alertRulesSetting = new MsPortalFx.ViewModels.Parts.SettingList.Setting(
            "alertsSetting",
            "AlertsListBlade",
            this._alertRulesBladeInputs,
            ExtensionDefinition.External.Microsoft_Azure_Insights.name);

		var auditLogsSetting = new MsPortalFx.ViewModels.Parts.SettingList.Setting(
            "auditLogs",
            "AzureDiagnosticsBladeWithParameter",
            this._auditLogsBladeInputs,
            ExtensionDefinition.External.Microsoft_Azure_Insights.name);

		var diagnosticsSetting = new SettingList.Setting(
			"diagnostics",
			ExtensionDefinition.External.Microsoft_Azure_Insights.Blades.DiagnosticsConfigurationUpdateBlade.name,
			this._diagnosticsBladeInputs,
			ExtensionDefinition.External.Microsoft_Azure_Insights.name);

        var quickStartSetting = new SettingList.Setting(
            "quickStart",
            ExtensionDefinition.BladeNames.quickStartBlade,
            this._quickStartBladeInputs);

        quickStartSetting.group(ClientResources.SettingsBlade.Group.help);
        quickStartSetting.displayText(ClientResources.SettingsBlade.quickStart);
        quickStartSetting.icon(PolychromaticImages.QuickStart());

        propertiesSetting.group(ClientResources.SettingsBlade.Group.manage);
        propertiesSetting.displayText(ClientResources.PropertiesBlade.title);
        propertiesSetting.icon(PolychromaticImages.Controls());
        propertiesSetting.keywords([
            ClientResources.status,
            ClientResources.AssetTypeNames.Resource.plural,
            ClientResources.location,
            ClientResources.subscription
        ]);

        keysSetting.group(ClientResources.SettingsBlade.Group.manage);
        keysSetting.displayText(ClientResources.SettingsBlade.keys);
        keysSetting.icon(PolychromaticImages.Key());

        pricingSetting.group(ClientResources.SettingsBlade.Group.manage);
        pricingSetting.displayText(ClientResources.pricingTier);
        pricingSetting.icon(PolychromaticImages.BillingHub());

        alertRulesSetting.group(ClientResources.SettingsBlade.Group.monitor);
        alertRulesSetting.displayText(ClientResources.SettingsBlade.alert);
        alertRulesSetting.icon(PolychromaticImages.Monitoring());

        auditLogsSetting.group(ClientResources.SettingsBlade.Group.monitor);
        auditLogsSetting.displayText(ClientResources.SettingsBlade.audit);
        auditLogsSetting.icon(PolychromaticImages.Log());

        diagnosticsSetting.group(ClientResources.SettingsBlade.Group.monitor);
        diagnosticsSetting.displayText(ClientResources.SettingsBlade.diagnostics);
        diagnosticsSetting.icon(PolychromaticImages.LogDiagnostics());

        return [
			propertiesSetting,
			keysSetting,
			pricingSetting,
			//alertRulesSetting,
			auditLogsSetting,
			//diagnosticsSetting,
			quickStartSetting
        ];
    }

	private _getSupportTicketParams(): any {
        return <MsPortalFx.ViewModels.Parts.SettingList.SettingOptions<any>>{
            parameterCollector: {
                supplyInitialData: () => {
                    return "";
                },
                receiveResult: (result) => {
                },
                supplyProviderConfig: () => {
                    return {
                        provisioningConfig: {
                            provisioningEnabled: true,
                            dontDiscardJourney: true
                        }
                    }
                }
            }
        };
    }

	private _saveSelectedPricingTier(selectedSpecId: string): MsPortalFx.Base.Promise {
		if (this._accountView.item().sku().name() == selectedSpecId) {
			return Q();
		}

		let deferred = Q.defer();

        let operationPromise = this._dataContext.resourceData.updateApiSku(this.resourceId(), selectedSpecId);
		var resName = this._accountView.item().name();
		var pricingTierText = Utilities.getPricingText(selectedSpecId);

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

        return deferred.promise;
    }
}