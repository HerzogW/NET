/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
define(["require", "exports"], function (require, exports) {
    "use strict";
    var PricingTierPartAdapterViewModel = (function () {
        function PricingTierPartAdapterViewModel(container, initialState, dataContext) {
            this.apiAccountId = ko.observable();
        }
        PricingTierPartAdapterViewModel.prototype.onInputsSet = function (inputs) {
            this.apiAccountId(inputs.id);
            return Q();
        };
        return PricingTierPartAdapterViewModel;
    })();
    exports.PricingTierPartAdapterViewModel = PricingTierPartAdapterViewModel;
});
