let strings = ["Hello", "98765", "110"];

let validators: { [s: string]: Validation.StringValidator } = {};
validators["ZIP Code"] = new Validation.ZipCodeValidator();
validators["Letters only"] = new Validation.LetterOnlyValidator();

for (let s of strings) {
    for (let name in validators) {
        let isMatch = validators[name].isAcceptable(s);
        //console.log(`'${s}' ${isMatch ? "matches" : "does not match"} '${name}'.`);
        console.log(s + (validators[name].isAcceptable(s) ? " matches" : " does not match ") + name);
    }
}