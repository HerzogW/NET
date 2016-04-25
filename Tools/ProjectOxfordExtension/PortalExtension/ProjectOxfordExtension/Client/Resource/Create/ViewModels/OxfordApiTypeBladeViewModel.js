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
    var OxfordApiTypeBladeViewModel = (function (_super) {
        __extends(OxfordApiTypeBladeViewModel, _super);
        function OxfordApiTypeBladeViewModel(container, initialState, dataContext) {
            _super.call(this);
            this.title(ClientResources.apiType);
            this.description(ClientResources.ApiTypeBlade.description);
            this.helpUri(Constants.fwlinks.Home);
        }
        return OxfordApiTypeBladeViewModel;
    })(MsPortalFx.ViewModels.Blade);
    exports.OxfordApiTypeBladeViewModel = OxfordApiTypeBladeViewModel;
});
