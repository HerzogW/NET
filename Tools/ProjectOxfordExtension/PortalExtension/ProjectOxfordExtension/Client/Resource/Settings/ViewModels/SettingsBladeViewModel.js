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
    var PolychromaticImages = MsPortalFx.Base.Images.Polychromatic;
    var SettingsBladeViewModel = (function (_super) {
        __extends(SettingsBladeViewModel, _super);
        function SettingsBladeViewModel(container, initialState, dataContext) {
            _super.call(this);
            this.title(ClientResources.SettingsBlade.title);
            this.icon(PolychromaticImages.Controls());
        }
        SettingsBladeViewModel.prototype.onInputsSet = function (inputs) {
            return null;
        };
        return SettingsBladeViewModel;
    })(MsPortalFx.ViewModels.Blade);
    exports.SettingsBladeViewModel = SettingsBladeViewModel;
    var BlankBladeBladeViewModel = (function (_super) {
        __extends(BlankBladeBladeViewModel, _super);
        function BlankBladeBladeViewModel(container, initialState, dataContext) {
            _super.call(this);
            this.title(ClientResources.SettingsBlade.title);
            this.icon(PolychromaticImages.Controls());
        }
        BlankBladeBladeViewModel.prototype.onInputsSet = function (inputs) {
            return null;
        };
        return BlankBladeBladeViewModel;
    })(MsPortalFx.ViewModels.Blade);
    exports.BlankBladeBladeViewModel = BlankBladeBladeViewModel;
});
