/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "../BrowseArea", "../../_generated/SvgLogo", "../../_generated/ExtensionDefinition", "../../Shared/Utilities", "ClientResources"], function (require, exports, BrowseArea, Svg, ExtensionDefinition, Utilities, ClientResources) {
    var ResourcePartViewModel = (function (_super) {
        __extends(ResourcePartViewModel, _super);
        function ResourcePartViewModel(container, initialState, dataContext) {
            _super.call(this);
            this.assetType(ExtensionDefinition.AssetTypes.ApiAccount.name);
            if (initialState.content && initialState.content.assetId) {
                this.assetId(initialState.content.assetId);
            }
            dataContext = new BrowseArea.DataContext();
            this._entityView = dataContext.apiEntities.createView(container);
            this.icon(Svg.Content.SVG.projectOxford2); // This will be changed in onInputsSet. It must be set here, if not there is an error.
        }
        ResourcePartViewModel.prototype.onInputsSet = function (inputs) {
            var _this = this;
            if (inputs && inputs.id) {
                this.assetId(inputs.id);
                var descriptor = MsPortalFx.ViewModels.Services.ResourceTypes.parseResourceDescriptor(this.assetId());
                this.assetName(descriptor.resource);
                this.icon(Utilities.getIconSvg(descriptor)); // Get the icon based on resource descriptor
                this.assetType(this.getAssetType(descriptor));
                return this._entityView.fetch(this.assetId())
                    .then(function () {
                    var provisioningState = _this._entityView.item().properties().provisioningState();
                    _this.status(Utilities.getResourceStatus(_this.assetId(), provisioningState));
                });
            }
            return Q();
        };
        ResourcePartViewModel.prototype.getAssetType = function (resourceDescriptor) {
            switch (Utilities.getResourceDecriptorType(resourceDescriptor)) {
                case Utilities.ResourceDescriptorType.CognitiveServices: return ClientResources.CognitiveServices.AssetTypeNames.Resource.singular;
                case Utilities.ResourceDescriptorType.ProjectOxford: return ExtensionDefinition.AssetTypes.ApiAccount.name;
                default:
                    MsPortalFx.Base.Diagnostics.Log.writeEntry(MsPortalFx.Base.Diagnostics.LogEntryLevel.Error, "ResourcePart", "uknown resource descriptor: {0}".format(ko.toJSON(resourceDescriptor)));
                    return "";
            }
        };
        return ResourcePartViewModel;
    })(MsPortalFx.ViewModels.AssetPart);
    exports.ResourcePartViewModel = ResourcePartViewModel;
});
