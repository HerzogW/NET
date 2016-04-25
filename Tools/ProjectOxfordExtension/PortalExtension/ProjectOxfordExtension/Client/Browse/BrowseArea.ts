﻿/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict"
import Constants = require("../Shared/Constants");
import DataModels = ProjectOxfordExtension.DataModels;

export class DataContext {
	public apiEntities = new MsPortalFx.Data.EntityCache<DataModels.Account, string>({
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
                        "BrowseBlade",
                        "Failed to get account list: {0}".format(ko.toJSON(reason)));
                });
        }
    });


	public getApiItem(apiType: string): MsPortalFx.Base.Net.JQueryPromiseXhr<any> {
		return MsPortalFx.Base.Net.ajax({
			uri: Constants.ApiConfigRoot + "apis/" + apiType,
			type: "GET",
			dataType: "json",
			cache: true,
			traditional: true,
			headers: { ["Accept-Language"]: Constants.CurrentLanguage },
			async: false
		}).catch((reason: any) => {
			MsPortalFx.Base.Diagnostics.Log.writeEntry(
				MsPortalFx.Base.Diagnostics.LogEntryLevel.Error,
				"APIConfigurationData",
				"Failed to load api item entities : {0}".format(ko.toJSON(reason)));
		});
	}
}