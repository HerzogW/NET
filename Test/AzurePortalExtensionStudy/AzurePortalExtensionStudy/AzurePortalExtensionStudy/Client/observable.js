define(["require", "exports"], function (require, exports) {
    "use strict";
    var Observable = (function () {
        function Observable() {
        }
        return Observable;
    }());
    exports.Observable = Observable;
    Array.prototype.toObservable = function () {
        return new Observable();
    };
});
