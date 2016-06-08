interface SquareConfig {
    color?: string;
    width?: number;
    [propName: string]: any;
}


function createSquare(config: SquareConfig): { color: string; area: number; } {
    let newSquare = { color: "white", area: 100 };

    if (config.color) {
        newSquare.color = config.color;
    }
    if (config.width) {
        newSquare.area = config.width * config.width;
    }
    return newSquare;
}

let squareOptions = { colour: "red", width: 10 };

let mySquare = createSquare(squareOptions);

interface SearchFunc {
    (source: string, subString: string): boolean;
}

let mySearch: SearchFunc;

mySearch = function (source: string, subString: string): boolean {
    let result = source.search(subString);
    if (result == -1) {
        return false;
    }
    else {
        return true;
    }
}

interface StringArray {
    [index: number]: string;
}

let myArray: StringArray;
myArray = ["Blob", "Fred"];

let myStr: string = myArray[2];


interface NumberDictionary {
    [index: string]: number;
    length: number;
    //name: string;
}

//interface ClockInterface {
//    currentTime: Date;
//    setTime(d: Date);
//}

//class Clock implements ClockInterface {
//    currentTime: Date;

//    setTime(d: Date) {
//        this.currentTime = d;
//    }

//    constructor(h: number, m: number) {
//    }
//}

interface ClockConstructor {
    new (hour: number, minute: number): ClockInterface;
}

interface ClockInterface {
    tick(): void;
}

function createClock(ctor: ClockConstructor, hour: number, minute: number): ClockInterface {
    return new ctor(hour, minute);
}

class DigitalClock implements ClockInterface {
    constructor(h: number, m: number) { }
    tick() {
        console.log("beep beep");
    }
}

class AnalogClock implements ClockInterface {
    constructor(h: number, m: number) {
    }
    tick() {
        console.log("tick tick");
    }
}

let digital = createClock(DigitalClock, 12, 17);
let analog = createClock(AnalogClock, 7, 32);

let clockCtor: ClockConstructor;
let tempClock = new clockCtor(1, 2);

//class Clock implements ClockConstructor {
//    currentTime: Date;
//    constructor(h: number, m: number) { }
//    new(hour: number, minute: number) {

//    }
//}

interface Shape {
    color: string;
}

interface PenStroke {
    penWidth: number;
}

interface Square extends Shape, PenStroke {
    sideLength: number;
}

let square = <Square>{};
square.color = "blue";
square.sideLength = 10;
square.penWidth = 5.0;

interface Counter {
    (start: number): string;
    interval: number;
    reset(): void;
}

function getCounter(): Counter {
    let counter = <Counter>function (start: number) { };
    counter.interval = 123;
    counter.reset = function () { };
    return counter;
}

let c = getCounter();
c(10);
c.reset();
c.interval = 5.0;

class Control {
    private state: any;
}

interface SelectableControl extends Control {
    select(): void;
}

class Button extends Control {
    select() { }
}

class TextBox extends Control {
    select() { }
}

class Image1 extends Control {
}

class Location1 {
    select() { }
}


class Greeter {
    greeting: string;
    constructor(message: string) {
        this.greeting = message;
    }

    greet(): string {
        return "Hello, " + this.greeting;
    }
}

let greeter = new Greeter("World");
greeter.greet();


class Animal {
    constructor(public name: string) {
    }

    move(distanceInMeters: number = 0) {
        console.log('${this.name} moverd${distanceInMeters}m.');
    }
}

class Snake extends Animal {
    constructor(name: string) {
        super(name);
    }
    move(distanceInMeters = 5) {
        console.log("Slightering...");
        super.move(distanceInMeters);
    }
}

class Horse extends Animal {
    constructor(name: string) {
        super(name);
    }
    move(distanceInMeters = 45) {
        console.log("Galloping...");
        super.move(distanceInMeters);
    }
}

let sam = new Snake("Sammy the Python");
let tom: Animal = new Horse("Tommy the Palomino");

sam.move();
tom.move(34);


abstract class Department {
    constructor(public name: string) {
    }

    printName(): void {
    }

    abstract printMeeting(): void;
}

class AccountingDepartment extends Department {
    constructor() {
        super("Accounting and Auditing");
    }

    printMeeting(): void {
        console.log("The Accounting Department meets each Monday at 10am.");
    }

    generateReports(): void {
        console.log("Generating accounting reports...");
    }
}

let department: Department = new AccountingDepartment();
department.printName();
department.printMeeting();



class Greeter2 {
    static standardGreeting = "Hello, There";

    greeting: string;
    greet() {
        if (this.greeting) {
            return "Hello, " + this.greeting;
        }
        else {
            return Greeter2.standardGreeting;
        }
    }
}

let greeter1: Greeter2;
greeter1 = new Greeter2();
console.log(greeter1.greet());

let greeterMaker2: typeof Greeter2 = Greeter2;
greeterMaker2.standardGreeting = "Hey, There";
Greeter2.standardGreeting = "Hey, Here";


let greeter3: Greeter2 = new greeterMaker2();
console.log(greeter3.greet());


//FUNCTION

function add(x: number, y: number): number {
    return x + y;
}

let myAdd = function (x: number, y: number): number
{ return x + y; }
myAdd(100, 200);

let z = 100;

function addToZ(x: number, y: number) {
    return x + y + z;
}

let yourAdd: (baseValue: number, increment: number) => number =
    function (x: number, y: number): number { return x + y; };

function buildName(firstName: string, lastName?: string) {
    if (lastName) {
        return firstName + " " + lastName;
    }
    else {
        return firstName;
    }
}

let result1 = buildName("Bob");
let result2 = buildName("Bob", "Adams");

let deck = {
    suits: ["hearts", "spades", "clubs", "diamonds"],
    cards: Array(52),
    createCardPicker: function () {
        return () => {
            let pickedCard = Math.floor(Math.random() * 52);
            let pickedSuit = Math.floor(pickedCard / 13);

            return { suit: this.suits[pickedSuit], card: pickedCard % 13 };
        }
    }
}

let cardPicker = deck.createCardPicker();
let pickedCard = cardPicker();
alert("card:" + pickedCard.card + " of" + pickedCard.suit);

let suits = ["hearts", "spades", "clubs", "diamonds"];




function pickCards(x: { suit: string; card: number; }[]): number;
function pickCards(x: number): { suit: string, card: number };
function pickCards(x: any): any {
    if (typeof x == "object") {
        let pockedCard = Math.floor(Math.random() * x.length);
        return pickedCard;
    }
    else if (typeof x == "number") {
        let pickedSuit = Math.floor(x / 13);
        return { suit: suits[pickedSuit], card: x % 13 };
    }
}

let myDeck = [{ suit: "diamonds", card: 2 }, { suit: "spades", card: 10 }, { suit: "hearts", card: 4 }];
let pickedCard1 = myDeck[pickCards(myDeck)];
alert("card: " + pickedCard1.card + " of" + pickedCard1.suit);

let pickedCard2 = pickCards(15);
alert("card: " + pickedCard2.card + " of" + pickedCard2.suit);







