define(["require", "exports", "./ZipCodeValidator", "./LettersOnlyValidator"], function (require, exports, ZipCodeValidator_1, LettersOnlyValidator_1) {
    "use strict";
    var strings = ["Hello", "980582", "101"];
    var validators = {};
    validators["ZIP Code"] = new ZipCodeValidator_1.ZipCodeValidator();
    validators["Letters Only"] = new LettersOnlyValidator_1.LettersOnlyValidator();
    strings.forEach(function (s) {
        for (var name_1 in validators) {
            console.log('"${s} - ${ validators[name].isAcceptable(s)? "matches":"does not match"} ${name}');
        }
    });
});
