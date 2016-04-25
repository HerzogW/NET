/**
 * Copyright (c) Microsoft Corporation.  All rights reserved.
 */
define(["require", "exports"], function (require, exports) {
    "use strict";
    var EventsPartAdapterViewModel = (function () {
        function EventsPartAdapterViewModel(container, initialState, dataContext) {
            this.apiAccountId = ko.observable();
            this.eventsOptions = ko.observable({});
        }
        EventsPartAdapterViewModel.prototype.onInputsSet = function (inputs) {
            this.apiAccountId(inputs.id);
            return Q();
        };
        return EventsPartAdapterViewModel;
    })();
    exports.EventsPartAdapterViewModel = EventsPartAdapterViewModel;
});
