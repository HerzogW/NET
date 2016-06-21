var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports"], function (require, exports) {
    "use strict";
    var ViewModelFactories;
    (function (ViewModelFactories) {
        var QuickStartViewModelFactoriesBase = (function (_super) {
            __extends(QuickStartViewModelFactoriesBase, _super);
            function QuickStartViewModelFactoriesBase() {
                _super.apply(this, arguments);
            }
            QuickStartViewModelFactoriesBase.prototype.QuickStartPartViewModel = function (container, initialState) {
                var _this = this;
                return this.loadViewModelAsync("../QuickStart/ViewModels/QuickStartPartViewModel", function (providerModule) { return new providerModule.QuickStartPartViewModel(container, initialState, _this.dataContext); }, require);
            };
            QuickStartViewModelFactoriesBase.prototype.QuickStartBladeViewModel = function (container, initialState) {
                var _this = this;
                return this.loadViewModelAsync("../QuickStart/ViewModels/QuickStartBladeViewModel", function (providerModule) { return new providerModule.QuickStartBladeViewModel(container, initialState, _this.dataContext); }, require);
            };
            QuickStartViewModelFactoriesBase.prototype.QuickStartInfoListViewModel = function (container, initialState) {
                var _this = this;
                return this.loadViewModelAsync("../QuickStart/ViewModels/QuickStartInfoListViewModel", function (providerModule) { return new providerModule.QuickStartInfoListViewModel(container, initialState, _this.dataContext); }, require);
            };
            return QuickStartViewModelFactoriesBase;
        }(FxImpl.Extension.ViewModelAreaFactoriesBase));
        ViewModelFactories.QuickStartViewModelFactoriesBase = QuickStartViewModelFactoriesBase;
        var BrowseViewModelFactoriesBase = (function (_super) {
            __extends(BrowseViewModelFactoriesBase, _super);
            function BrowseViewModelFactoriesBase() {
                _super.apply(this, arguments);
            }
            BrowseViewModelFactoriesBase.prototype.ResourcePartViewModel = function (container, initialState) {
                var _this = this;
                return this.loadViewModelAsync("../Browse/ViewModels/ResourcePartViewModel", function (providerModule) { return new providerModule.ResourcePartViewModel(container, initialState, _this.dataContext); }, require);
            };
            BrowseViewModelFactoriesBase.prototype.AssetTypeViewModel = function (container, initialState) {
                var _this = this;
                return this.loadViewModelAsync("../Browse/ViewModels/AssetTypeViewModel", function (providerModule) { return new providerModule.AssetTypeViewModel(container, initialState, _this.dataContext); }, require);
            };
            return BrowseViewModelFactoriesBase;
        }(FxImpl.Extension.ViewModelAreaFactoriesBase));
        ViewModelFactories.BrowseViewModelFactoriesBase = BrowseViewModelFactoriesBase;
        var ResourceViewModelFactoriesBase = (function (_super) {
            __extends(ResourceViewModelFactoriesBase, _super);
            function ResourceViewModelFactoriesBase() {
                _super.apply(this, arguments);
            }
            ResourceViewModelFactoriesBase.prototype.PropertiesBladeViewModel = function (container, initialState) {
                var _this = this;
                return this.loadViewModelAsync("../Resource/Properties/ViewModels/PropertiesBladeViewModel", function (providerModule) { return new providerModule.PropertiesBladeViewModel(container, initialState, _this.dataContext); }, require);
            };
            ResourceViewModelFactoriesBase.prototype.PropertiesPartViewModel = function (container, initialState) {
                var _this = this;
                return this.loadViewModelAsync("../Resource/Properties/ViewModels/PropertiesPartViewModel", function (providerModule) { return new providerModule.PropertiesPartViewModel(container, initialState, _this.dataContext); }, require);
            };
            ResourceViewModelFactoriesBase.prototype.ResourceBladeViewModel = function (container, initialState) {
                var _this = this;
                return this.loadViewModelAsync("../Resource/Summary/ViewModels/ResourceBladeViewModel", function (providerModule) { return new providerModule.ResourceBladeViewModel(container, initialState, _this.dataContext); }, require);
            };
            ResourceViewModelFactoriesBase.prototype.ResourceSummaryPartViewModel = function (container, initialState) {
                var _this = this;
                return this.loadViewModelAsync("../Resource/Summary/ViewModels/ResourceSummaryPartViewModel", function (providerModule) { return new providerModule.ResourceSummaryPartViewModel(container, initialState, _this.dataContext); }, require);
            };
            ResourceViewModelFactoriesBase.prototype.SettingsBladeViewModel = function (container, initialState) {
                var _this = this;
                return this.loadViewModelAsync("../Resource/Settings/ViewModels/SettingsBladeViewModel", function (providerModule) { return new providerModule.SettingsBladeViewModel(container, initialState, _this.dataContext); }, require);
            };
            ResourceViewModelFactoriesBase.prototype.SettingsPartViewModel = function (container, initialState) {
                var _this = this;
                return this.loadViewModelAsync("../Resource/Settings/ViewModels/SettingsPartViewModel", function (providerModule) { return new providerModule.SettingsPartViewModel(container, initialState, _this.dataContext); }, require);
            };
            ResourceViewModelFactoriesBase.prototype.CreateBladeViewModel = function (container, initialState) {
                var _this = this;
                return this.loadViewModelAsync("../Resource/Create/ViewModels/CreateBladeViewModel", function (providerModule) { return new providerModule.CreateBladeViewModel(container, initialState, _this.dataContext); }, require);
            };
            return ResourceViewModelFactoriesBase;
        }(FxImpl.Extension.ViewModelAreaFactoriesBase));
        ViewModelFactories.ResourceViewModelFactoriesBase = ResourceViewModelFactoriesBase;
        var ViewModelFactoriesBase = (function () {
            function ViewModelFactoriesBase() {
            }
            ViewModelFactoriesBase.prototype.SetQuickStartViewModelFactories = function (factories) {
                this._QuickStartViewModelFactories = factories;
            };
            ViewModelFactoriesBase.prototype.QuickStart = function () {
                this._QuickStartViewModelFactories = this._QuickStartViewModelFactories || new QuickStartViewModelFactoriesBase();
                return this._QuickStartViewModelFactories;
            };
            ViewModelFactoriesBase.prototype.QuickStart$QuickStartPartViewModel = function (container, initialState) {
                return this.QuickStart().QuickStartPartViewModel(container, initialState);
            };
            ViewModelFactoriesBase.prototype.SetBrowseViewModelFactories = function (factories) {
                this._BrowseViewModelFactories = factories;
            };
            ViewModelFactoriesBase.prototype.Browse = function () {
                this._BrowseViewModelFactories = this._BrowseViewModelFactories || new BrowseViewModelFactoriesBase();
                return this._BrowseViewModelFactories;
            };
            ViewModelFactoriesBase.prototype.Browse$ResourcePartViewModel = function (container, initialState) {
                return this.Browse().ResourcePartViewModel(container, initialState);
            };
            ViewModelFactoriesBase.prototype.SetResourceViewModelFactories = function (factories) {
                this._ResourceViewModelFactories = factories;
            };
            ViewModelFactoriesBase.prototype.Resource = function () {
                this._ResourceViewModelFactories = this._ResourceViewModelFactories || new ResourceViewModelFactoriesBase();
                return this._ResourceViewModelFactories;
            };
            ViewModelFactoriesBase.prototype.Resource$PropertiesBladeViewModel = function (container, initialState) {
                return this.Resource().PropertiesBladeViewModel(container, initialState);
            };
            ViewModelFactoriesBase.prototype.Resource$PropertiesPartViewModel = function (container, initialState) {
                return this.Resource().PropertiesPartViewModel(container, initialState);
            };
            ViewModelFactoriesBase.prototype.Resource$ResourceBladeViewModel = function (container, initialState) {
                return this.Resource().ResourceBladeViewModel(container, initialState);
            };
            ViewModelFactoriesBase.prototype.Resource$ResourceSummaryPartViewModel = function (container, initialState) {
                return this.Resource().ResourceSummaryPartViewModel(container, initialState);
            };
            ViewModelFactoriesBase.prototype.QuickStart$QuickStartBladeViewModel = function (container, initialState) {
                return this.QuickStart().QuickStartBladeViewModel(container, initialState);
            };
            ViewModelFactoriesBase.prototype.QuickStart$QuickStartInfoListViewModel = function (container, initialState) {
                return this.QuickStart().QuickStartInfoListViewModel(container, initialState);
            };
            ViewModelFactoriesBase.prototype.Resource$SettingsBladeViewModel = function (container, initialState) {
                return this.Resource().SettingsBladeViewModel(container, initialState);
            };
            ViewModelFactoriesBase.prototype.Resource$SettingsPartViewModel = function (container, initialState) {
                return this.Resource().SettingsPartViewModel(container, initialState);
            };
            ViewModelFactoriesBase.prototype.Resource$CreateBladeViewModel = function (container, initialState) {
                return this.Resource().CreateBladeViewModel(container, initialState);
            };
            ViewModelFactoriesBase.prototype.Browse$AssetTypeViewModel = function (container, initialState) {
                return this.Browse().AssetTypeViewModel(container, initialState);
            };
            return ViewModelFactoriesBase;
        }());
        ViewModelFactories.ViewModelFactoriesBase = ViewModelFactoriesBase;
    })(ViewModelFactories || (ViewModelFactories = {}));
    return ViewModelFactories;
});
