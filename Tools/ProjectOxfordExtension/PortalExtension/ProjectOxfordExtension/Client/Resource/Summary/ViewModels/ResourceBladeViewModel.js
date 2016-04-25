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
    var ResourceBladeViewModel = (function (_super) {
        __extends(ResourceBladeViewModel, _super);
        function ResourceBladeViewModel(container, initialState, dataContext) {
            var _this = this;
            _super.call(this);
            this.id = ko.observable();
            this.eventsOptions = ko.observable({});
            this._entityView = dataContext.resourceData.resourceEntities.createView(container);
            this._resource = this._entityView.item;
            ko.reactor(container, function () {
                if (_this._resource()) {
                    _this.title(_this._resource().name());
                }
            });
        }
        ResourceBladeViewModel.prototype.onInputsSet = function (inputs) {
            if (inputs && inputs.id) {
                var descriptor = MsPortalFx.ViewModels.Services.ResourceTypes.parseResourceDescriptor(inputs.id);
                this.subtitle(this.getSubtitle(descriptor));
                this.icon(Utilities.getIconSvg(descriptor)); // Get the icon based on resource descriptor
            }
            this.id(inputs.id);
            return this._entityView.fetch(inputs.id);
        };
        ResourceBladeViewModel.prototype.getSubtitle = function (resourceDescriptor) {
            switch (Utilities.getResourceDecriptorType(resourceDescriptor)) {
                case Utilities.ResourceDescriptorType.CognitiveServices: return ClientResources.CognitiveServices.ResourceBlade.subtitle;
                case Utilities.ResourceDescriptorType.ProjectOxford: return ClientResources.ResourceBlade.subtitle;
                default:
                    MsPortalFx.Base.Diagnostics.Log.writeEntry(MsPortalFx.Base.Diagnostics.LogEntryLevel.Error, "ResourceBlade", "uknown resource descriptor: {0}".format(ko.toJSON(resourceDescriptor)));
                    return "";
            }
        };
        return ResourceBladeViewModel;
    })(MsPortalFx.ViewModels.Blade);
    exports.ResourceBladeViewModel = ResourceBladeViewModel;
});
