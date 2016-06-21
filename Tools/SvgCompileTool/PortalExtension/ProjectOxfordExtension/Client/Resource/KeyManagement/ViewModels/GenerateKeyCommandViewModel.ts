/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ResourceArea = require("../../ResourceArea");
import ClientResources = require("ClientResources");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import Dialogs = MsPortalFx.ViewModels.Dialogs;

export class GenerateKeyCommandViewModel extends MsPortalFx.ViewModels.Command {

    private _confirmationMessageBox: Dialogs.MessageBox;
    public resourceId = ko.observable<string>();
	public _dataContext: ResourceArea.DataContext;

    constructor(container: MsPortalFx.ViewModels.ContainerContract,
		initialValue: any,
		dataContext: ResourceArea.DataContext,
		resetKeyTitle: string,
		resetKeyText: string) {
        super();
        this._confirmationMessageBox = new Dialogs.MessageBox(
            resetKeyTitle,
            resetKeyText,
            Dialogs.MessageBoxButtons.YesNo);
		this._dataContext = dataContext;
    }

    public execute(): void {
        this.dialog(this._confirmationMessageBox);
    }

	public getOperationPromise(): any {
		MsPortalFx.Base.Diagnostics.Log.writeEntry(
            MsPortalFx.Base.Diagnostics.LogEntryLevel.Error,
            "RegenKeyCommandViewModel",
            "getOperationPromise was not overridden in reset key command view model");
    }

    public dialogClick(result: Dialogs.DialogResult): void {
		if (result === MsPortalFx.ViewModels.Dialogs.DialogResult.Yes) {
            var operationPromise: any;
            this._dataContext.keyData.managementKeyBladeContentState(MsPortalFx.ViewModels.ContentState.Info);
            this._dataContext.keyData.managementKeyBladeContentStateMessage(ClientResources.KeyManagementBlade.genKeyStateGenerateing);
            operationPromise = this.getOperationPromise();

            if (operationPromise) {
                operationPromise.then(() => {
                    this._dataContext.keyData.managementKeyBladeContentState(MsPortalFx.ViewModels.ContentState.Success);
                    this._dataContext.keyData.managementKeyBladeContentStateMessage(ClientResources.KeyManagementBlade.genKeyStateSuccess);
                }).fail(() => {
                    this._dataContext.keyData.managementKeyBladeContentState(MsPortalFx.ViewModels.ContentState.None);
                    this._dataContext.keyData.managementKeyBladeContentStateMessage("");
                }).finally(() => {
                    this._dataContext.keyData.regenerateKey1Indicator(false);
                });
            }
        }
    }

    public onInputsSet(inputs: any): MsPortalFx.Base.Promise {
        this.resourceId(inputs.resourceId);
        return null;
    }
}
