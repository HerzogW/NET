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
    var PartsProperties = MsPortalFx.ViewModels.Parts.Properties;
    var KeysPropertiesPartViewModel = (function (_super) {
        __extends(KeysPropertiesPartViewModel, _super);
        function KeysPropertiesPartViewModel(container, initializeState, dataContext) {
            var _this = this;
            _super.call(this, container);
            this._resourceId = ko.observable();
            this._keyView = dataContext.keyData.keyEntityCache.createView(container);
            var partProperties = [];
            this._resourceProperty = new PartsProperties.CopyFieldProperty(container, {
                label: ClientResources.resourceName,
                value: ko.computed(container, function () { return _this._resourceId() && _this._resourceId().substring(_this._resourceId().lastIndexOf("/") + 1); })
            });
            this._key1Property = new PartsProperties.CopyFieldProperty(container, {
                label: ClientResources.KeyManagementBlade.key1Title,
                value: ko.computed(container, function () {
                    return _this._keyView.item() && _this._keyView.item().key1() ? _this._keyView.item().key1() : ClientResources.loadingText;
                }),
            });
            this._key2Property = new PartsProperties.CopyFieldProperty(container, {
                label: ClientResources.KeyManagementBlade.key2Title,
                value: ko.computed(container, function () {
                    return _this._keyView.item() && _this._keyView.item().key2() ? _this._keyView.item().key2() : ClientResources.loadingText;
                }),
            });
            partProperties.push(this._resourceProperty, this._key1Property, this._key2Property);
            this.setProperties(partProperties);
        }
        KeysPropertiesPartViewModel.prototype.onInputsSet = function (inputs, settings) {
            this._resourceId(inputs.resourceId);
            return this._keyView.fetch(inputs.resourceId);
        };
        return KeysPropertiesPartViewModel;
    })(PartsProperties.ViewModel);
    exports.KeysPropertiesPartViewModel = KeysPropertiesPartViewModel;
});
