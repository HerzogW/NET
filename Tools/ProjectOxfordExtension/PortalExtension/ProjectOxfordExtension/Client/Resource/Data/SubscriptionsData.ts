/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
export class SubscriptionsData {

    public subscriptions = ko.pureComputed<HubsExtension.Azure.Subscription[]>(() => {
        var list = this._subscriptionList();
        return list && list() && list().subscriptions || [];
    });

    private _load: MsPortalFx.Base.Promise;
    private _subscriptionList = ko.observable<KnockoutObservable<HubsExtension.Azure.SubscriptionList>>();

    public fetchSubscriptions(): MsPortalFx.Base.Promise {
        if (!this._load) {
            this._load = MsPortalFx.Extension.getService(MsPortalFx.Base.Constants.ExtensionNames.Hubs, MsPortalFx.Extension.ServiceType.Observable, "Subscriptions")
            .then((service: MsPortalFx.Extension.ObservableServiceClient<KnockoutObservable<HubsExtension.Azure.SubscriptionList>>) => {
                return service.getObservable()
                .then(subscriptionList => {
                    this._subscriptionList(subscriptionList);
                });
            });
        }

        return this._load;
    }
}