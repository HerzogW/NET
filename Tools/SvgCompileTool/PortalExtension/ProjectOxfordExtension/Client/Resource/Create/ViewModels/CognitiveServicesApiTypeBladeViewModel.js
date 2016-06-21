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
    var CognitiveServicesApiTypeBladeViewModel = (function (_super) {
        __extends(CognitiveServicesApiTypeBladeViewModel, _super);
        function CognitiveServicesApiTypeBladeViewModel(container, initialState, dataContext) {
            _super.call(this);
            this.title(ClientResources.apiType);
            this.description(ClientResources.CognitiveServices.ApiTypeBlade.description);
            this.helpUri(Constants.fwlinks.Home);
        }
        return CognitiveServicesApiTypeBladeViewModel;
    })(MsPortalFx.ViewModels.Blade);
    exports.CognitiveServicesApiTypeBladeViewModel = CognitiveServicesApiTypeBladeViewModel;
});
