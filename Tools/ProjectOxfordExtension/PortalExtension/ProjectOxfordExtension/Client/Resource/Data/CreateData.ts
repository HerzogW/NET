/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict"
import ClientResources = require("ClientResources");
import Svg = require("../../_generated/Svg");
import SvgContent = Svg.Content.SVG;
import SvgLogo = require("../../_generated/SvgLogo");
import SvgLogoContent = SvgLogo.Content.SVG;
import PickerBase = MsPortalFx.ViewModels.ParameterCollectionV3.Pickers.PickerBase;
import SpecPicker = HubsExtension.Azure.SpecPicker;
import Constants = require("../../Shared/Constants");

export interface ApiItem extends PickerBase.Item {
    // API Category (Oxford, Cognitive Services
    categories: string[];
}

export interface ApiLegalForm {
	agreed: boolean;
}

//export interface SkuQuota {
//	code: string;
//	name: string;
//	quota: number;
//}

export class CreateData {
	public apiItemsEntities = new MsPortalFx.Data.EntityCache<any[], any>({
        sourceUri: MsPortalFx.Data.uriFormatter(Constants.ApiConfigRoot + "apis", false),
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
}