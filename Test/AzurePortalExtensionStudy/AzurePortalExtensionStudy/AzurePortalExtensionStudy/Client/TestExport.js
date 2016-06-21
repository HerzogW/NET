define(["require", "exports"], function (require, exports) {
    "use strict";
    var numberRegexp = /^[0-9]+$/;
    function default_1(s) {
        return s.length === 5 && numberRegexp.test(s);
    }
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.default = default_1;
});
