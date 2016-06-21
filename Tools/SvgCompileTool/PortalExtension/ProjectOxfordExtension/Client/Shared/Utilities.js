/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
define(["require", "exports", "ClientResources", "Shared/Constants", "../_generated/SvgLogo"], function (require, exports, ClientResources, Constants, Svg) {
    function getPricingText(pricing) {
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
    exports.getPricingText = getPricingText;
    function getResourceStatus(resourceId, provisioningState) {
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
    exports.getResourceStatus = getResourceStatus;
    (function (ResourceDescriptorType) {
        ResourceDescriptorType[ResourceDescriptorType["CognitiveServices"] = 0] = "CognitiveServices";
        ResourceDescriptorType[ResourceDescriptorType["ProjectOxford"] = 1] = "ProjectOxford";
        ResourceDescriptorType[ResourceDescriptorType["Unknown"] = 2] = "Unknown";
    })(exports.ResourceDescriptorType || (exports.ResourceDescriptorType = {}));
    var ResourceDescriptorType = exports.ResourceDescriptorType;
    function getResourceDecriptorType(resourceDescriptor) {
        if (resourceDescriptor.provider === Constants.CognitiveServicesRPNamespace && resourceDescriptor.type === Constants.CognitiveServicesAccountsResourceType)
            return ResourceDescriptorType.CognitiveServices;
        if (resourceDescriptor.provider === Constants.RPNamespace && resourceDescriptor.type === Constants.accountsResource)
            return ResourceDescriptorType.ProjectOxford;
        return ResourceDescriptorType.Unknown;
    }
    exports.getResourceDecriptorType = getResourceDecriptorType;
    function getIconSvg(resourceDescriptor) {
        switch (getResourceDecriptorType(resourceDescriptor)) {
            case ResourceDescriptorType.CognitiveServices: return Svg.Content.SVG.cognitiveServices2;
            case ResourceDescriptorType.ProjectOxford: return Svg.Content.SVG.projectOxford2;
            default:
                return null;
        }
    }
    exports.getIconSvg = getIconSvg;
    var Requires = (function () {
        function Requires(paramName, param) {
            this.paramName = paramName;
            this.param = param;
        }
        Requires.Argument = function (paramName, param) {
            return new Requires(paramName, param);
        };
        Requires.prototype.notNull = function () {
            if (this.param === undefined || this.param === null) {
                throw "{0} cannot be null or undefined.".format(this.paramName);
            }
            return this;
        };
        Requires.prototype.notNullOrEmpty = function () {
            this.notNull();
            if ($.isArray(this.param)) {
                if (!(this.param.length)) {
                    throw "{0} cannot be an empty array".format(this.paramName);
                }
            }
            else if (typeof (this.param) === "string" && this.param === "") {
                throw "{0} cannot be an empty string".format(this.paramName);
            }
            return this;
        };
        return Requires;
    })();
    exports.Requires = Requires;
    var NotificationSettings = (function () {
        function NotificationSettings(successTitle, successMessage, failureTitle, failureMessage, inProgressTitle, inProgressMessage, assetInfo) {
            this._inProgressTitle = inProgressTitle;
            this._inProgressMessage = inProgressMessage;
            this._successTitle = successTitle;
            this._successMessage = successMessage;
            this._failureTitle = failureTitle;
            this._failureMessage = failureMessage;
            this.assetInfo = assetInfo;
        }
        Object.defineProperty(NotificationSettings.prototype, "inProgressTitle", {
            get: function () { return this._inProgressTitle; },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(NotificationSettings.prototype, "inProgressMessage", {
            get: function () { return this._inProgressMessage; },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(NotificationSettings.prototype, "successTitle", {
            get: function () { return this._successTitle; },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(NotificationSettings.prototype, "successMessage", {
            get: function () { return this._successMessage; },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(NotificationSettings.prototype, "failureTitle", {
            get: function () { return this._failureTitle; },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(NotificationSettings.prototype, "failureMessage", {
            get: function () { return this._failureMessage; },
            enumerable: true,
            configurable: true
        });
        return NotificationSettings;
    })();
    exports.NotificationSettings = NotificationSettings;
    function trackOperationState(notificationParams, operationPromise) {
        var _this = this;
        Requires.Argument("notificationParams", notificationParams).notNullOrEmpty();
        Requires.Argument("operationPromise", operationPromise).notNullOrEmpty();
        var notification = new MsPortalFx.Hubs.Notifications.ClientNotification({
            title: notificationParams.inProgressTitle,
            description: notificationParams.inProgressMessage,
            status: MsPortalFx.Hubs.Notifications.NotificationStatus.InProgress,
            asset: notificationParams.assetInfo
        });
        notification.publish();
        operationPromise.done(function (response) {
            notification.title = notificationParams.successTitle;
            notification.description = notificationParams.successMessage;
            notification.status = MsPortalFx.Hubs.Notifications.NotificationStatus.Information;
            notification.publish();
        }).progress(function () {
            notification.title = notificationParams.inProgressTitle;
            notification.description = notificationParams.inProgressMessage;
            notification.status = MsPortalFx.Hubs.Notifications.NotificationStatus.InProgress;
            notification.publish();
        }).fail(function (error) {
            var errorMessage = _this.getErrorMessage(error);
            var formatArgs = { errorMsg: errorMessage };
            notification.title = notificationParams.failureTitle;
            notification.description = notificationParams.failureMessage.format(formatArgs);
            notification.status = MsPortalFx.Hubs.Notifications.NotificationStatus.Error;
            notification.publish();
        });
    }
    exports.trackOperationState = trackOperationState;
    function getErrorMessage(error) {
        var errorMessage;
        errorMessage = typeof error === "string" ? error : this.getXhrError(error);
        if (!errorMessage) {
            errorMessage = "Error happened";
        }
        return errorMessage;
    }
    exports.getErrorMessage = getErrorMessage;
    function getXhrError(xhr) {
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
                var parsedXhr = JSON.parse(xhr.responseText);
                if (parsedXhr) {
                    return parsedXhr.Message || parsedXhr.message || "";
                }
            }
        }
        catch (e) { }
        return "";
    }
    exports.getXhrError = getXhrError;
    function getDisabledSpecsByApiType(data, apiType) {
        var specsToDisable = new Array();
        for (var i = 0; i < Constants.skusToCheck.length; i++) {
            if (data.value().filter(function (account, index, accounts) {
                return accounts[index].kind() == apiType && accounts[index].sku().name() == Constants.skusToCheck[i];
            }).length >= Constants.skusToCheckQuota[Constants.skusToCheck[i]]) {
                specsToDisable.push({
                    specId: Constants.skusToCheck[i],
                    message: ClientResources.SpecCard.disabledMessage,
                    helpBalloonMessage: ClientResources.SpecCard.disabledHelpMessage.format(Constants.skusToCheckQuota[Constants.skusToCheck[i]], getPricingText(Constants.skusToCheck[i]))
                });
            }
        }
        return specsToDisable;
    }
    exports.getDisabledSpecsByApiType = getDisabledSpecsByApiType;
    function getApiDisplayName(apiKind, apiItems) {
        return apiItems.filter(function (api, index, apis) {
            return apis[index].item == apiKind;
        })[0].title();
    }
    exports.getApiDisplayName = getApiDisplayName;
});
