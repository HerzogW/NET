var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "./_generated/ExtensionDefinition", "./_generated/ViewModelFactories"], function (require, exports, ExtensionDefinition, ViewModelFactories) {
    "use strict";
    "use strict";
    var EntryPoint = (function (_super) {
        __extends(EntryPoint, _super);
        function EntryPoint() {
            _super.apply(this, arguments);
        }
        EntryPoint.prototype.initialize = function () {
            MsPortalFx.Base.Diagnostics.Telemetry.initialize(ExtensionDefinition.definitionName, false);
            this.viewModelFactories = new ViewModelFactories.ViewModelFactoriesBase();
            this.viewModelFactories.Resource().setDataContextFactory("./Resource/ResourceArea", function (contextModule) { return new contextModule.DataContext(); });
        };
        EntryPoint.prototype.getDefinition = function () {
            return ExtensionDefinition.getDefinition();
        };
        return EntryPoint;
    }(MsPortalFx.Extension.EntryPointBase));
    exports.EntryPoint = EntryPoint;
    function create() {
        return new EntryPoint();
    }
    exports.create = create;
});
