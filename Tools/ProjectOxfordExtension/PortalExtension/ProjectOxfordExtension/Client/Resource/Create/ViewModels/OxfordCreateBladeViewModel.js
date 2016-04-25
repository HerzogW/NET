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
    var OxfordCreateBladeViewModel = (function (_super) {
        __extends(OxfordCreateBladeViewModel, _super);
        function OxfordCreateBladeViewModel(container, initialState, dataContext) {
            _super.call(this, container, initialState, dataContext, ClientResources.CreateBlade.title, ClientResources.CreateBlade.subtitle, Svg.Content.SVG.projectOxford2, Constants.ProjectOxfordApiTypeCategory, Constants.RPNamespace, Constants.accountsResource);
        }
        return OxfordCreateBladeViewModel;
    })(CreateBladeViewModel.CreateBladeViewModel);
    exports.OxfordCreateBladeViewModel = OxfordCreateBladeViewModel;
});
