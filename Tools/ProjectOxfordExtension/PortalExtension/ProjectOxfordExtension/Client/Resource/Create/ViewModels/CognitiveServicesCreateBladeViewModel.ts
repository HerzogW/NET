/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import CreateBladeViewModel = require("../ViewModels/CreateBladeViewModel");
import ResourceArea = require("../../ResourceArea");
import ClientResources = require("ClientResources");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import Svg = require("../../../_generated/SvgLogo");
import Constants = require("../../../Shared/Constants");
import Def = ExtensionDefinition.ViewModels.Resource.CognitiveServicesCreateBladeViewModel;

export class CognitiveServicesCreateBladeViewModel extends CreateBladeViewModel.CreateBladeViewModel implements Def.Contract {   

    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        super(container, initialState, dataContext,
            ClientResources.CognitiveServices.CreateBlade.title,
            ClientResources.CognitiveServices.CreateBlade.subtitle,
            Svg.Content.SVG.cognitiveServices2,
            Constants.CognitiveServicesApiTypeCategory,
            Constants.CognitiveServicesRPNamespace,
            Constants.CognitiveServicesAccountsResourceType);
    }
}