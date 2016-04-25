/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import ResourceArea = require("../../ResourceArea");
import ClientResources = require("ClientResources");

export class ApiAccountSpecPickerBladeViewModel extends MsPortalFx.ViewModels.Blade {
    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        super();
        this.title(ClientResources.SpecPickerBlade.title);
        this.subtitle(ClientResources.SpecPickerBlade.subtitle);        
    }
}