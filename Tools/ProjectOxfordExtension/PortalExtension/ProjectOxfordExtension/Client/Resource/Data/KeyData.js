/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
define(["require", "exports", "ClientResources", "../../Shared/Constants"], function (require, exports, ClientResources, Constants) {
    var KeyData = (function () {
        function KeyData() {
            // Content state property for management key blade
            this.managementKeyBladeContentState = ko.observable();
            // Content state message property for management key blade
            this.managementKeyBladeContentStateMessage = ko.observable();
            // Flags to indicate regenerating keys in management key blade
            this.regenerateKey1Indicator = ko.observable(false);
            this.regenerateKey2Indicator = ko.observable(false);
            this.keyEntityCache = new MsPortalFx.Data.EntityCache({
                entityTypeName: ProjectOxfordExtension.DataModels.KeysType,
                sourceUri: MsPortalFx.Data.uriFormatter(Constants.ArmServiceEndpoint + "{id}" + "/listKeys?" + Constants.ArmServiceVersion, false),
                supplyData: function (httpMethod, uri, headers, data, params) {
                    return MsPortalFx.Base.Net.ajax({
                        uri: uri,
                        type: "POST",
                        dataType: "json",
                        cache: false,
                        traditional: true,
                        contentType: "application/json",
                        setAuthorizationHeader: true,
                    })
                        .catch(function (reason) {
                        MsPortalFx.Base.Diagnostics.Log.writeEntry(MsPortalFx.Base.Diagnostics.LogEntryLevel.Error, "KeyManagementBlade", "Failed to list all keys: {0}".format(ko.toJSON(reason)));
                    });
                },
                poll: true
            });
        }
        KeyData.prototype.updateKeys = function (id, keyType) {
            var _this = this;
            return MsPortalFx.Base.Net.ajax({
                uri: Constants.ArmServiceEndpoint + id + "/regenerateKey?" + Constants.ArmServiceVersion,
                type: "POST",
                dataType: "json",
                cache: false,
                traditional: true,
                contentType: "application/json",
                setAuthorizationHeader: true,
                data: keyType
            })
                .done(function (keysResponse) {
                _this.keyEntityCache.applyChanges(function (entityId, item) {
                    if (entityId == id) {
                        if (keysResponse.key1) {
                            item.data().key1(keysResponse.key1);
                        }
                        if (keysResponse.key2) {
                            item.data().key2(keysResponse.key2);
                        }
                    }
                });
            })
                .progress(function () {
                _this.keyEntityCache.applyChanges(function (entityId, item) {
                    if (keyType.lastIndexOf("1") > 0) {
                        item.data().key1(ClientResources.loadingText);
                    }
                    if (keyType.lastIndexOf("2") > 0) {
                        item.data().key2(ClientResources.loadingText);
                    }
                });
            })
                .catch(function (reason) {
                MsPortalFx.Base.Diagnostics.Log.writeEntry(MsPortalFx.Base.Diagnostics.LogEntryLevel.Error, "RegenKeyCommand", "Failed to generate key for resource '{0}': {1}".format(id, ko.toJSON(reason)));
            });
        };
        return KeyData;
    })();
    exports.KeyData = KeyData;
});
