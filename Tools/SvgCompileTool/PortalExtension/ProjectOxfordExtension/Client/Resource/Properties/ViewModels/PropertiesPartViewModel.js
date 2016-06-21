/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "ClientResources", "../../../Shared/Utilities"], function (require, exports, ClientResources, Utilities) {
    var MsFxProperties = MsPortalFx.ViewModels.Parts.Properties;
    var ResourceManager = MsPortalFx.Azure.ResourceManager;
    var PropertiesPartViewModel = (function (_super) {
        __extends(PropertiesPartViewModel, _super);
        function PropertiesPartViewModel(container, initialState, dataContext) {
            _super.call(this, initialState);
            this._location = ko.observable(ClientResources.loadingText);
            this._subscriptionName = ko.observable(ClientResources.loadingText);
            this._container = container;
            this._entityView = dataContext.resourceData.resourceEntities.createView(container);
        }
        PropertiesPartViewModel.prototype.onInputsSet = function (inputs) {
            var _this = this;
            return this._entityView.fetch(inputs.id)
                .then(function () {
                var item = _this._entityView.item();
                if (item) {
                    _this.populateSubscriptionName(MsPortalFx.ViewModels.Services.ResourceTypes.parseResourceDescriptor(inputs.id).subscription);
                    _this.populateLocationDisplayName();
                    _this.populateProperties(_this._container, _this._entityView.item);
                }
            });
        };
        PropertiesPartViewModel.prototype.populateLocationDisplayName = function () {
            var _this = this;
            MsPortalFx.Azure.getLocations()
                .then(function (value) {
                _this._location(value[_this._entityView.item().location()]);
            });
        };
        PropertiesPartViewModel.prototype.populateSubscriptionName = function (subscriptionId) {
            var _this = this;
            MsPortalFx.Azure.getSubscriptionInfo(subscriptionId)
                .then(function (subscriptionData) {
                _this._subscriptionName(subscriptionData.displayName);
            });
        };
        PropertiesPartViewModel.prototype.populateProperties = function (lifetime, resource) {
            var _this = this;
            var resourceId = ko.computed(lifetime, function () { return resource() ? resource().id() : ClientResources.loadingText; });
            var partProperties = [];
            partProperties.push(new MsFxProperties.TextProperty(ClientResources.status, ko.computed(lifetime, function () { return resource() ? Utilities.getResourceStatus(resourceId(), _this._entityView.item().properties().provisioningState()) : ClientResources.loadingText; })), new MsFxProperties.TextProperty(ClientResources.pricingTier, ko.computed(lifetime, function () { return resource() ? Utilities.getPricingText(resource().sku().name()) : ClientResources.loadingText; })), new MsFxProperties.CopyFieldProperty(lifetime, {
                label: ClientResources.PropertiesBlade.subscriptionName,
                value: this._subscriptionName,
                editBlade: ResourceManager.getMoveResourceBlade(resourceId(), 0 /* Subscription */)
            }), new MsFxProperties.CopyFieldProperty(lifetime, ClientResources.PropertiesBlade.subscriptionId, ko.computed(lifetime, function () { return resource() ? MsPortalFx.ViewModels.Services.ResourceTypes.parseResourceDescriptor(resource().id()).subscription : ClientResources.loadingText; })), new MsFxProperties.CopyFieldProperty(lifetime, {
                label: ClientResources.resourceGroup,
                value: ko.computed(lifetime, function () { return resource() ? MsPortalFx.ViewModels.Services.ResourceTypes.parseResourceDescriptor(resource().id()).resourceGroup : ClientResources.loadingText; }),
                editBlade: ResourceManager.getMoveResourceBlade(resourceId(), 1 /* ResourceGroup */)
            }), new MsFxProperties.CopyFieldProperty(lifetime, ClientResources.PropertiesBlade.resourceId, ko.computed(lifetime, function () { return resource() ? resource().id() : ClientResources.loadingText; })), new MsFxProperties.TextProperty(ClientResources.location, this._location));
            this.setProperties(partProperties);
        };
        return PropertiesPartViewModel;
    })(MsPortalFx.ViewModels.Parts.Properties.ViewModel);
    exports.PropertiesPartViewModel = PropertiesPartViewModel;
});
