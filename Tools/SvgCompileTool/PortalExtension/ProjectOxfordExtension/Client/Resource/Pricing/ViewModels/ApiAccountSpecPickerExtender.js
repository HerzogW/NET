/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
"use strict";
define(["require", "exports", "../../../Shared/Utilities"], function (require, exports, Utilities) {
    var ApiAccountSpecPickerExtender = (function () {
        function ApiAccountSpecPickerExtender(container, initialState, dataContext) {
            var _this = this;
            this.input = ko.observable();
            this.output = ko.observable();
            this._specData = ko.observable();
            this._disabledSpecs = new Array();
            this._resourcesPerSubscription = dataContext.resourceData.apiEntitiesPerSubscription.createView(container, { interceptNotFound: false });
            this.input.subscribe(container, function (value) {
                var apiType = value.options.apiType;
                var subscriptionId = value.options.subscriptionId;
                var specData = dataContext.createData.getSpecData(apiType);
                var promise = _this._resourcesPerSubscription.fetch(subscriptionId).then(function () {
                    if (_this._resourcesPerSubscription.item()) {
                        var specs = Utilities.getDisabledSpecsByApiType(_this._resourcesPerSubscription.item(), apiType);
                        var specsToDisable = MsPortalFx.remove(specs, function (v) {
                            return v.specId == value.selectedSpecId;
                        });
                        var output = { specData: specData, disabledSpecs: specs };
                        _this.output(output);
                    }
                    else {
                        var output = { specData: specData };
                        _this.output(output);
                    }
                }).catch(function (reason) {
                    // when current promise exception, just output original source to unblock UI.
                    var output = { specData: specData };
                    _this.output(output);
                });
            });
        }
        return ApiAccountSpecPickerExtender;
    })();
    exports.ApiAccountSpecPickerExtender = ApiAccountSpecPickerExtender;
});
