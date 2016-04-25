/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "ClientResources", "../../../_generated/ExtensionDefinition", "../../../Shared/Utilities"], function (require, exports, ClientResources, ExtensionDefinition, Utilities) {
    var SettingList = MsPortalFx.ViewModels.Parts.SettingList;
    var PolychromaticImages = MsPortalFx.Base.Images.Polychromatic;
    var SettingsPartViewModel = (function (_super) {
        __extends(SettingsPartViewModel, _super);
        function SettingsPartViewModel(container, initialState, dataContext) {
            var options = {
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
            _super.call(this, container, initialState, this._getSettings(), options);
        }
        SettingsPartViewModel.prototype.onInputsSet = function (inputs) {
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
                defaultFilter: {
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
        };
        SettingsPartViewModel.prototype._getSettings = function () {
            var _this = this;
            var pricingSettingOptions = {
                parameterCollector: {
                    supplyInitialData: function () {
                        var subId = _this.resourceId().split("/")[2];
                        var accountItem = _this._accountView.item();
                        var locationName = accountItem.location();
                        return {
                            fromCollectorToProvider: {
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
                            fromProviderToCollector: null
                        };
                    },
                    receiveResult: function (result) {
                        return _this._saveSelectedPricingTier(result.fromProviderToCollector.selectedSpecId);
                    }
                }
            };
            var propertiesSetting = new SettingList.Setting("properties", ExtensionDefinition.BladeNames.propertiesBlade, this._propertiesSettingBladeInputs);
            var keysSetting = new SettingList.Setting("keys", ExtensionDefinition.BladeNames.keySettingBlade, this._keysSettingBladeInputs);
            var pricingSetting = new SettingList.Setting("pricingTiers", ExtensionDefinition.BladeNames.apiAccountSpecPicker, ko.observable({}), ExtensionDefinition.definitionName, pricingSettingOptions);
            var alertRulesSetting = new MsPortalFx.ViewModels.Parts.SettingList.Setting("alertsSetting", "AlertsListBlade", this._alertRulesBladeInputs, ExtensionDefinition.External.Microsoft_Azure_Insights.name);
            var auditLogsSetting = new MsPortalFx.ViewModels.Parts.SettingList.Setting("auditLogs", "AzureDiagnosticsBladeWithParameter", this._auditLogsBladeInputs, ExtensionDefinition.External.Microsoft_Azure_Insights.name);
            var diagnosticsSetting = new SettingList.Setting("diagnostics", ExtensionDefinition.External.Microsoft_Azure_Insights.Blades.DiagnosticsConfigurationUpdateBlade.name, this._diagnosticsBladeInputs, ExtensionDefinition.External.Microsoft_Azure_Insights.name);
            var quickStartSetting = new SettingList.Setting("quickStart", ExtensionDefinition.BladeNames.quickStartBlade, this._quickStartBladeInputs);
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
        };
        SettingsPartViewModel.prototype._getSupportTicketParams = function () {
            return {
                parameterCollector: {
                    supplyInitialData: function () {
                        return "";
                    },
                    receiveResult: function (result) {
                    },
                    supplyProviderConfig: function () {
                        return {
                            provisioningConfig: {
                                provisioningEnabled: true,
                                dontDiscardJourney: true
                            }
                        };
                    }
                }
            };
        };
        SettingsPartViewModel.prototype._saveSelectedPricingTier = function (selectedSpecId) {
            var _this = this;
            if (this._accountView.item().sku().name() == selectedSpecId) {
                return Q();
            }
            var deferred = Q.defer();
            var operationPromise = this._dataContext.resourceData.updateApiSku(this.resourceId(), selectedSpecId);
            var resName = this._accountView.item().name();
            var pricingTierText = Utilities.getPricingText(selectedSpecId);
            var notificationSettings = new Utilities.NotificationSettings(ClientResources.SettingsBlade.PricingTier.UpdateSuccess.title, ClientResources.SettingsBlade.PricingTier.UpdateSuccess.message.format(pricingTierText, resName), ClientResources.SettingsBlade.PricingTier.UpdateFail.title, ClientResources.SettingsBlade.PricingTier.UpdateFail.message, ClientResources.SettingsBlade.PricingTier.UpdateProgress.title, ClientResources.SettingsBlade.PricingTier.UpdateProgress.message);
            Utilities.trackOperationState(notificationSettings, operationPromise);
            operationPromise.finally(function () { return _this._accountView.refresh().finally(function () {
                deferred.resolve();
            }); });
            return deferred.promise;
        };
        return SettingsPartViewModel;
    })(MsPortalFx.ViewModels.Parts.SettingList.ViewModelV2);
    exports.SettingsPartViewModel = SettingsPartViewModel;
});
