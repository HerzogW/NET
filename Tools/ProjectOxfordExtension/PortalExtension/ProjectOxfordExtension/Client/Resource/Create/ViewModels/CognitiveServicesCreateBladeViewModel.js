/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "../ViewModels/CreateBladeViewModel", "ClientResources", "../../../_generated/SvgLogo", "../../../Shared/Constants"], function (require, exports, CreateBladeViewModel, ClientResources, Svg, Constants) {
    var CognitiveServicesCreateBladeViewModel = (function (_super) {
        __extends(CognitiveServicesCreateBladeViewModel, _super);
        function CognitiveServicesCreateBladeViewModel(container, initialState, dataContext) {
            _super.call(this, container, initialState, dataContext, ClientResources.CognitiveServices.CreateBlade.title, ClientResources.CognitiveServices.CreateBlade.subtitle, Svg.Content.SVG.cognitiveServices2, Constants.CognitiveServicesApiTypeCategory, Constants.CognitiveServicesRPNamespace, Constants.CognitiveServicesAccountsResourceType);
        }
        return CognitiveServicesCreateBladeViewModel;
    })(CreateBladeViewModel.CreateBladeViewModel);
    exports.CognitiveServicesCreateBladeViewModel = CognitiveServicesCreateBladeViewModel;
});
