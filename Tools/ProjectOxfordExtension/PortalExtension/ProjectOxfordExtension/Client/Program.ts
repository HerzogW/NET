/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ExtensionDefinition = require("./_generated/ExtensionDefinition");
import ViewModelFactories = require("./_generated/ViewModelFactories");
import ResourceArea = require("./Resource/ResourceArea");

export class EntryPoint extends MsPortalFx.Extension.EntryPointBase<ViewModelFactories.ViewModelFactoriesBase> {
    public initialize(): void {

        MsPortalFx.Base.Diagnostics.Telemetry.initialize(ExtensionDefinition.definitionName, false /* traceBrowserInformation already captured by shell */);

        this.viewModelFactories = new ViewModelFactories.ViewModelFactoriesBase();
        this.viewModelFactories.Resource().setDataContextFactory<typeof ResourceArea>(
            "./Resource/ResourceArea",
            (contextModule) => new contextModule.DataContext());
    }

    public getDefinition(): MsPortalFx.Extension.Definition {
        return ExtensionDefinition.getDefinition();
    }
}

export function create(): MsPortalFx.Extension.EntryPointContract {
    return new EntryPoint();
}
