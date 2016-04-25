/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict"
import ClientResources = require("ClientResources");
import Constants = require("../../Shared/Constants");
import SpecPicker = HubsExtension.Azure.SpecPicker;

export class ApiData {
	public apiItemEntities = new MsPortalFx.Data.EntityCache<any, any>({
        sourceUri: MsPortalFx.Data.uriFormatter(Constants.ApiConfigRoot + "apis/{id}", false),
		extendEntryLifetimes: true,
        supplyData: (httpMethod: string, uri: string, headers?: StringMap<any>, data?: any, params?: any) => {
			return MsPortalFx.Base.Net.ajax({
				uri: uri,
				type: "GET",
				dataType: "json",
				cache: true,
				traditional: true,
				headers: { ["Accept-Language"]: Constants.CurrentLanguage }
			}).catch((reason: any) => {
				MsPortalFx.Base.Diagnostics.Log.writeEntry(
					MsPortalFx.Base.Diagnostics.LogEntryLevel.Error,
					"APIConfigurationData",
					"Failed to load api item entities : {0}".format(ko.toJSON(reason)));
			});
        }
    });

	public apiSpecDataEntities = new MsPortalFx.Data.EntityCache<SpecPicker.SpecData, string>({
        sourceUri: MsPortalFx.Data.uriFormatter(Constants.ApiConfigRoot + "apis/{id}/specs", false),
		extendEntryLifetimes: true,
        supplyData: (httpMethod: string, uri: string, headers?: StringMap<any>, data?: any, params?: any) => {
			return MsPortalFx.Base.Net.ajax({
				uri: uri,
				type: "GET",
				dataType: "json",
				cache: true,
				traditional: true,
				headers: { ["Accept-Language"]: Constants.CurrentLanguage }
			}).catch((reason: any) => {
				MsPortalFx.Base.Diagnostics.Log.writeEntry(
					MsPortalFx.Base.Diagnostics.LogEntryLevel.Error,
					"APIConfigurationData",
					"Failed to load api sepc entities : {0}".format(ko.toJSON(reason)));
			});
        }
    });

	public apiQuickStartEntities = new MsPortalFx.Data.EntityCache<any, any>({
        sourceUri: MsPortalFx.Data.uriFormatter(Constants.ApiConfigRoot + "apis/{id}/quickstart", false),
		extendEntryLifetimes: true,
        supplyData: (httpMethod: string, uri: string, headers?: StringMap<any>, data?: any, params?: any) => {
			return MsPortalFx.Base.Net.ajax({
				uri: uri,
				type: "GET",
				dataType: "json",
				cache: true,
				traditional: true,
				headers: { ["Accept-Language"]: Constants.CurrentLanguage }
			}).catch((reason: any) => {
				MsPortalFx.Base.Diagnostics.Log.writeEntry(
					MsPortalFx.Base.Diagnostics.LogEntryLevel.Error,
					"APIConfigurationData",
					"Failed to load api quick start entities : {0}".format(ko.toJSON(reason)));
			});
    }
    });
}
