/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import Constants = require("../../Shared/Constants");
import DataModels = ProjectOxfordExtension.DataModels;

export class ResourceData {
    public resourceEntities = new MsPortalFx.Data.EntityCache<DataModels.Account, string>({
        entityTypeName: ProjectOxfordExtension.DataModels.AccountType,
        sourceUri: MsPortalFx.Data.uriFormatter(Constants.ArmServiceEndpoint + "{id}?" + Constants.ArmServiceVersion, false),
        supplyData: (httpMethod: string, uri: string, headers?: StringMap<any>, data?: any, params?: any) => {
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
                .catch((reason) => {
                    MsPortalFx.Base.Diagnostics.Log.writeEntry(
                        MsPortalFx.Base.Diagnostics.LogEntryLevel.Error,
                        "ResourceData",
                        "Failed to load resource entities : {0}".format(ko.toJSON(reason)));
                });
        }
    });

	public apiEntitiesPerSubscription = new MsPortalFx.Data.EntityCache<any, string>({
        entityTypeName: ProjectOxfordExtension.DataModels.AccountType,
        sourceUri: MsPortalFx.Data.uriFormatter(Constants.ArmServiceEndpoint + "/subscriptions/{id}/providers/Microsoft.CognitiveServices/accounts?" + Constants.ArmServiceVersion, false),
        supplyData: (httpMethod: string, uri: string, headers?: StringMap<any>, data?: any, params?: any) => {
			return MsPortalFx.Base.Net.ajax({
				uri: uri,
				type: "GET",
				dataType: "json",
				cache: false,
				traditional: true,
				contentType: "application/json",
				setAuthorizationHeader: true,
			}).catch((reason: any) => {
				MsPortalFx.Base.Diagnostics.Log.writeEntry(
					MsPortalFx.Base.Diagnostics.LogEntryLevel.Error,
					"ResourceData",
					"Failed to load resource entities : {0}".format(ko.toJSON(reason)));
				if (reason.status == 404) {
				}
			});
        }
    });

    public deleteResource(id: string): MsPortalFx.Base.Net.JQueryPromiseXhr<any> {
        var uri = MsPortalFx.Data.uriFormatter(Constants.ArmServiceEndpoint + "{id}?" + Constants.ArmServiceVersion, false)(id);
        return MsPortalFx.Base.Net.ajax({
            uri: uri,
            type: "DELETE",
            setAuthorizationHeader: true,
        });
    }

	public updateApiSku(resourceId: string, skuId: string): MsPortalFx.Base.Net.JQueryPromiseXhr<any> {
		var uri = MsPortalFx.Data.uriFormatter(Constants.ArmServiceEndpoint + "{id}?" + Constants.ArmServiceVersion, false)(resourceId);
        return MsPortalFx.Base.Net.ajax({
            uri: uri,
            type: "PATCH",
			dataType: "json",
			contentType: "application/json",
            setAuthorizationHeader: true,
			data: "{\"sku\": {\"name\": \"" + skuId + "\"}}"
        });
	}
}