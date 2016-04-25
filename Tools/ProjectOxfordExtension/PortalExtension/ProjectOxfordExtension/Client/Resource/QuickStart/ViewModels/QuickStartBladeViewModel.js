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
    var QuickStartBladeViewModel = (function (_super) {
        __extends(QuickStartBladeViewModel, _super);
        function QuickStartBladeViewModel(contianer, initialState, dataContext) {
            _super.call(this);
            this.resourceId = ko.observable();
            this.title(ClientResources.QuickStartBlade.title);
            this.icon(MsPortalFx.Base.Images.AzureQuickstart());
        }
        QuickStartBladeViewModel.prototype.onInputsSet = function (inputs) {
            this.resourceId(inputs.id);
            this.subtitle(MsPortalFx.ViewModels.Services.ResourceTypes.parseResourceDescriptor(this.resourceId()).resource);
            return null;
        };
        return QuickStartBladeViewModel;
    })(MsPortalFx.ViewModels.Blade);
    exports.QuickStartBladeViewModel = QuickStartBladeViewModel;
});
