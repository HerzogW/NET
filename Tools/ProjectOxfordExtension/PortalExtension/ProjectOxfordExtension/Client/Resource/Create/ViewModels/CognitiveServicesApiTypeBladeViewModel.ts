/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict"
import ResourceArea = require("../../ResourceArea");
import ClientResources = require("ClientResources");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import Def = ExtensionDefinition.ViewModels.Resource.CognitiveServicesApiTypeBladeViewModel;
import Constants = require("../../../Shared/Constants");

export class CognitiveServicesApiTypeBladeViewModel extends MsPortalFx.ViewModels.Blade implements Def.Contract {
    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        super();
        this.title(ClientResources.apiType);
        this.description(ClientResources.CognitiveServices.ApiTypeBlade.description);
        this.helpUri(Constants.fwlinks.Home);
    }
}


