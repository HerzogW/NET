/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ResourceArea = require("../../ResourceArea");
import ClientResources = require("ClientResources");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import Svg = require("../../../_generated/Svg");
import BaseCommand = require("./GenerateKeyCommandViewModel");
import GenerateKeyCommandViewModel = BaseCommand.GenerateKeyCommandViewModel;
import CmdGenPrimary = ExtensionDefinition.ViewModels.Resource.GeneratePrimaryCommandViewModel;

export class GeneratePrimaryCommandViewModel extends GenerateKeyCommandViewModel implements CmdGenPrimary.Contract {

    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialValue: any, dataContext: ResourceArea.DataContext) {
        super(container, initialValue, dataContext, ClientResources.KeyManagementBlade.MsgBox.genKey1Title, ClientResources.KeyManagementBlade.MsgBox.genKey1Des);
        this.icon(Svg.Content.SVG.regenerateKeyOne);
    }
 
	public getOperationPromise(): any {
		var promise = this._dataContext.keyData.updateKeys(this.resourceId(), "{\"keyName\": \"Key1\"}");
        this._dataContext.keyData.regenerateKey1Indicator(true);
        return promise;
    }
}
