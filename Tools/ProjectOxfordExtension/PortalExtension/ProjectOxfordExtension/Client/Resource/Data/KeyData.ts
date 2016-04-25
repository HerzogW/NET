/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ClientResources = require("ClientResources");
import Constants = require("../../Shared/Constants");

export class KeyData {
	// Content state property for management key blade
    public managementKeyBladeContentState = ko.observable<MsPortalFx.ViewModels.ContentState>();

    // Content state message property for management key blade
    public managementKeyBladeContentStateMessage = ko.observable<string>();

    // Flags to indicate regenerating keys in management key blade
    public regenerateKey1Indicator = ko.observable<boolean>(false);
    public regenerateKey2Indicator = ko.observable<boolean>(false);

	public keyEntityCache = new MsPortalFx.Data.EntityCache<ProjectOxfordExtension.DataModels.Keys, string>({
        entityTypeName: ProjectOxfordExtension.DataModels.KeysType,
        sourceUri: MsPortalFx.Data.uriFormatter(Constants.ArmServiceEndpoint + "{id}" + "/listKeys?" + Constants.ArmServiceVersion, false),
        supplyData: (httpMethod: string, uri: string, headers: StringMap<any>, data: any, params?: any): MsPortalFx.Base.Promise => {
            return MsPortalFx.Base.Net.ajax({
				uri: uri,
				type: "POST",
				dataType: "json",
				cache: false,
				traditional: true,
				contentType: "application/json",
				setAuthorizationHeader: true,
            })
                .catch((reason) => {
                    MsPortalFx.Base.Diagnostics.Log.writeEntry(
                        MsPortalFx.Base.Diagnostics.LogEntryLevel.Error,
                        "KeyManagementBlade",
                        "Failed to list all keys: {0}".format(ko.toJSON(reason)));
                });
        },
        poll: true
    });

    public updateKeys(id: string, keyType: string): MsPortalFx.Base.Net.JQueryPromiseXhr<string> {
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
			.done((keysResponse: any) => {
				this.keyEntityCache.applyChanges((entityId, item) => {
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
			.progress(() => {
				this.keyEntityCache.applyChanges((entityId, item) => {
					if (keyType.lastIndexOf("1") > 0) {
						item.data().key1(ClientResources.loadingText);
					}
					if (keyType.lastIndexOf("2") > 0) {
						item.data().key2(ClientResources.loadingText);
					}
				});
			})
			.catch((reason) => {
				MsPortalFx.Base.Diagnostics.Log.writeEntry(
					MsPortalFx.Base.Diagnostics.LogEntryLevel.Error,
					"RegenKeyCommand",
					"Failed to generate key for resource '{0}': {1}".format(id, ko.toJSON(reason)));
			});
    }
}