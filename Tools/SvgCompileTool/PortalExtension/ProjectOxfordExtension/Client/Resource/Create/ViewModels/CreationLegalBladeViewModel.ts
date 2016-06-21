/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ResourceArea = require("../../ResourceArea");
import ClientResources = require("ClientResources");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import Svg = require("../../../_generated/Svg");
import CreateModel = require("../Models/CreateModel");
import CreateData = require("../../Data/CreateData");

export class CreationLegalBladeViewModel
	extends MsPortalFx.ViewModels.Forms.Form.ViewModel<CreateData.ApiLegalForm>
    implements ExtensionDefinition.ViewModels.Resource.CreationLegalBladeViewModel.Contract {
    public title = ko.observable(ClientResources.CreateBlade.LegalTerm.bladeTitle);
    public subtitle = ko.observable("");
    public icon = ko.observable(MsPortalFx.Base.Images.Gear());

    public actionBar: MsPortalFx.ViewModels.ActionBars.GenericActionBar.ViewModel;
    public parameterProvider: MsPortalFx.ViewModels.ParameterProvider<CreateData.ApiLegalForm, CreateData.ApiLegalForm>;

	private _incomingForm: CreateData.ApiLegalForm;

    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        super(container);

        this.actionBar = new MsPortalFx.ViewModels.ActionBars.GenericActionBar.ViewModel(container);
		this.actionBar.actionButtonLabel(ClientResources.CreateBlade.LegalTerm.buttonText);

        this.parameterProvider = new MsPortalFx.ViewModels.ParameterProvider<CreateData.ApiLegalForm, CreateData.ApiLegalForm>(container, {
			mapIncomingDataForEditScope: (incomingData: CreateData.ApiLegalForm) => {
				this._incomingForm = incomingData;
				this._incomingForm.agreed = true;
				return this._incomingForm;
            },

            mapOutgoingDataForCollector: (editScopeData: CreateData.ApiLegalForm) => {
				return {
					agreed: editScopeData.agreed,
				};
            }
        });

        this.editScope = this.parameterProvider.editScope;
    }
}