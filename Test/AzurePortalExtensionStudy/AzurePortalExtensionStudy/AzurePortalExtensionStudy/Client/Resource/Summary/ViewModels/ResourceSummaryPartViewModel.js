var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "../../../_generated/ExtensionDefinition", "ClientResources"], function (require, exports, ExtensionDefinition, ClientResources) {
    "use strict";
    var ResourceSummaryPartViewModel = (function (_super) {
        __extends(ResourceSummaryPartViewModel, _super);
        function ResourceSummaryPartViewModel(container, initialState, dataContext) {
            _super.call(this, initialState, this._getOptions(), container);
            this._resourceId = ko.observable();
            this._bladeSelection = ko.observable({
                detailBlade: ExtensionDefinition.BladeNames.resourceBlade,
                detailBladeInputs: {}
            });
            var properties = [];
            properties.push(new MsPortalFx.ViewModels.Parts.Properties.TextProperty(ClientResources.textPropertyLabel, this._resourceId));
            properties.push(new MsPortalFx.ViewModels.Parts.Properties.LinkProperty(ClientResources.linkPropertyLabel, ClientResources.microsoftUri, ClientResources.linkPropertyLabel));
            properties.push(new MsPortalFx.ViewModels.Parts.Properties.OpenBladeProperty(ExtensionDefinition.BladeNames.resourceBlade, ko.observable(ExtensionDefinition.BladeNames.resourceBlade), this._bladeSelection));
            this.setProperties(properties);
        }
        ResourceSummaryPartViewModel.prototype.onInputsSet = function (inputs, settings) {
            var _this = this;
            return _super.prototype.onInputsSet.call(this, inputs, settings).then(function () {
                _this._resourceId(inputs.resourceId);
                _this._bladeSelection({
                    detailBlade: ExtensionDefinition.BladeNames.resourceBlade,
                    detailBladeInputs: {
                        id: inputs.resourceId
                    }
                });
            });
        };
        ResourceSummaryPartViewModel.prototype._getOptions = function () {
            var getQuickStartSelection = function (inputs) {
                return {
                    detailBlade: ExtensionDefinition.BladeNames.quickStartBlade,
                    detailBladeInputs: {
                        id: inputs.resourceId
                    }
                };
            };
            var getAllSettingsSelection = function (inputs) {
                return {
                    detailBlade: ExtensionDefinition.BladeNames.settingsBlade,
                    detailBladeInputs: {
                        id: inputs.resourceId
                    }
                };
            };
            return {
                getQuickStartSelection: getQuickStartSelection,
                getSettingsSelection: getAllSettingsSelection,
                collapsed: false,
            };
        };
        return ResourceSummaryPartViewModel;
    }(MsPortalFx.ViewModels.Parts.ResourceSummary.ViewModel));
    exports.ResourceSummaryPartViewModel = ResourceSummaryPartViewModel;
});
