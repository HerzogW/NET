/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ResourceArea = require("../../ResourceArea");
import ClientResources = require("ClientResources");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import Constants = require("../../../Shared/Constants");
import Def = ExtensionDefinition.ViewModels.Resource.KeySettingsBladeViewModel;

export class KeySettingsBladeViewModel extends MsPortalFx.ViewModels.Blade implements Def.Contract {
    private _resourceId = ko.observable<string>();
	private _resetContentStateId: number;
    private _dataContext: ResourceArea.DataContext;

    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        super();
        this.title(ClientResources.KeyManagementBlade.title);
        this.icon(MsPortalFx.Base.Images.Polychromatic.Key());
        this._dataContext = dataContext;

        ko.computed(container, () => {
            var newContentState = this._dataContext.keyData.managementKeyBladeContentState(),
                newDisplayText = this._dataContext.keyData.managementKeyBladeContentStateMessage(),
                resetId = new Date().getTime();

            this._resetContentStateId = resetId;
            this.contentState(newContentState);
            this.contentStateDisplayText(newDisplayText);

            setTimeout(() => {
                this.resetContentState(resetId);
            }, Constants.LoadTimeout);
        });
    }

	private resetContentState(id: number): void {
        if (id === this._resetContentStateId) {
            this.contentState(null);
            this.contentStateDisplayText("");
            this._dataContext.keyData.managementKeyBladeContentState(null);
            this._dataContext.keyData.managementKeyBladeContentStateMessage("");
        }
    }

    public onInputsSet(inputs: Def.InputsContract): MsPortalFx.Base.Promise {
        this._resourceId(inputs.id);
        let descriptor = MsPortalFx.ViewModels.Services.ResourceTypes.parseResourceDescriptor(this._resourceId());
        this.subtitle(descriptor.resource);
		return null;
    }
}
