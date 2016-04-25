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
    var PropertiesBladeViewModel = (function (_super) {
        __extends(PropertiesBladeViewModel, _super);
        function PropertiesBladeViewModel(container, initialState, dataContext) {
            _super.call(this);
            this.resourceId = ko.observable();
            this.title(ClientResources.PropertiesBlade.title);
            this.icon(MsPortalFx.Base.Images.Polychromatic.Info());
        }
        PropertiesBladeViewModel.prototype.onInputsSet = function (inputs) {
            this.resourceId(inputs.id);
            this.subtitle(MsPortalFx.ViewModels.Services.ResourceTypes.parseResourceDescriptor(inputs.id).resource);
            return null;
        };
        return PropertiesBladeViewModel;
    })(MsPortalFx.ViewModels.Blade);
    exports.PropertiesBladeViewModel = PropertiesBladeViewModel;
});
