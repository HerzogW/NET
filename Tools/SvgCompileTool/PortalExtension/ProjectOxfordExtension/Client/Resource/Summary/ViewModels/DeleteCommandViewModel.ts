/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";

import ResourceArea = require("../../ResourceArea");
import ClientResources = require("ClientResources");
import Constants = require("../../../Shared/Constants");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import Utilities = require("../../../Shared/Utilities");
import Def = ExtensionDefinition.ViewModels.Resource.DeleteCommandViewModel;
import Dialogs = MsPortalFx.ViewModels.Dialogs;

export class DeleteCommandViewModel extends MsPortalFx.ViewModels.Command implements Def.Contract {

    private _confirmationMessageBox: Dialogs.MessageBox;
    private _dataContext: ResourceArea.DataContext;
    public resourceId: KnockoutObservable<string>;

    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialValue: any, dataContext: ResourceArea.DataContext) {
        super();
        this.resourceId = ko.observable("");
        this.icon(MsPortalFx.Base.Images.Delete());
        this._dataContext = dataContext;
        this._confirmationMessageBox = new Dialogs.MessageBox(ClientResources.ResourceBlade.DeleteCommand.title, ClientResources.ResourceBlade.DeleteCommand.confirmation, Dialogs.MessageBoxButtons.YesNo);
    }

    public execute(): void {
        this.dialog(this._confirmationMessageBox);
    }

    public dialogClick(result: Dialogs.DialogResult): void {
        if (result === Dialogs.DialogResult.Yes) {
            this.enabled(false);
			var operationPromise = this._dataContext.resourceData.deleteResource(this.resourceId());
			var notificationSettings = new Utilities.NotificationSettings(
				ClientResources.BrowseBlade.DeleteResourceSuccess.title,
				ClientResources.BrowseBlade.DeleteResourceSuccess.message.format(this.resourceId()),
				ClientResources.BrowseBlade.DeleteResourceFail.title,
				ClientResources.BrowseBlade.DeleteResourceFail.message.format(this.resourceId()),
				ClientResources.BrowseBlade.DeleteResourceInProgress.title,
				ClientResources.BrowseBlade.DeleteResourceInProgress.message.format(this.resourceId())
			);

			Utilities.trackOperationState(notificationSettings, operationPromise);
			operationPromise.finally(() => {
				this.dialog(null);
			});
        }
        else {
            this.status(MsPortalFx.ViewModels.CommandStatus.None);
        }
    }

    public onInputsSet(inputs: Def.InputsContract): MsPortalFx.Base.Promise {
        this.resourceId(inputs.resourceId);
        return null;
    }
}