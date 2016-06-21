import Validate from "./TestExport";
let strings = ["Hello", "98052", "101"];

strings.forEach(s => {
    console.log(`"${s}" ${Validate(s)?" matches":" does not match"}`);
});