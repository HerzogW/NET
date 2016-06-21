/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
define(["require", "exports", "../Shared/Constants"], function (require, exports, Constants) {
    var DataContext = (function () {
        function DataContext() {
            this.apiEntities = new MsPortalFx.Data.EntityCache({
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
                        MsPortalFx.Base.Diagnostics.Log.writeEntry(MsPortalFx.Base.Diagnostics.LogEntryLevel.Error, "BrowseBlade", "Failed to get account list: {0}".format(ko.toJSON(reason)));
                    });
                }
            });
        }
        return DataContext;
    })();
    exports.DataContext = DataContext;
});
