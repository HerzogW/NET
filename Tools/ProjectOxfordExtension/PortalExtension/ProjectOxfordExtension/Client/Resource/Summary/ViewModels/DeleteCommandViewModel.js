/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "ClientResources", "../../../Shared/Utilities"], function (require, exports, ClientResources, Utilities) {
    var Dialogs = MsPortalFx.ViewModels.Dialogs;
    var DeleteCommandViewModel = (function (_super) {
        __extends(DeleteCommandViewModel, _super);
        function DeleteCommandViewModel(container, initialValue, dataContext) {
            _super.call(this);
            this.resourceId = ko.observable("");
            this.icon(MsPortalFx.Base.Images.Delete());
            this._dataContext = dataContext;
            this._confirmationMessageBox = new Dialogs.MessageBox(ClientResources.ResourceBlade.DeleteCommand.title, ClientResources.ResourceBlade.DeleteCommand.confirmation, Dialogs.MessageBoxButtons.YesNo);
        }
        DeleteCommandViewModel.prototype.execute = function () {
            this.dialog(this._confirmationMessageBox);
        };
        DeleteCommandViewModel.prototype.dialogClick = function (result) {
            var _this = this;
            if (result === Dialogs.DialogResult.Yes) {
                this.enabled(false);
                var operationPromise = this._dataContext.resourceData.deleteResource(this.resourceId());
                var notificationSettings = new Utilities.NotificationSettings(ClientResources.BrowseBlade.DeleteResourceSuccess.title, ClientResources.BrowseBlade.DeleteResourceSuccess.message.format(this.resourceId()), ClientResources.BrowseBlade.DeleteResourceFail.title, ClientResources.BrowseBlade.DeleteResourceFail.message.format(this.resourceId()), ClientResources.BrowseBlade.DeleteResourceInProgress.title, ClientResources.BrowseBlade.DeleteResourceInProgress.message.format(this.resourceId()));
                Utilities.trackOperationState(notificationSettings, operationPromise);
                operationPromise.finally(function () {
                    _this.dialog(null);
                });
            }
            else {
                this.status(MsPortalFx.ViewModels.CommandStatus.None);
            }
        };
        DeleteCommandViewModel.prototype.onInputsSet = function (inputs) {
            this.resourceId(inputs.resourceId);
            return null;
        };
        return DeleteCommandViewModel;
    })(MsPortalFx.ViewModels.Command);
    exports.DeleteCommandViewModel = DeleteCommandViewModel;
});
