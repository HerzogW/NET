//Advanced type
//-----Union Type
function padLeft(value: string, padding: string | number) {
    if (typeof padding === "number") {
        return Array(padding + 1).join(" ") + value;
    }
    if (typeof padding === "string") {
        return padding + value;
    }
    throw new Error("Expected string or number, got '${padding}'.");
}

padLeft("Hello world", 5);

interface Bird {
    fly(): void;
    layEggs(): void;
    sleep(): void;
}

interface Fish {
    swim(): void;
    layEggs(): void;
    sleep(): void;
}

function getSmallPet(): Fish | Bird {
    return;
}

let pet = getSmallPet();
pet.layEggs();
pet.sleep();
(<Bird>pet).fly();

if ((<Bird>pet).fly) {
    (<Bird>pet).fly();
}
else if ((<Fish>pet).swim) {
    (<Fish>pet).swim();
}

function isFish(pet: Fish | Bird): pet is Fish {
    return (<Fish>pet).swim !== undefined;
}

if (isFish(pet)) {
    pet.swim();
}
else {
    pet.fly();
}


function isNumber(x: any): x is number {
    return typeof x === "number";
}

function isString(x: any): x is string {
    return typeof x === "string";
}

function newPadLeft(value: string, padding: string | number) {
    if (isNumber(padding)) {
        return Array(padding + 1).join(" ") + value;
    }
    if (isString(padding)) {
        return padding + value;
    }
    throw new Error("Excepted string or number, got '${padding}'.");
}


interface Padder {
    getPaddingString(): string;
}

class SpaceRepeatingPadder implements Padder {
    constructor(private numSpaces: number) { }
    getPaddingString() {
        return Array(this.numSpaces + 1).join(" ");
    }
}

class StringPadder implements Padder {
    constructor(private value: string) { }
    getPaddingString() {
        return this.value;
    }
}

function getRandomPadder() {
    return Math.random() < 0.5 ?
        new SpaceRepeatingPadder(4) :
        new StringPadder(" ");
}

let padder: Padder = getRandomPadder();
if (padder instanceof SpaceRepeatingPadder) {
    padder;
}

if (padder instanceof StringPadder) {
    padder;
}

////Intersection Type

function extend<T, U>(first: T, second: U): T & U {
    let result = <T & U>{};
    for (let id in first) {
        (<any>result)[id] = (<any>first)[id];
    }
    for (let id in second) {
        if (!result.hasOwnProperty(id)) {
            (<any>result)[id] = (<any>second)[id];
        }
    }
    return result;
}

class Person {
    constructor(public name: string) { }
}

interface Loggable {
    log(): void;
}
class ConsoleLogger implements Loggable {
    log() {
        console.log("**********");
    }
}

let jim = extend(new Person("Jim"), new ConsoleLogger());
let n = jim.name;
jim.log();


////Type Aliases

type Name = string;
type NameResolver = (x: string) => string;
type NameOrResolver = Name | NameResolver;
function getName(n: NameOrResolver): Name {
    if (typeof n === "string") {
        return n;
    }
    else {
        return n("test");
    }
}

type Container<T> = { value: T };

type Tree<T> = {
    value: T;
    left: Tree<T>;
    right: Tree<T>;
}

type LinkedList<T> = T & { next: LinkedList<T> };

interface NewPerson {
    name: string;
}

let people: LinkedList<NewPerson>;
var s = people.name;
s = people.next.name;
s = people.next.next.name;
s = people.next.next.next.name;

////String Literal Types

type Easing = "ease-in" | "ease-out" | "ease-in-out";
class UIElement {
    animate(dx: number, dy: number, easing: Easing) {
        if (easing === "ease-in") {
        }
        else if (easing === "ease-out") {
        }
        else if (easing === "ease-in-out") {
        }
        else {
        }
    }
}

let button = new UIElement();
button.animate(0, 0, "ease-in");
button.animate(0, 0, "ease-in-out");


////Polymorphic this types

class BasicCalculator {
    public constructor(protected value: number = 0) { }
    public currentValue(): number {
        return this.value;
    }
    public add(operand: number): this {
        this.value += operand;
        return this;
    }
    public multiply(operand: number): this {
        this.value *= operand;
        return this;
    }
}

let v = new BasicCalculator(2).multiply(5).add(1).currentValue();

class ScientificCalculator extends BasicCalculator {
    public constructor(value = 0) {
        super(value);
    }
    public sin(): this {
        this.value = Math.sin(this.value);
        return this;
    }
}

let v2 = new ScientificCalculator(2).multiply(5).sin().add(1).currentValue();












