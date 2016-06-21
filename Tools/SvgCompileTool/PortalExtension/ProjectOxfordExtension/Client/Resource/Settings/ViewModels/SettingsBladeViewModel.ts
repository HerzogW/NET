/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ResourceArea = require("../../ResourceArea");
import ClientResources = require("ClientResources");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import Def = ExtensionDefinition.ViewModels.Resource.SettingsBladeViewModel;
import Blank = ExtensionDefinition.ViewModels.Resource.BlankBladeBladeViewModel;
import PolychromaticImages = MsPortalFx.Base.Images.Polychromatic;

export class SettingsBladeViewModel extends MsPortalFx.ViewModels.Blade implements Def.Contract {

    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        super();

        this.title(ClientResources.SettingsBlade.title);
        this.icon(PolychromaticImages.Controls());
    }

    public onInputsSet(inputs: Def.InputsContract): MsPortalFx.Base.Promise {
		return null;
    }
}

export class BlankBladeBladeViewModel extends MsPortalFx.ViewModels.Blade implements Blank.Contract {

    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        super();

        this.title(ClientResources.SettingsBlade.title);
        this.icon(PolychromaticImages.Controls());
    }

    public onInputsSet(inputs: Blank.InputsContract): MsPortalFx.Base.Promise {
        return null;
    }
}





