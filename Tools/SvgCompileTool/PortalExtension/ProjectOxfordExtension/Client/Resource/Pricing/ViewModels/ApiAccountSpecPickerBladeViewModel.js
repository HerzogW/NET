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
    var ApiAccountSpecPickerBladeViewModel = (function (_super) {
        __extends(ApiAccountSpecPickerBladeViewModel, _super);
        function ApiAccountSpecPickerBladeViewModel(container, initialState, dataContext) {
            _super.call(this);
            this.title(ClientResources.SpecPickerBlade.title);
            this.subtitle(ClientResources.SpecPickerBlade.subtitle);
        }
        return ApiAccountSpecPickerBladeViewModel;
    })(MsPortalFx.ViewModels.Blade);
    exports.ApiAccountSpecPickerBladeViewModel = ApiAccountSpecPickerBladeViewModel;
});
