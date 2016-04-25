/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
define(["require", "exports", "../../../Shared/Utilities"], function (require, exports, Utilities) {
    var InfoList = MsPortalFx.ViewModels.Parts.InfoList;
    var QuickStartPartViewModel = (function (_super) {
        __extends(QuickStartPartViewModel, _super);
        function QuickStartPartViewModel(container, initialState, dataContext) {
            this._accountView = dataContext.resourceData.resourceEntities.createView(container);
            this._apiQuickStartsView = dataContext.apiData.apiQuickStartEntities.createView(container, { interceptNotFound: false });
            this._resourceId = ko.observable();
            _super.call(this, initialState);
        }
        QuickStartPartViewModel.prototype.onInputsSet = function (inputs, settings) {
            var _this = this;
            this._resourceId(inputs.id);
            return this._accountView.fetch(inputs.id).then(function () {
                _this.populateSections();
            });
        };
        QuickStartPartViewModel.prototype.populateSections = function () {
            var _this = this;
            this._apiQuickStartsView.fetch(this._accountView.item().kind()).then(function () {
                if (_this._apiQuickStartsView.item()) {
                    var metaData = ko.toJS(_this._apiQuickStartsView.item());
                    for (var i = 0; i < metaData.quickStarts.length; i++) {
                        var links = metaData.quickStarts[i].links;
                        var qLinks = new Array();
                        for (var j = 0; j < links.length; j++) {
                            qLinks.push(new InfoList.Link(links[j].text, links[j].uri));
                        }
                        _this.addSection(metaData.quickStarts[i].title, metaData.quickStarts[i].description, qLinks, Utilities.getBaseImage(metaData.quickStarts[i].icon));
                    }
                }
            });
        };
        return QuickStartPartViewModel;
    })(MsPortalFx.ViewModels.Parts.InfoList.ViewModel);
    exports.QuickStartPartViewModel = QuickStartPartViewModel;
});
