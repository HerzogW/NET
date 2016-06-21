namespace Validation {
    const letterRegexp = /^[A-Za-z]+$/;
    export class LetterOnlyValidator implements StringValidator {
        isAcceptable(s: string) {
            return letterRegexp.test(s);
        }
    }
}