/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict"
import BrowseArea = require("../BrowseArea");
import DataModels = ProjectOxfordExtension.DataModels;
import Svg = require("../../_generated/SvgLogo");
import ExtensionDefinition = require("../../_generated/ExtensionDefinition");
import Utilities = require("../../Shared/Utilities");
import ClientResources = require("ClientResources");

export class ResourcePartViewModel extends MsPortalFx.ViewModels.AssetPart {
    private _entityView: MsPortalFx.Data.EntityView<DataModels.Account, string>;

    constructor(container: MsPortalFx.ViewModels.PartContainerContract, initialState: any, dataContext: BrowseArea.DataContext) {
        super();
        this.assetType(ExtensionDefinition.AssetTypes.ApiAccount.name);

        if (initialState.content && initialState.content.assetId) {
            this.assetId(initialState.content.assetId);
        }

        dataContext = new BrowseArea.DataContext();
        this._entityView = dataContext.apiEntities.createView(container);
        this.icon(Svg.Content.SVG.projectOxford2); // This will be changed in onInputsSet. It must be set here, if not there is an error.
    }

    public onInputsSet(inputs: any): MsPortalFx.Base.Promise {
        if (inputs && inputs.id) {
            this.assetId(inputs.id);
            var descriptor = MsPortalFx.ViewModels.Services.ResourceTypes.parseResourceDescriptor(this.assetId());
            this.assetName(descriptor.resource);
            this.icon(Utilities.getIconSvg(descriptor)); // Get the icon based on resource descriptor
            this.assetType(this.getAssetType(descriptor));
            return this._entityView.fetch(this.assetId())
                .then(() => {
                    var provisioningState = this._entityView.item().properties().provisioningState();
                    this.status(Utilities.getResourceStatus(this.assetId(), provisioningState));
                })
        }

        return Q();
    }

    private getAssetType(resourceDescriptor: MsPortalFx.ViewModels.Services.ResourceTypes.ResourceDescriptor): string {
        switch (Utilities.getResourceDecriptorType(resourceDescriptor)) {
            case Utilities.ResourceDescriptorType.CognitiveServices: return ClientResources.CognitiveServices.AssetTypeNames.Resource.singular;
            case Utilities.ResourceDescriptorType.ProjectOxford: return ExtensionDefinition.AssetTypes.ApiAccount.name;
            default:
                MsPortalFx.Base.Diagnostics.Log.writeEntry(
                    MsPortalFx.Base.Diagnostics.LogEntryLevel.Error,
                    "ResourcePart",
                    "uknown resource descriptor: {0}".format(ko.toJSON(resourceDescriptor)));
                return "";
        }
    }
}