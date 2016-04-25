/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
define(["require", "exports", "./Data/ApiData", "./Data/CreateData", "./Data/ResourceData", "./Data/SubscriptionsData", "./Data/KeyData"], function (require, exports, ApiData, CreateData, ResourceData, SubscriptionsData, KeyData) {
    var DataContext = (function () {
        function DataContext() {
            this.subscriptions = new SubscriptionsData.SubscriptionsData();
            this.keyData = new KeyData.KeyData();
            this.apiData = new ApiData.ApiData();
            this.createData = new CreateData.CreateData();
            this.resourceData = new ResourceData.ResourceData();
        }
        return DataContext;
    })();
    exports.DataContext = DataContext;
});
