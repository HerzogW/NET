var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "ClientResources", "../../../_generated/ExtensionDefinition"], function (require, exports, ClientResources, ExtensionDefinition) {
    "use strict";
    var SettingsPartViewModel = (function (_super) {
        __extends(SettingsPartViewModel, _super);
        function SettingsPartViewModel(container, initialState, dataContext) {
            var options = {
                enableRbac: true,
                enableTags: true
            };
            this._propertiesSettingBladeInputs = ko.observable({});
            _super.call(this, container, initialState, this._getSettings(), options);
        }
        SettingsPartViewModel.prototype.onInputsSet = function (inputs) {
            this.resourceId(inputs.id);
            this._propertiesSettingBladeInputs({ id: inputs.id });
            return null;
        };
        SettingsPartViewModel.prototype._getSettings = function () {
            var propertiesSetting = new MsPortalFx.ViewModels.Parts.SettingList.Setting("properties", ExtensionDefinition.BladeNames.propertiesBlade, this._propertiesSettingBladeInputs);
            propertiesSetting.displayText(ClientResources.propertiesBladeTitle);
            propertiesSetting.icon(MsPortalFx.Base.Images.Polychromatic.Controls());
            propertiesSetting.keywords([
                ClientResources.status,
                ClientResources.AssetTypeNames.Resource.plural,
                ClientResources.resourceLocationColumn,
                ClientResources.subscription
            ]);
            return [
                propertiesSetting
            ];
        };
        return SettingsPartViewModel;
    }(MsPortalFx.ViewModels.Parts.SettingList.ViewModel));
    exports.SettingsPartViewModel = SettingsPartViewModel;
});
