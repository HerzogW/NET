var strings = ["Hello", "98765", "110"];
var validators = {};
validators["ZIP Code"] = new Validation.ZipCodeValidator();
validators["Letters only"] = new Validation.LetterOnlyValidator();
for (var _i = 0, strings_1 = strings; _i < strings_1.length; _i++) {
    var s_1 = strings_1[_i];
    for (var name_1 in validators) {
        var isMatch = validators[name_1].isAcceptable(s_1);
        console.log(s_1 + (validators[name_1].isAcceptable(s_1) ? " matches" : " does not match ") + name_1);
    }
}
