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
import CmdGenSecondary = ExtensionDefinition.ViewModels.Resource.GenerateSecondaryCommandViewModel;

export class GenerateSecondaryCommandViewModel extends GenerateKeyCommandViewModel implements CmdGenSecondary.Contract {
    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialValue: any, dataContext: ResourceArea.DataContext) {
        super(container, initialValue, dataContext, ClientResources.KeyManagementBlade.MsgBox.genKey2Title, ClientResources.KeyManagementBlade.MsgBox.genKey2Des);
        this.icon(Svg.Content.SVG.regenerateKeyTwo);
    }

	public getOperationPromise(): any {
		var promise = this._dataContext.keyData.updateKeys(this.resourceId(), "{\"keyName\": \"Key2\"}");
        this._dataContext.keyData.regenerateKey1Indicator(true);
        return promise;
    }
}
