/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "ClientResources"], function (require, exports, ClientResources) {
    var Dialogs = MsPortalFx.ViewModels.Dialogs;
    var GenerateKeyCommandViewModel = (function (_super) {
        __extends(GenerateKeyCommandViewModel, _super);
        function GenerateKeyCommandViewModel(container, initialValue, dataContext, resetKeyTitle, resetKeyText) {
            _super.call(this);
            this.resourceId = ko.observable();
            this._confirmationMessageBox = new Dialogs.MessageBox(resetKeyTitle, resetKeyText, Dialogs.MessageBoxButtons.YesNo);
            this._dataContext = dataContext;
        }
        GenerateKeyCommandViewModel.prototype.execute = function () {
            this.dialog(this._confirmationMessageBox);
        };
        GenerateKeyCommandViewModel.prototype.getOperationPromise = function () {
            MsPortalFx.Base.Diagnostics.Log.writeEntry(MsPortalFx.Base.Diagnostics.LogEntryLevel.Error, "RegenKeyCommandViewModel", "getOperationPromise was not overridden in reset key command view model");
        };
        GenerateKeyCommandViewModel.prototype.dialogClick = function (result) {
            var _this = this;
            if (result === MsPortalFx.ViewModels.Dialogs.DialogResult.Yes) {
                var operationPromise;
                this._dataContext.keyData.managementKeyBladeContentState(MsPortalFx.ViewModels.ContentState.Info);
                this._dataContext.keyData.managementKeyBladeContentStateMessage(ClientResources.KeyManagementBlade.genKeyStateGenerateing);
                operationPromise = this.getOperationPromise();
                if (operationPromise) {
                    operationPromise.then(function () {
                        _this._dataContext.keyData.managementKeyBladeContentState(MsPortalFx.ViewModels.ContentState.Success);
                        _this._dataContext.keyData.managementKeyBladeContentStateMessage(ClientResources.KeyManagementBlade.genKeyStateSuccess);
                    }).fail(function () {
                        _this._dataContext.keyData.managementKeyBladeContentState(MsPortalFx.ViewModels.ContentState.None);
                        _this._dataContext.keyData.managementKeyBladeContentStateMessage("");
                    }).finally(function () {
                        _this._dataContext.keyData.regenerateKey1Indicator(false);
                    });
                }
            }
        };
        GenerateKeyCommandViewModel.prototype.onInputsSet = function (inputs) {
            this.resourceId(inputs.resourceId);
            return null;
        };
        return GenerateKeyCommandViewModel;
    })(MsPortalFx.ViewModels.Command);
    exports.GenerateKeyCommandViewModel = GenerateKeyCommandViewModel;
});
