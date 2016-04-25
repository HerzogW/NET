/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "ClientResources", "../../../Shared/Constants"], function (require, exports, ClientResources, Constants) {
    var KeySettingsBladeViewModel = (function (_super) {
        __extends(KeySettingsBladeViewModel, _super);
        function KeySettingsBladeViewModel(container, initialState, dataContext) {
            var _this = this;
            _super.call(this);
            this._resourceId = ko.observable();
            this.title(ClientResources.KeyManagementBlade.title);
            this.icon(MsPortalFx.Base.Images.Polychromatic.Key());
            this._dataContext = dataContext;
            ko.computed(container, function () {
                var newContentState = _this._dataContext.keyData.managementKeyBladeContentState(), newDisplayText = _this._dataContext.keyData.managementKeyBladeContentStateMessage(), resetId = new Date().getTime();
                _this._resetContentStateId = resetId;
                _this.contentState(newContentState);
                _this.contentStateDisplayText(newDisplayText);
                setTimeout(function () {
                    _this.resetContentState(resetId);
                }, Constants.LoadTimeout);
            });
        }
        KeySettingsBladeViewModel.prototype.resetContentState = function (id) {
            if (id === this._resetContentStateId) {
                this.contentState(null);
                this.contentStateDisplayText("");
                this._dataContext.keyData.managementKeyBladeContentState(null);
                this._dataContext.keyData.managementKeyBladeContentStateMessage("");
            }
        };
        KeySettingsBladeViewModel.prototype.onInputsSet = function (inputs) {
            this._resourceId(inputs.id);
            var descriptor = MsPortalFx.ViewModels.Services.ResourceTypes.parseResourceDescriptor(this._resourceId());
            this.subtitle(descriptor.resource);
            return null;
        };
        return KeySettingsBladeViewModel;
    })(MsPortalFx.ViewModels.Blade);
    exports.KeySettingsBladeViewModel = KeySettingsBladeViewModel;
});
