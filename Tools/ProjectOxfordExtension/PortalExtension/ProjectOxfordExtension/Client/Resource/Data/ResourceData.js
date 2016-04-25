/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
define(["require", "exports", "../../Shared/Constants"], function (require, exports, Constants) {
    var ResourceData = (function () {
        function ResourceData() {
            this.resourceEntities = new MsPortalFx.Data.EntityCache({
                entityTypeName: ProjectOxfordExtension.DataModels.AccountType,
                sourceUri: MsPortalFx.Data.uriFormatter(Constants.ArmServiceEndpoint + "{id}?" + Constants.ArmServiceVersion, false),
                supplyData: function (httpMethod, uri, headers, data, params) {
                    return MsPortalFx.Base.Net.ajax({
                        uri: uri,
                        type: httpMethod || "GET",
                        dataType: "json",
                        cache: false,
                        traditional: true,
                        headers: headers,
                        contentType: "application/json",
                        setAuthorizationHeader: true,
                        data: data
                    })
                        .catch(function (reason) {
                        MsPortalFx.Base.Diagnostics.Log.writeEntry(MsPortalFx.Base.Diagnostics.LogEntryLevel.Error, "ResourceData", "Failed to load resource entities : {0}".format(ko.toJSON(reason)));
                    });
                }
            });
            this.apiEntitiesPerSubscription = new MsPortalFx.Data.EntityCache({
                entityTypeName: ProjectOxfordExtension.DataModels.AccountType,
                sourceUri: MsPortalFx.Data.uriFormatter(Constants.ArmServiceEndpoint + "/subscriptions/{id}/providers/Microsoft.CognitiveServices/accounts?" + Constants.ArmServiceVersion, false),
                supplyData: function (httpMethod, uri, headers, data, params) {
                    return MsPortalFx.Base.Net.ajax({
                        uri: uri,
                        type: "GET",
                        dataType: "json",
                        cache: false,
                        traditional: true,
                        contentType: "application/json",
                        setAuthorizationHeader: true,
                    }).catch(function (reason) {
                        MsPortalFx.Base.Diagnostics.Log.writeEntry(MsPortalFx.Base.Diagnostics.LogEntryLevel.Error, "ResourceData", "Failed to load resource entities : {0}".format(ko.toJSON(reason)));
                        if (reason.status == 404) {
                        }
                    });
                }
            });
        }
        ResourceData.prototype.deleteResource = function (id) {
            var uri = MsPortalFx.Data.uriFormatter(Constants.ArmServiceEndpoint + "{id}?" + Constants.ArmServiceVersion, false)(id);
            return MsPortalFx.Base.Net.ajax({
                uri: uri,
                type: "DELETE",
                setAuthorizationHeader: true,
            });
        };
        ResourceData.prototype.updateApiSku = function (resourceId, skuId) {
            var uri = MsPortalFx.Data.uriFormatter(Constants.ArmServiceEndpoint + "{id}?" + Constants.ArmServiceVersion, false)(resourceId);
            return MsPortalFx.Base.Net.ajax({
                uri: uri,
                type: "PATCH",
                dataType: "json",
                contentType: "application/json",
                setAuthorizationHeader: true,
                data: "{\"sku\": {\"name\": \"" + skuId + "\"}}"
            });
        };
        return ResourceData;
    })();
    exports.ResourceData = ResourceData;
});
