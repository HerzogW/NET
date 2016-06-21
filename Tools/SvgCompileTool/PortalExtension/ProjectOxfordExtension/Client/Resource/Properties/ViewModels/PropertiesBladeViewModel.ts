/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ResourceArea = require("../../ResourceArea");
import ClientResources = require("ClientResources");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import Def = ExtensionDefinition.ViewModels.Resource.PropertiesBladeViewModel;

export class PropertiesBladeViewModel extends MsPortalFx.ViewModels.Blade implements Def.Contract {
    public resourceId: KnockoutObservable<string> = ko.observable<string>();

    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        super();
        this.title(ClientResources.PropertiesBlade.title);
        this.icon(MsPortalFx.Base.Images.Polychromatic.Info());
    }

    public onInputsSet(inputs: Def.InputsContract): MsPortalFx.Base.Promise {
        this.resourceId(inputs.id);
        this.subtitle(MsPortalFx.ViewModels.Services.ResourceTypes.parseResourceDescriptor(inputs.id).resource);
        return null;
    }
}