/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "ClientResources", "../../../_generated/Svg", "./GenerateKeyCommandViewModel"], function (require, exports, ClientResources, Svg, BaseCommand) {
    var GenerateKeyCommandViewModel = BaseCommand.GenerateKeyCommandViewModel;
    var GenerateSecondaryCommandViewModel = (function (_super) {
        __extends(GenerateSecondaryCommandViewModel, _super);
        function GenerateSecondaryCommandViewModel(container, initialValue, dataContext) {
            _super.call(this, container, initialValue, dataContext, ClientResources.KeyManagementBlade.MsgBox.genKey2Title, ClientResources.KeyManagementBlade.MsgBox.genKey2Des);
            this.icon(Svg.Content.SVG.regenerateKeyTwo);
        }
        GenerateSecondaryCommandViewModel.prototype.getOperationPromise = function () {
            var promise = this._dataContext.keyData.updateKeys(this.resourceId(), "{\"keyName\": \"Key2\"}");
            this._dataContext.keyData.regenerateKey1Indicator(true);
            return promise;
        };
        return GenerateSecondaryCommandViewModel;
    })(GenerateKeyCommandViewModel);
    exports.GenerateSecondaryCommandViewModel = GenerateSecondaryCommandViewModel;
});
