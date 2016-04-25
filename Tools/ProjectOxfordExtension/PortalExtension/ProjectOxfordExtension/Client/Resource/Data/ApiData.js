/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
define(["require", "exports", "../../Shared/Constants"], function (require, exports, Constants) {
    var ApiData = (function () {
        function ApiData() {
            this.apiItemEntities = new MsPortalFx.Data.EntityCache({
                sourceUri: MsPortalFx.Data.uriFormatter(Constants.ApiConfigRoot + "apis/{id}", false),
                extendEntryLifetimes: true,
                supplyData: function (httpMethod, uri, headers, data, params) {
                    return MsPortalFx.Base.Net.ajax({
                        uri: uri,
                        type: "GET",
                        dataType: "json",
                        cache: true,
                        traditional: true,
                        headers: (_a = {}, _a["Accept-Language"] = Constants.CurrentLanguage, _a)
                    }).catch(function (reason) {
                        MsPortalFx.Base.Diagnostics.Log.writeEntry(MsPortalFx.Base.Diagnostics.LogEntryLevel.Error, "APIConfigurationData", "Failed to load api item entities : {0}".format(ko.toJSON(reason)));
                    });
                    var _a;
                }
            });
            this.apiSpecDataEntities = new MsPortalFx.Data.EntityCache({
                sourceUri: MsPortalFx.Data.uriFormatter(Constants.ApiConfigRoot + "apis/{id}/specs", false),
                extendEntryLifetimes: true,
                supplyData: function (httpMethod, uri, headers, data, params) {
                    return MsPortalFx.Base.Net.ajax({
                        uri: uri,
                        type: "GET",
                        dataType: "json",
                        cache: true,
                        traditional: true,
                        headers: (_a = {}, _a["Accept-Language"] = Constants.CurrentLanguage, _a)
                    }).catch(function (reason) {
                        MsPortalFx.Base.Diagnostics.Log.writeEntry(MsPortalFx.Base.Diagnostics.LogEntryLevel.Error, "APIConfigurationData", "Failed to load api sepc entities : {0}".format(ko.toJSON(reason)));
                    });
                    var _a;
                }
            });
            this.apiQuickStartEntities = new MsPortalFx.Data.EntityCache({
                sourceUri: MsPortalFx.Data.uriFormatter(Constants.ApiConfigRoot + "apis/{id}/quickstart", false),
                extendEntryLifetimes: true,
                supplyData: function (httpMethod, uri, headers, data, params) {
                    return MsPortalFx.Base.Net.ajax({
                        uri: uri,
                        type: "GET",
                        dataType: "json",
                        cache: true,
                        traditional: true,
                        headers: (_a = {}, _a["Accept-Language"] = Constants.CurrentLanguage, _a)
                    }).catch(function (reason) {
                        MsPortalFx.Base.Diagnostics.Log.writeEntry(MsPortalFx.Base.Diagnostics.LogEntryLevel.Error, "APIConfigurationData", "Failed to load api quick start entities : {0}".format(ko.toJSON(reason)));
                    });
                    var _a;
                }
            });
        }
        return ApiData;
    })();
    exports.ApiData = ApiData;
});
