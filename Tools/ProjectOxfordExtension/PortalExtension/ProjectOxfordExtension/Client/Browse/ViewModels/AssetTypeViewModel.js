/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
define(["require", "exports", "../../_generated/ExtensionDefinition", "../BrowseArea", "ClientResources", "../../Shared/Utilities"], function (require, exports, ExtensionDefinition, BrowseArea, ClientResources, Utilities) {
    var AssetTypeViewModel = (function () {
        function AssetTypeViewModel(container, initialState, dataContext) {
            this._aggregateSupplementalDataStream = [];
            this.apiDisplayMap = {};
            this.supplementalDataStream = ko.observableArray([]);
            this._container = container;
            this._dataContext = new BrowseArea.DataContext();
        }
        AssetTypeViewModel.prototype.getBrowseConfig = function () {
            var config = {
                columns: [
                    {
                        id: "pricingTier",
                        name: ko.observable(ClientResources.pricingTier),
                        itemKey: "pricingTier"
                    },
                    {
                        id: "ApiType",
                        name: ko.observable(ClientResources.apiType),
                        itemKey: "ApiType"
                    },
                    {
                        id: "Status",
                        name: ko.observable(ClientResources.status),
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
        };
        AssetTypeViewModel.prototype.getSupplementalData = function (resourceIds, columns) {
            var _this = this;
            if (resourceIds.length > 0) {
                if (columns.some(function (column) { return column === "pricingTier"; })
                    || columns.some(function (column) { return column === "ApiType"; })
                    || columns.some(function (column) { return column === "Status"; })) {
                    this._aggregateSupplementalDataStream.length = 0;
                    return Q.allSettled(resourceIds.map(function (resourceId) { return _this._getApiPromise(resourceId); }, this)).finally(function () {
                        _this.supplementalDataStream(_this._aggregateSupplementalDataStream);
                    });
                }
            }
            return Q();
        };
        AssetTypeViewModel.prototype._getApiPromise = function (resourceId) {
            var _this = this;
            var result = { resourceId: resourceId };
            var apisEntityView = this._dataContext.apiEntities.createView(this._container);
            var fetchData = [apisEntityView.fetch(resourceId)];
            return Q.allSettled(fetchData).then(function () {
                var api = apisEntityView.item();
                if (_this.apiDisplayMap && _this.apiDisplayMap[api.kind()]) {
                    result["ApiType"] = _this.apiDisplayMap[api.kind()];
                }
                else {
                    _this._dataContext.getApiItem(api.kind()).done(function (response) {
                        result["ApiType"] = response.title;
                        _this.apiDisplayMap[api.kind()] = response.title;
                    });
                }
                result["pricingTier"] = Utilities.getPricingText(api.sku().name());
                result["Status"] = Utilities.getResourceStatus(resourceId, api.properties().provisioningState());
            }, function (reason) {
                MsPortalFx.Base.Diagnostics.Log.writeEntry(MsPortalFx.Base.Diagnostics.LogEntryLevel.Error, "AssetTypeViewModel", "Failed to load asset type '{0}': {1}".format(resourceId, ko.toJSON(reason)));
            }).finally(function () {
                _this._aggregateSupplementalDataStream.push(result);
            });
        };
        return AssetTypeViewModel;
    })();
    exports.AssetTypeViewModel = AssetTypeViewModel;
});
