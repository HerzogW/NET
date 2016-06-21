/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */

"use strict";
import ClientResources = require("ClientResources");
import Constants = require("Shared/Constants");
import Notifications = MsPortalFx.Hubs.Notifications;
import Svg = require("../_generated/SvgLogo");

export function getPricingText(pricing: string): string {
    switch (pricing) {
        case "F0":
            return ClientResources.PricingTier.free;
        case "B0":
            return ClientResources.PricingTier.basic;
        case "S0":
        case "S1":
        case "S2":
        case "S3":
        case "S4":
            return ClientResources.PricingTier.standard;
        case "P0":
            return ClientResources.PricingTier.premium;
        default:
            return "";
    }
}

export function getResourceStatus(resourceId: string, provisioningState: string): string {
	switch (provisioningState) {
		case "Succeeded":
            return ClientResources.Status.active;
        case "Failed":
            return ClientResources.Status.provisionFailed;
        case "Canceled":
            return ClientResources.Status.provisionCanceled;
		default:
			return "";
	}
}

export enum ResourceDescriptorType {
    CognitiveServices,
    ProjectOxford,
    Unknown,
}

export function getResourceDecriptorType(resourceDescriptor: MsPortalFx.ViewModels.Services.ResourceTypes.ResourceDescriptor): ResourceDescriptorType {
    if (resourceDescriptor.provider === Constants.CognitiveServicesRPNamespace && resourceDescriptor.type === Constants.CognitiveServicesAccountsResourceType) return ResourceDescriptorType.CognitiveServices;
    if (resourceDescriptor.provider === Constants.RPNamespace && resourceDescriptor.type === Constants.accountsResource) return ResourceDescriptorType.ProjectOxford;
    return ResourceDescriptorType.Unknown;
}

export function getIconSvg(resourceDescriptor: MsPortalFx.ViewModels.Services.ResourceTypes.ResourceDescriptor): MsPortalFx.Base.Image {
    switch (getResourceDecriptorType(resourceDescriptor)) {
        case ResourceDescriptorType.CognitiveServices: return Svg.Content.SVG.cognitiveServices2;
        case ResourceDescriptorType.ProjectOxford: return Svg.Content.SVG.projectOxford2;
        default:
            return null;
    }
}

export class Requires {
	private paramName: string;
	private param: any;

	constructor(paramName: string, param: any) {
		this.paramName = paramName;
		this.param = param;
	}

	public static Argument(paramName: string, param: any): Requires {
		return new Requires(paramName, param);
	}

	public notNull(): Requires {
		if (this.param === undefined || this.param === null) {
			throw "{0} cannot be null or undefined.".format(this.paramName);
		}

		return this;
	}

	public notNullOrEmpty(): Requires {
		this.notNull();
		if ($.isArray(this.param)) {
			if (!((<Array<any>>this.param).length)) {
				throw "{0} cannot be an empty array".format(this.paramName);
			}
		} else if (typeof (this.param) === "string" && this.param === "") {
			throw "{0} cannot be an empty string".format(this.paramName);
		}
		return this;
	}
}

export class NotificationSettings {

	private _inProgressTitle: string;
	private _inProgressMessage: string;
	private _successTitle: string;
	private _successMessage: string;
	private _failureTitle: string;
	private _failureMessage: string;
	public assetInfo: MsPortalFx.Assets.AssetTriplet;

	constructor(successTitle: string, successMessage: string,
		failureTitle: string, failureMessage: string,
		inProgressTitle: string, inProgressMessage: string,
		assetInfo?: MsPortalFx.Assets.AssetTriplet) {

		this._inProgressTitle = inProgressTitle;
		this._inProgressMessage = inProgressMessage;
		this._successTitle = successTitle;
		this._successMessage = successMessage;
		this._failureTitle = failureTitle;
		this._failureMessage = failureMessage;
		this.assetInfo = assetInfo;
	}

	public get inProgressTitle(): string { return this._inProgressTitle; }
	public get inProgressMessage(): string { return this._inProgressMessage; }
	public get successTitle(): string { return this._successTitle; }
	public get successMessage(): string { return this._successMessage; }
	public get failureTitle(): string { return this._failureTitle; }
	public get failureMessage(): string { return this._failureMessage; }
}

export function trackOperationState(
	notificationParams: NotificationSettings,
	operationPromise: JQueryPromiseVR<any, any>): void {

	Requires.Argument("notificationParams", notificationParams).notNullOrEmpty();
	Requires.Argument("operationPromise", operationPromise).notNullOrEmpty();

	let notification = new MsPortalFx.Hubs.Notifications.ClientNotification({
		title: notificationParams.inProgressTitle,
		description: notificationParams.inProgressMessage,
		status: MsPortalFx.Hubs.Notifications.NotificationStatus.InProgress,
		asset: notificationParams.assetInfo
	});
	notification.publish();

	operationPromise.done((response: any) => {
		notification.title = notificationParams.successTitle;
		notification.description = notificationParams.successMessage;
		notification.status = MsPortalFx.Hubs.Notifications.NotificationStatus.Information;
		notification.publish();
	}).progress(() => {
		notification.title = notificationParams.inProgressTitle;
		notification.description = notificationParams.inProgressMessage;
		notification.status = MsPortalFx.Hubs.Notifications.NotificationStatus.InProgress;
		notification.publish();
	}).fail((error: any) => {
		let errorMessage: string = this.getErrorMessage(error);
		let formatArgs = { errorMsg: errorMessage };

		notification.title = notificationParams.failureTitle;
		notification.description = notificationParams.failureMessage.format(formatArgs);
		notification.status = MsPortalFx.Hubs.Notifications.NotificationStatus.Error;
		notification.publish();
	});
}

export function getErrorMessage(error: XMLHttpRequest): string;
export function getErrorMessage(error: string): string;
export function getErrorMessage(error: any): string {
	let errorMessage: string;

	errorMessage = typeof error === "string" ? error : this.getXhrError(error);

	if (!errorMessage) {
		errorMessage = "Error happened";
	}

	return errorMessage;
}

export function getXhrError(xhr: any): string {
	if (!xhr) {
		return "";
	}

	if (xhr.responseJSON) {
		return xhr.responseJSON.Message || xhr.responseJSON.message || "";
	}

	if (xhr.statusText) {
		return xhr.statusText;
	}

	try {
		if (xhr.responseText) {
			let parsedXhr = JSON.parse(xhr.responseText);
			if (parsedXhr) {
				return parsedXhr.Message || parsedXhr.message || "";
			}
		}
	} catch (e) { }

	return "";
}

export function getDisabledSpecsByApiType(data: any, apiType: any): Array<HubsExtension.Azure.SpecPicker.DisabledSpec> {
	var specsToDisable = new Array<HubsExtension.Azure.SpecPicker.DisabledSpec>();
	for (var i = 0; i < Constants.skusToCheck.length; i++) {
		if (data.value().filter(function (account: any, index: number, accounts: any) {
			return accounts[index].kind() == apiType && accounts[index].sku().name() == Constants.skusToCheck[i];
		}).length >= Constants.skusToCheckQuota[Constants.skusToCheck[i]]) {
			specsToDisable.push({
				specId: Constants.skusToCheck[i],
				message: ClientResources.SpecCard.disabledMessage,
				helpBalloonMessage: ClientResources.SpecCard.disabledHelpMessage.format(
					Constants.skusToCheckQuota[Constants.skusToCheck[i]],
					getPricingText(Constants.skusToCheck[i]))
			});
		}
	}

	return specsToDisable;
}

export function getApiDisplayName(apiKind: string, apiItems: any) {
	return apiItems.filter(function (api: any, index: number, apis: any) {
		return apis[index].item == apiKind;
	})[0].title();
}