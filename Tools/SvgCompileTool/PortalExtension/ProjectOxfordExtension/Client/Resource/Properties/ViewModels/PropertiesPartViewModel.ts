/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ResourceArea = require("../../ResourceArea");
import ClientResources = require("ClientResources");
import ExtensionDefinition = require("../../../_generated/ExtensionDefinition");
import Utilities = require("../../../Shared/Utilities");

import Def = ExtensionDefinition.ViewModels.Resource.PropertiesPartViewModel;
import MsFxProperties = MsPortalFx.ViewModels.Parts.Properties;
import ResourceManager = MsPortalFx.Azure.ResourceManager;
import DataModels = ProjectOxfordExtension.DataModels;

export class PropertiesPartViewModel extends MsPortalFx.ViewModels.Parts.Properties.ViewModel implements Def.Contract {
    private _container: MsPortalFx.ViewModels.PartContainerContract;
    private _entityView: MsPortalFx.Data.EntityView<DataModels.Account, any>;
    private _location = ko.observable<string>(ClientResources.loadingText);
    private _subscriptionName = ko.observable<string>(ClientResources.loadingText);

    constructor(container: MsPortalFx.ViewModels.PartContainerContract, initialState: any, dataContext: ResourceArea.DataContext) {
        super(initialState);
        this._container = container;
        this._entityView = dataContext.resourceData.resourceEntities.createView(container);
    }

    public onInputsSet(inputs: Def.InputsContract): MsPortalFx.Base.Promise {
        return this._entityView.fetch(inputs.id)
            .then(() => {
                var item = this._entityView.item();

                if (item) {
                    this.populateSubscriptionName(MsPortalFx.ViewModels.Services.ResourceTypes.parseResourceDescriptor(inputs.id).subscription);
                    this.populateLocationDisplayName();
                    this.populateProperties(this._container, this._entityView.item);
                }
            });
    }

    private populateLocationDisplayName(): void {
        MsPortalFx.Azure.getLocations()
            .then((value: StringMap<string>) => {
                this._location(value[this._entityView.item().location()]);
            });
    }

    private populateSubscriptionName(subscriptionId: string): void {
        MsPortalFx.Azure.getSubscriptionInfo(subscriptionId)
            .then((subscriptionData: MsPortalFx.Azure.Subscription) => {
                this._subscriptionName(subscriptionData.displayName);
            });
    }

    private populateProperties(lifetime: MsPortalFx.Base.LifetimeManager, resource: KnockoutObservable<DataModels.Account>): void {
        var resourceId = ko.computed(lifetime, () => { return resource() ? resource().id() : ClientResources.loadingText });

        var partProperties: MsFxProperties.Property[] = [];
        partProperties.push(
            new MsFxProperties.TextProperty(ClientResources.status, ko.computed(lifetime, () => { return resource() ? Utilities.getResourceStatus(resourceId(), this._entityView.item().properties().provisioningState()) : ClientResources.loadingText })),
            new MsFxProperties.TextProperty(ClientResources.pricingTier, ko.computed(lifetime, () => { return resource() ? Utilities.getPricingText(resource().sku().name()) : ClientResources.loadingText })),
            new MsFxProperties.CopyFieldProperty(lifetime, {
                label: ClientResources.PropertiesBlade.subscriptionName,
                value: this._subscriptionName,
                editBlade: ResourceManager.getMoveResourceBlade(resourceId(), ResourceManager.MoveType.Subscription)
            }),
            new MsFxProperties.CopyFieldProperty(lifetime, ClientResources.PropertiesBlade.subscriptionId, ko.computed(lifetime, () => { return resource() ? MsPortalFx.ViewModels.Services.ResourceTypes.parseResourceDescriptor(resource().id()).subscription : ClientResources.loadingText })),
            new MsFxProperties.CopyFieldProperty(lifetime, {
                label: ClientResources.resourceGroup,
                value: ko.computed(lifetime, () => { return resource() ? MsPortalFx.ViewModels.Services.ResourceTypes.parseResourceDescriptor(resource().id()).resourceGroup : ClientResources.loadingText }),
                editBlade: ResourceManager.getMoveResourceBlade(resourceId(), ResourceManager.MoveType.ResourceGroup)
            }),
            new MsFxProperties.CopyFieldProperty(lifetime, ClientResources.PropertiesBlade.resourceId, ko.computed(lifetime, () => { return resource() ? resource().id() : ClientResources.loadingText })),
            new MsFxProperties.TextProperty(ClientResources.location, this._location)
        );

        this.setProperties(partProperties);
    }
}
