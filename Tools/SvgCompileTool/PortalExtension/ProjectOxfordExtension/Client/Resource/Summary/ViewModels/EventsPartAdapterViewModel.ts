/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import ResourceArea = require("../../ResourceArea");

import Def = ExtensionDefinition.ViewModels.Resource.EventsPartAdapterViewModel;

"use strict";

export class EventsPartAdapterViewModel implements Def.Contract {
    public apiAccountId = ko.observable<any>();
    public eventsOptions = ko.observable<any>({});
	
    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
    }

    public onInputsSet(inputs: any): MsPortalFx.Base.Promise {
        this.apiAccountId(inputs.id);
        return Q();
    }
}
