/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ClientResources = require("ClientResources");
import ResourceArea = require("../../ResourceArea");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");

import Def = ExtensionDefinition.ViewModels.Resource.QuickStartBladeViewModel;

export class QuickStartBladeViewModel extends MsPortalFx.ViewModels.Blade implements Def.Contract {
    public resourceId: KnockoutObservable<string> = ko.observable<string>();
    constructor(contianer: MsPortalFx.ViewModels.ContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        super();
        this.title(ClientResources.QuickStartBlade.title);
        this.icon(MsPortalFx.Base.Images.AzureQuickstart());
    }

    public onInputsSet(inputs: Def.InputsContract): MsPortalFx.Base.Promise {
        this.resourceId(inputs.id);
        this.subtitle(MsPortalFx.ViewModels.Services.ResourceTypes.parseResourceDescriptor(this.resourceId()).resource);
        return null;
    }
}