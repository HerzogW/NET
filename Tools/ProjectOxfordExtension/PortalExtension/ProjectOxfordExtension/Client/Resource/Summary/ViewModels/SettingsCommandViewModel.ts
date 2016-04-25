/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import Def = ExtensionDefinition.ViewModels.Resource.SettingsCommandViewModel;

export class SettingsCommandViewModel extends MsPortalFx.ViewModels.OpenBladeCommand implements Def.Contract {
    public resourceId = ko.observable<string>();

    constructor(container: MsPortalFx.ViewModels.CommandContainerContract, initialValue: any, dataContext: any) {
        super(container);
        this.icon(MsPortalFx.Base.Images.Gear());
    }

    public onInputsSet(inputs: Def.InputsContract): MsPortalFx.Base.Promise {
        this.resourceId(inputs.resourceId);
        return null;
    }
}