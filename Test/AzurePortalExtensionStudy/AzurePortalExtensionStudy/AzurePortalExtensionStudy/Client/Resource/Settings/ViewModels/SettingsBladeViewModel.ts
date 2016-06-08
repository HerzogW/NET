/// <reference path="../../../TypeReferences.d.ts" />

import ResourceArea = require("../../ResourceArea");
import ClientResources = require("ClientResources");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");

import Def = ExtensionDefinition.ViewModels.Resource.SettingsBladeViewModel;

export class SettingsBladeViewModel extends MsPortalFx.ViewModels.Blade implements Def.Contract {

    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        super();

        this.title(ClientResources.settings);
        this.icon(MsPortalFx.Base.Images.Polychromatic.Controls());
    }

    public onInputsSet(inputs: Def.InputsContract): MsPortalFx.Base.Promise {
        return null;
    }
}