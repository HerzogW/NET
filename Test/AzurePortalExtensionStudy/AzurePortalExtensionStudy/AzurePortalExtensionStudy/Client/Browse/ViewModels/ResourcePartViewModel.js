var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "../../_generated/ExtensionDefinition"], function (require, exports, ExtensionDefinition) {
    "use strict";
    var ResourcePartViewModel = (function (_super) {
        __extends(ResourcePartViewModel, _super);
        function ResourcePartViewModel(container, initialState, dataContext) {
            _super.call(this);
            this.assetType(ExtensionDefinition.AssetTypes.MyResource.name);
            this.icon(MsPortalFx.Base.Images.Polychromatic.PowerUp());
            if (initialState.content && initialState.content.assetId) {
                this.assetId(initialState.content.assetId);
            }
            this.icon(MsPortalFx.Base.Images.Logos.MicrosoftSquares());
        }
        ResourcePartViewModel.prototype.onInputsSet = function (inputs) {
            this.assetId(inputs.id);
            var descriptor = MsPortalFx.ViewModels.Services.ResourceTypes.parseResourceDescriptor(inputs.id);
            this.assetName(descriptor.resource);
            return null;
        };
        return ResourcePartViewModel;
    }(MsPortalFx.ViewModels.AssetPart));
    exports.ResourcePartViewModel = ResourcePartViewModel;
});
