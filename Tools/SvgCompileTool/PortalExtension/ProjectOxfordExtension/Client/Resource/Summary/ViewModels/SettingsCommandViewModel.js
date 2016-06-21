/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports"], function (require, exports) {
    var SettingsCommandViewModel = (function (_super) {
        __extends(SettingsCommandViewModel, _super);
        function SettingsCommandViewModel(container, initialValue, dataContext) {
            _super.call(this, container);
            this.resourceId = ko.observable();
            this.icon(MsPortalFx.Base.Images.Gear());
        }
        SettingsCommandViewModel.prototype.onInputsSet = function (inputs) {
            this.resourceId(inputs.resourceId);
            return null;
        };
        return SettingsCommandViewModel;
    })(MsPortalFx.ViewModels.OpenBladeCommand);
    exports.SettingsCommandViewModel = SettingsCommandViewModel;
});
