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
import Def = ExtensionDefinition.ViewModels.Resource.OxfordCreateBladeViewModel;

export class OxfordCreateBladeViewModel extends CreateBladeViewModel.CreateBladeViewModel implements Def.Contract {   

    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        super(container, initialState, dataContext,
            ClientResources.CreateBlade.title,
            ClientResources.CreateBlade.subtitle,
            Svg.Content.SVG.projectOxford2,
            Constants.ProjectOxfordApiTypeCategory,
            Constants.RPNamespace,
            Constants.accountsResource);
    }
}