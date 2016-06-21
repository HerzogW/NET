/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";

import ApiTypePart = require("../ViewModels/ApiTypePartViewModel");
import CreateData = require("../../Data/CreateData");

interface CreateModel {
    name: KnockoutObservable<string>;
    apiType: KnockoutObservable<ApiTypePart.ApiInfo>;
    spec: KnockoutObservable<MsPortalFx.Azure.ResourceManager.Pickers.Specs.Result>;
    subscription: KnockoutObservable<MsPortalFx.Azure.Subscription>;
    resourceGroup: KnockoutObservable<MsPortalFx.Azure.CreatorAndDropdDownValue<MsPortalFx.Azure.ResourceGroup>>;
    location: KnockoutObservable<MsPortalFx.Azure.Location>;
	legalForm: KnockoutObservable<CreateData.ApiLegalForm>;
}
export = CreateModel;