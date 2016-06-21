/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
define(["require", "exports", "../../../_generated/ExtensionDefinition", "ClientResources", "../../../Shared/Constants", "../../../Shared/Utilities"], function (require, exports, ExtensionDefinition, ClientResources, Constants, Utilities) {
    var ApiAccountPricingTierV3LauncherExtender = (function () {
        function ApiAccountPricingTierV3LauncherExtender(container, initialState, dataContext) {
            var _this = this;
            this.output = ko.observable();
            this.input = ko.observable();
            this.currentEntity = ko.observable();
            this.specView = ko.observable();
            this.incomingEntityId = ko.observable();
            var entityView = dataContext.resourceData.resourceEntities.createView(container);
            this._dataContext = dataContext;
            this._accountView = entityView;
            this.input.subscribe(container, function (input) {
                if (!input) {
                    return;
                }
                entityView.fetch(input.entityId).then(function () {
                    _this.incomingEntityId(input.entityId);
                    var specData = dataContext.createData.getSpecData(entityView.item().kind());
                    _this.specView(specData);
                    _this.currentEntity(entityView.item());
                });
                ko.reactor(container, function () {
                    var entity = _this.currentEntity(), // subscribe entity change
                    specData = _this.specView(); // subscribe spec data change
                    if (!entity || !specData) {
                        return;
                    }
                    var pricingTierDisplay = {
                        assetName: ClientResources.CognitiveServices.AssetTypeNames.Resource.singular
                    };
                    var specPickerBladeParameter = {
                        selectedSpecId: _this.currentEntity().sku().name(),
                        entityId: _this.incomingEntityId(),
                        recommendedSpecIds: [],
                        selectRecommendedView: true,
                        subscriptionId: _this.incomingEntityId().split("/")[2],
                        regionId: _this.currentEntity().location(),
                        options: { apiType: _this.currentEntity().kind() },
                        disabledSpecs: [],
                        detailBlade: ExtensionDefinition.BladeNames.apiAccountSpecPicker,
                        extension: "Microsoft_Azure_ProjectOxford"
                    };
                    var spec = specData.specs.first(function (s) { return s.id == _this.currentEntity().sku().name(); });
                    var extenderOutput = {
                        pricingTierDisplay: pricingTierDisplay,
                        specPickerBladeParameter: specPickerBladeParameter,
                        specData: {
                            features: specData.features,
                            spec: spec
                        }
                    };
                    _this.output(extenderOutput);
                });
            });
        }
        ApiAccountPricingTierV3LauncherExtender.prototype.saveSelectedSpecAsync = function (selectedSpecId) {
            var _this = this;
            if (this.currentEntity().sku().name() == selectedSpecId) {
                return Q();
            }
            var deferred = $.Deferred();
            window.setTimeout(function () {
                var operationPromise = _this._dataContext.resourceData.updateApiSku(_this.incomingEntityId(), selectedSpecId);
                var resName = _this.currentEntity().name();
                var pricingTierText = Utilities.getPricingText(_this.currentEntity().sku().name());
                var notificationSettings = new Utilities.NotificationSettings(ClientResources.SettingsBlade.PricingTier.UpdateSuccess.title, ClientResources.SettingsBlade.PricingTier.UpdateSuccess.message.format(pricingTierText, resName), ClientResources.SettingsBlade.PricingTier.UpdateFail.title, ClientResources.SettingsBlade.PricingTier.UpdateFail.message, ClientResources.SettingsBlade.PricingTier.UpdateProgress.title, ClientResources.SettingsBlade.PricingTier.UpdateProgress.message);
                Utilities.trackOperationState(notificationSettings, operationPromise);
                operationPromise.finally(function () { return _this._accountView.refresh().finally(function () {
                    deferred.resolve();
                }); });
                deferred.resolve();
            }, Constants.SaveSelectedSpecTimeout);
            return deferred.promise();
        };
        return ApiAccountPricingTierV3LauncherExtender;
    })();
    exports.ApiAccountPricingTierV3LauncherExtender = ApiAccountPricingTierV3LauncherExtender;
});
