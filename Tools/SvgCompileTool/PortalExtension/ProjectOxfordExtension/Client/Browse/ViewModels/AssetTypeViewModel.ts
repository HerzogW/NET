/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict"
import ExtensionDefinition = require("../../_generated/ExtensionDefinition");
import BrowseArea = require("../BrowseArea");
import ClientResources = require("ClientResources");
import Assets = MsPortalFx.Assets;
import ResourceTypesViewModels = ExtensionDefinition.ViewModels.Browse;
import Utilities = require("../../Shared/Utilities");
import CreateData = require("../../Resource/Data/CreateData");

export class AssetTypeViewModel implements ResourceTypesViewModels.AssetTypeViewModel.Contract {
    public supplementalDataStream: KnockoutObservableArray<Assets.SupplementalData>;
	private _container: MsPortalFx.ViewModels.ContainerContract;
    private _dataContext: BrowseArea.DataContext;
	private _aggregateSupplementalDataStream: Assets.SupplementalData[] = [];

    constructor(container: MsPortalFx.ViewModels.ContainerContract, initialState: any, dataContext: BrowseArea.DataContext) {
        this.supplementalDataStream = ko.observableArray<Assets.SupplementalData>([]);
		this._container = container;
        this._dataContext = new BrowseArea.DataContext();
    }

    public getBrowseConfig(): MsPortalFx.Base.PromiseV<Assets.BrowseConfig> {
		let config = {
			columns: <Assets.Column[]>[
				{
                    id: "pricingTier",
                    name: ko.observable<string>(ClientResources.pricingTier),
                    itemKey: "pricingTier"
                },
				{
                    id: "ApiType",
                    name: ko.observable<string>(ClientResources.apiType),
                    itemKey: "ApiType"
                },
				{
                    id: "Status",
                    name: ko.observable<string>(ClientResources.status),
                    itemKey: "Status"
                },
            ],
			defaultColumns: [
				"Status",
				"ApiType",
				"pricingTier"
            ],
            contextMenu: {
                commandGroup: ExtensionDefinition.CommandGroupNames.browseAccountCommands
            }
		};
        
        return Q(config);
    }

    public getSupplementalData(resourceIds: string[], columns: string[]): MsPortalFx.Base.Promise {
        if (resourceIds.length > 0) {
            if (columns.some(column => column === "pricingTier")
				|| columns.some(column => column === "ApiType")
				|| columns.some(column => column === "Status")) {
				this._aggregateSupplementalDataStream.length = 0;
                return Q.allSettled(resourceIds.map((resourceId) => this._getApiPromise(resourceId), this)).finally(() => {
                    this.supplementalDataStream(this._aggregateSupplementalDataStream);
                });
            }
        }

        return Q();
    }

	private _getApiPromise(resourceId: string): MsPortalFx.Base.Promise {
        let result: Assets.SupplementalData = { resourceId: resourceId };
        let apisEntityView = this._dataContext.apiEntities.createView(this._container);
        let fetchData = [apisEntityView.fetch(resourceId)];
        return Q.all(fetchData).then(
            () => {
                let api = apisEntityView.item();
                result["pricingTier"] = Utilities.getPricingText(api.sku().name());
				result["ApiType"] = Utilities.getApiDisplayName(api.kind(), new CreateData.CreateData().apiTypeList());
                result["Status"] = Utilities.getResourceStatus(resourceId, api.properties().provisioningState());
            },
            (reason: any) => {
                MsPortalFx.Base.Diagnostics.Log.writeEntry(MsPortalFx.Base.Diagnostics.LogEntryLevel.Error,
                    "AssetTypeViewModel",
                    "Failed to load asset type '{0}': {1}".format(resourceId, ko.toJSON(reason)));
            }).finally(() => {
				this._aggregateSupplementalDataStream.push(result);
			});
    }
}