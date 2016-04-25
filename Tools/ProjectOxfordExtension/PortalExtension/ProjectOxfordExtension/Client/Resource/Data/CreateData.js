/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
define(["require", "exports", "../../Shared/Constants"], function (require, exports, Constants) {
    //export interface SkuQuota {
    //	code: string;
    //	name: string;
    //	quota: number;
    //}
    var CreateData = (function () {
        function CreateData() {
            this.apiItemsEntities = new MsPortalFx.Data.EntityCache({
                sourceUri: MsPortalFx.Data.uriFormatter(Constants.ApiConfigRoot + "apis", false),
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
        }
        return CreateData;
    })();
    exports.CreateData = CreateData;
});
