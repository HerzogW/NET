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
    var CreationLegalBladeViewModel = (function (_super) {
        __extends(CreationLegalBladeViewModel, _super);
        function CreationLegalBladeViewModel(container, initialState, dataContext) {
            var _this = this;
            _super.call(this, container);
            this.title = ko.observable(ClientResources.CreateBlade.LegalTerm.bladeTitle);
            this.subtitle = ko.observable("");
            this.icon = ko.observable(MsPortalFx.Base.Images.Gear());
            this.actionBar = new MsPortalFx.ViewModels.ActionBars.GenericActionBar.ViewModel(container);
            this.actionBar.actionButtonLabel(ClientResources.CreateBlade.LegalTerm.buttonText);
            this.parameterProvider = new MsPortalFx.ViewModels.ParameterProvider(container, {
                mapIncomingDataForEditScope: function (incomingData) {
                    _this._incomingForm = incomingData;
                    _this._incomingForm.agreed = true;
                    return _this._incomingForm;
                },
                mapOutgoingDataForCollector: function (editScopeData) {
                    return {
                        agreed: editScopeData.agreed,
                    };
                }
            });
            this.editScope = this.parameterProvider.editScope;
        }
        return CreationLegalBladeViewModel;
    })(MsPortalFx.ViewModels.Forms.Form.ViewModel);
    exports.CreationLegalBladeViewModel = CreationLegalBladeViewModel;
});
