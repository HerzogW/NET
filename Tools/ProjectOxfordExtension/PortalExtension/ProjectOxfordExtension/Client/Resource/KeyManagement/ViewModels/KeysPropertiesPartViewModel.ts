/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ResourceArea = require("../../ResourceArea");
import ClientResources = require("ClientResources");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import DataModels = ProjectOxfordExtension.DataModels;
import PartsProperties = MsPortalFx.ViewModels.Parts.Properties;
import KeysPart = ExtensionDefinition.ViewModels.Resource.KeysPropertiesPartViewModel;

export class KeysPropertiesPartViewModel extends PartsProperties.ViewModel implements KeysPart.Contract {

	private _keyView: MsPortalFx.Data.EntityView<DataModels.Keys, string>;
	private _resourceProperty: PartsProperties.CopyFieldProperty;
	private _key1Property: PartsProperties.CopyFieldProperty;
	private _key2Property: PartsProperties.CopyFieldProperty;
	private _resourceId = ko.observable<string>();

    constructor(container: MsPortalFx.ViewModels.PartContainerContract, initializeState: any, dataContext: ResourceArea.DataContext) {
        super(container);

		this._keyView = dataContext.keyData.keyEntityCache.createView(container);

		var partProperties: PartsProperties.Property[] = [];
        this._resourceProperty = new PartsProperties.CopyFieldProperty(container, {
            label: ClientResources.resourceName,
			value: ko.computed(container, () => { return this._resourceId() && this._resourceId().substring(this._resourceId().lastIndexOf("/") + 1); })
		});

        this._key1Property = new PartsProperties.CopyFieldProperty(container, {
            label: ClientResources.KeyManagementBlade.key1Title,
			value: ko.computed(container, () => {
                return this._keyView.item() && this._keyView.item().key1() ? this._keyView.item().key1() : ClientResources.loadingText;
            }),
		});

        this._key2Property = new PartsProperties.CopyFieldProperty(container, {
            label: ClientResources.KeyManagementBlade.key2Title,
			value: ko.computed(container, () => {
                return this._keyView.item() && this._keyView.item().key2() ? this._keyView.item().key2() : ClientResources.loadingText;
            }),
		});

		partProperties.push(this._resourceProperty, this._key1Property, this._key2Property);

		this.setProperties(partProperties);
    }

    public onInputsSet(inputs: KeysPart.InputsContract, settings: any): MsPortalFx.Base.Promise {
		this._resourceId(inputs.resourceId);
		return this._keyView.fetch(inputs.resourceId);
    }
}
