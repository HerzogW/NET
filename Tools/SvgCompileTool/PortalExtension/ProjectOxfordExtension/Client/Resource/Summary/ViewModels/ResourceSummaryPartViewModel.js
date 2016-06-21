/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "../../../_generated/ExtensionDefinition", "ClientResources", "../../../Shared/Utilities"], function (require, exports, ExtensionDefinition, ClientResources, Utilities) {
    var Properties = MsPortalFx.ViewModels.Parts.Properties;
    var ResourceSummaryPartViewModel = (function (_super) {
        __extends(ResourceSummaryPartViewModel, _super);
        function ResourceSummaryPartViewModel(container, initialState, dataContext) {
            this._dataContext = dataContext;
            this._accountView = dataContext.resourceData.resourceEntities.createView(container);
            this._resourceId = ko.observable();
            this._bladeSelection = ko.observable({
                detailBlade: ExtensionDefinition.BladeNames.resourceBlade,
                detailBladeInputs: {}
            });
            _super.call(this, initialState, this._getOptions(container), container);
        }
        ResourceSummaryPartViewModel.prototype.onInputsSet = function (inputs, settings) {
            var _this = this;
            this._resourceId(inputs.resourceId);
            return _super.prototype.onInputsSet.call(this, inputs, settings).then(function () {
                _this._bladeSelection({
                    detailBlade: ExtensionDefinition.BladeNames.resourceBlade,
                    detailBladeInputs: {
                        id: inputs.resourceId
                    }
                });
                return _this._accountView.fetch(inputs.resourceId);
            });
        };
        ResourceSummaryPartViewModel.prototype._getOptions = function (lifetime) {
            var _this = this;
            var getKeysSelection = function (inputs) {
                return {
                    detailBlade: ExtensionDefinition.BladeNames.keySettingBlade,
                    detailBladeInputs: {
                        id: inputs.resourceId
                    }
                };
            };
            var getQuickStartSelection = function (inputs) {
                return {
                    detailBlade: ExtensionDefinition.BladeNames.quickStartBlade,
                    detailBladeInputs: {
                        id: inputs.resourceId
                    }
                };
            };
            var getSetingsSelection = function (inputs) {
                return {
                    detailBlade: ExtensionDefinition.BladeNames.settingsBlade,
                    detailBladeInputs: {
                        id: inputs.resourceId
                    }
                };
            };
            var accountView = this._accountView;
            var properties = [];
            properties.push(new Properties.TextProperty({
                label: ClientResources.apiType,
                value: ko.computed(lifetime, function () {
                    if (accountView.item()) {
                        return Utilities.getApiDisplayName(accountView.item().kind(), _this._dataContext.createData.apiTypeList());
                    }
                }),
                isLoading: accountView.loading
            }));
            properties.push(new Properties.TextProperty({
                label: ClientResources.pricingTier,
                value: ko.computed(lifetime, function () {
                    var pricing = accountView.item() && accountView.item().sku().name();
                    return Utilities.getPricingText(pricing);
                }),
                isLoading: accountView.loading
            }));
            properties.push(new Properties.TextProperty({
                label: ClientResources.ResourceBlade.endPoint,
                value: ko.computed(lifetime, function () {
                    // Following code should work, once all resources would be provisioned with the new RP
                    if (accountView.item() && accountView.item().properties().endpoint && accountView.item().properties().endpoint()) {
                        return accountView.item().properties().endpoint();
                    }
                    // backup in case the previous code didn't work
                    return _this._dataContext.apiData.getEndPoint(accountView.item() && accountView.item().kind());
                }),
                isLoading: accountView.loading
            }));
            var statusValue = ko.computed(lifetime, function () {
                // TODO: Disable some features if the provisioningState is not active
                if (accountView.item() == null) {
                    return "";
                }
                else {
                    return Utilities.getResourceStatus(_this._resourceId(), accountView.item().properties().provisioningState());
                }
            });
            return {
                getQuickStartSelection: getQuickStartSelection,
                getSettingsSelection: getSetingsSelection,
                getKeysSelection: getKeysSelection,
                collapsed: false,
                supportsResourceMove: 3 /* SubscriptionAndResourceGroup */,
                status: {
                    value: statusValue,
                    isLoading: accountView.loading
                },
                staticProperties: properties
            };
        };
        return ResourceSummaryPartViewModel;
    })(MsPortalFx.ViewModels.Parts.ResourceSummary.ViewModel2);
    exports.ResourceSummaryPartViewModel = ResourceSummaryPartViewModel;
});
