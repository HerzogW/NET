/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
define(["require", "exports"], function (require, exports) {
    var SubscriptionsData = (function () {
        function SubscriptionsData() {
            var _this = this;
            this.subscriptions = ko.pureComputed(function () {
                var list = _this._subscriptionList();
                return list && list() && list().subscriptions || [];
            });
            this._subscriptionList = ko.observable();
        }
        SubscriptionsData.prototype.fetchSubscriptions = function () {
            var _this = this;
            if (!this._load) {
                this._load = MsPortalFx.Extension.getService(MsPortalFx.Base.Constants.ExtensionNames.Hubs, MsPortalFx.Extension.ServiceType.Observable, "Subscriptions")
                    .then(function (service) {
                    return service.getObservable()
                        .then(function (subscriptionList) {
                        _this._subscriptionList(subscriptionList);
                    });
                });
            }
            return this._load;
        };
        return SubscriptionsData;
    })();
    exports.SubscriptionsData = SubscriptionsData;
});
