/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ResourceArea = require("../../ResourceArea");
import Svg = require("../../../_generated/Svg");
import ClientResources = require("ClientResources");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import Constants = require("../../../Shared/Constants");
import Utilities = require("../../../Shared/Utilities");
import DataModels = ProjectOxfordExtension.DataModels;
import Def = ExtensionDefinition.ViewModels.Resource.ResourceBladeViewModel;

export class ResourceBladeViewModel extends MsPortalFx.ViewModels.Blade implements Def.Contract {
    public id = ko.observable<string>();
	public eventsOptions = ko.observable<any>({});
    private _entityView: MsPortalFx.Data.EntityView<DataModels.Account, any>;
    private _resource: KnockoutObservable<DataModels.Account>;

    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        super();
        this._entityView = dataContext.resourceData.resourceEntities.createView(container);
        this._resource = this._entityView.item;
        ko.reactor(container, () => {
            if (this._resource()) {
                this.title(this._resource().name());
            }
        });
    }

    public onInputsSet(inputs: Def.InputsContract): MsPortalFx.Base.Promise {
        if (inputs && inputs.id) {
            var descriptor = MsPortalFx.ViewModels.Services.ResourceTypes.parseResourceDescriptor(inputs.id);
            this.subtitle(this.getSubtitle(descriptor));
            this.icon(Utilities.getIconSvg(descriptor)); // Get the icon based on resource descriptor
        }
        this.id(inputs.id);
        return this._entityView.fetch(inputs.id);
    }

    private getSubtitle(resourceDescriptor: MsPortalFx.ViewModels.Services.ResourceTypes.ResourceDescriptor): string {
        switch (Utilities.getResourceDecriptorType(resourceDescriptor)) {
            case Utilities.ResourceDescriptorType.CognitiveServices: return ClientResources.CognitiveServices.ResourceBlade.subtitle;
            case Utilities.ResourceDescriptorType.ProjectOxford: return ClientResources.ResourceBlade.subtitle;
            default:
                MsPortalFx.Base.Diagnostics.Log.writeEntry(
                    MsPortalFx.Base.Diagnostics.LogEntryLevel.Error,
                    "ResourceBlade",
                    "uknown resource descriptor: {0}".format(ko.toJSON(resourceDescriptor)));
                return "";
        }
    }
}