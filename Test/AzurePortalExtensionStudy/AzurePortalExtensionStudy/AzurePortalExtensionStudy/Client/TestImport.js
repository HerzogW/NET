define(["require", "exports", "./TestExport"], function (require, exports, TestExport_1) {
    "use strict";
    var strings = ["Hello", "98052", "101"];
    strings.forEach(function (s) {
        console.log("\"" + s + "\" " + (TestExport_1.default(s) ? " matches" : " does not match"));
    });
});
