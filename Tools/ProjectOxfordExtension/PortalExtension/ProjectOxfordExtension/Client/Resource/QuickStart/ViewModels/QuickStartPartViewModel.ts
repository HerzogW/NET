/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ClientResources = require("ClientResources");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import ResourceArea = require("../../ResourceArea");
import BaseImages = MsPortalFx.Base.Images;
import InfoList = MsPortalFx.ViewModels.Parts.InfoList;
import Constants = require("../../../Shared/Constants");
import Utilities = require("../../../Shared/Utilities");
import Def = ExtensionDefinition.ViewModels.Resource.QuickStartPartViewModel;

export class QuickStartPartViewModel extends MsPortalFx.ViewModels.Parts.InfoList.ViewModel implements Def.Contract {
    private _accountView: MsPortalFx.Data.EntityView<ProjectOxfordExtension.DataModels.Account, any>;
    private _resourceId: KnockoutObservableBase<string>;
	private _apiQuickStartsView: MsPortalFx.Data.EntityView<any, any>;

    constructor(container: MsPortalFx.ViewModels.PartContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        this._accountView = dataContext.resourceData.resourceEntities.createView(container);
		this._apiQuickStartsView = dataContext.apiData.apiQuickStartEntities.createView(container, { interceptNotFound: false });
        this._resourceId = ko.observable<string>();
        super(initialState);
    }

    public onInputsSet(inputs: Def.InputsContract, settings: any): MsPortalFx.Base.Promise {
        this._resourceId(inputs.id);
        return this._accountView.fetch(inputs.id).then(() => {
			this.populateSections();
		});
    }

    private populateSections() {
        this._apiQuickStartsView.fetch(this._accountView.item().kind()).then(() => {
			if (this._apiQuickStartsView.item()) {
				var metaData = ko.toJS(this._apiQuickStartsView.item());
				for (var i = 0; i < metaData.quickStarts.length; i++) {
					var links = metaData.quickStarts[i].links;
					var qLinks: InfoList.Link[] = new Array();
					for (var j = 0; j < links.length; j++) {
						qLinks.push(new InfoList.Link(<string>links[j].text, <string>links[j].uri));
					}

					this.addSection(
						metaData.quickStarts[i].title,
						metaData.quickStarts[i].description,
						qLinks,
						Utilities.getBaseImage(metaData.quickStarts[i].icon));
				}
			}
		});
    }	
}