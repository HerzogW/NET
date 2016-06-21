export class Observable<T>{

}

declare global {
    interface Array<T> {
        toObservable(): Observable<T>;    
    }
}

Array.prototype.toObservable = function () {
    return new Observable<any>();
};