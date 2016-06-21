var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
function createSquare(config) {
    var newSquare = { color: "white", area: 100 };
    if (config.color) {
        newSquare.color = config.color;
    }
    if (config.width) {
        newSquare.area = config.width * config.width;
    }
    return newSquare;
}
var squareOptions = { colour: "red", width: 10 };
var mySquare = createSquare(squareOptions);
var mySearch;
mySearch = function (source, subString) {
    var result = source.search(subString);
    if (result == -1) {
        return false;
    }
    else {
        return true;
    }
};
var myArray;
myArray = ["Blob", "Fred"];
var myStr = myArray[2];
function createClock(ctor, hour, minute) {
    return new ctor(hour, minute);
}
var DigitalClock = (function () {
    function DigitalClock(h, m) {
    }
    DigitalClock.prototype.tick = function () {
        console.log("beep beep");
    };
    return DigitalClock;
}());
var AnalogClock = (function () {
    function AnalogClock(h, m) {
    }
    AnalogClock.prototype.tick = function () {
        console.log("tick tick");
    };
    return AnalogClock;
}());
var digital = createClock(DigitalClock, 12, 17);
var analog = createClock(AnalogClock, 7, 32);
var clockCtor;
var tempClock = new clockCtor(1, 2);
var square = {};
square.color = "blue";
square.sideLength = 10;
square.penWidth = 5.0;
function getCounter() {
    var counter = function (start) { };
    counter.interval = 123;
    counter.reset = function () { };
    return counter;
}
var c = getCounter();
c(10);
c.reset();
c.interval = 5.0;
var Control = (function () {
    function Control() {
    }
    return Control;
}());
var Button = (function (_super) {
    __extends(Button, _super);
    function Button() {
        _super.apply(this, arguments);
    }
    Button.prototype.select = function () { };
    return Button;
}(Control));
var TextBox = (function (_super) {
    __extends(TextBox, _super);
    function TextBox() {
        _super.apply(this, arguments);
    }
    TextBox.prototype.select = function () { };
    return TextBox;
}(Control));
var Image1 = (function (_super) {
    __extends(Image1, _super);
    function Image1() {
        _super.apply(this, arguments);
    }
    return Image1;
}(Control));
var Location1 = (function () {
    function Location1() {
    }
    Location1.prototype.select = function () { };
    return Location1;
}());
var Greeter = (function () {
    function Greeter(message) {
        this.greeting = message;
    }
    Greeter.prototype.greet = function () {
        return "Hello, " + this.greeting;
    };
    return Greeter;
}());
var greeter = new Greeter("World");
greeter.greet();
var Animal = (function () {
    function Animal(name) {
        this.name = name;
    }
    Animal.prototype.move = function (distanceInMeters) {
        if (distanceInMeters === void 0) { distanceInMeters = 0; }
        console.log('${this.name} moverd${distanceInMeters}m.');
    };
    return Animal;
}());
var Snake = (function (_super) {
    __extends(Snake, _super);
    function Snake(name) {
        _super.call(this, name);
    }
    Snake.prototype.move = function (distanceInMeters) {
        if (distanceInMeters === void 0) { distanceInMeters = 5; }
        console.log("Slightering...");
        _super.prototype.move.call(this, distanceInMeters);
    };
    return Snake;
}(Animal));
var Horse = (function (_super) {
    __extends(Horse, _super);
    function Horse(name) {
        _super.call(this, name);
    }
    Horse.prototype.move = function (distanceInMeters) {
        if (distanceInMeters === void 0) { distanceInMeters = 45; }
        console.log("Galloping...");
        _super.prototype.move.call(this, distanceInMeters);
    };
    return Horse;
}(Animal));
var sam = new Snake("Sammy the Python");
var tom = new Horse("Tommy the Palomino");
sam.move();
tom.move(34);
var Department = (function () {
    function Department(name) {
        this.name = name;
    }
    Department.prototype.printName = function () {
    };
    return Department;
}());
var AccountingDepartment = (function (_super) {
    __extends(AccountingDepartment, _super);
    function AccountingDepartment() {
        _super.call(this, "Accounting and Auditing");
    }
    AccountingDepartment.prototype.printMeeting = function () {
        console.log("The Accounting Department meets each Monday at 10am.");
    };
    AccountingDepartment.prototype.generateReports = function () {
        console.log("Generating accounting reports...");
    };
    return AccountingDepartment;
}(Department));
var department = new AccountingDepartment();
department.printName();
department.printMeeting();
var Greeter2 = (function () {
    function Greeter2() {
    }
    Greeter2.prototype.greet = function () {
        if (this.greeting) {
            return "Hello, " + this.greeting;
        }
        else {
            return Greeter2.standardGreeting;
        }
    };
    Greeter2.standardGreeting = "Hello, There";
    return Greeter2;
}());
var greeter1;
greeter1 = new Greeter2();
console.log(greeter1.greet());
var greeterMaker2 = Greeter2;
greeterMaker2.standardGreeting = "Hey, There";
Greeter2.standardGreeting = "Hey, Here";
var greeter3 = new greeterMaker2();
console.log(greeter3.greet());
function add(x, y) {
    return x + y;
}
var myAdd = function (x, y) { return x + y; };
myAdd(100, 200);
var z = 100;
function addToZ(x, y) {
    return x + y + z;
}
var yourAdd = function (x, y) { return x + y; };
function buildName(firstName, lastName) {
    if (lastName) {
        return firstName + " " + lastName;
    }
    else {
        return firstName;
    }
}
var result1 = buildName("Bob");
var result2 = buildName("Bob", "Adams");
var deck = {
    suits: ["hearts", "spades", "clubs", "diamonds"],
    cards: Array(52),
    createCardPicker: function () {
        var _this = this;
        return function () {
            var pickedCard = Math.floor(Math.random() * 52);
            var pickedSuit = Math.floor(pickedCard / 13);
            return { suit: _this.suits[pickedSuit], card: pickedCard % 13 };
        };
    }
};
var cardPicker = deck.createCardPicker();
var pickedCard = cardPicker();
alert("card:" + pickedCard.card + " of" + pickedCard.suit);
var suits = ["hearts", "spades", "clubs", "diamonds"];
function pickCards(x) {
    if (typeof x == "object") {
        var pockedCard = Math.floor(Math.random() * x.length);
        return pickedCard;
    }
    else if (typeof x == "number") {
        var pickedSuit = Math.floor(x / 13);
        return { suit: suits[pickedSuit], card: x % 13 };
    }
}
var myDeck = [{ suit: "diamonds", card: 2 }, { suit: "spades", card: 10 }, { suit: "hearts", card: 4 }];
var pickedCard1 = myDeck[pickCards(myDeck)];
alert("card: " + pickedCard1.card + " of" + pickedCard1.suit);
var pickedCard2 = pickCards(15);
alert("card: " + pickedCard2.card + " of" + pickedCard2.suit);
function identity(arg) {
    return arg;
}
var myIdentity = identity;
var myIdentity2 = identity;
var myIdentity3 = identity;
var myIdentity4 = identity;
var myIdentity5 = identity;
var GenericNumber = (function () {
    function GenericNumber() {
    }
    return GenericNumber;
}());
var myGenericNumber = new GenericNumber();
myGenericNumber.zeroValue = 0;
myGenericNumber.add = function (x, y) { return x + y; };
var stringGenericNumber = new GenericNumber();
stringGenericNumber.zeroValue = "";
stringGenericNumber.add = function (x, y) { return x + y; };
alert(stringGenericNumber.add(stringGenericNumber.zeroValue, "test"));
function loggingIdentity(args) {
    console.log(args.length);
    return args;
}
loggingIdentity({ length: 10, value: 3 });
var BeeKeeper = (function () {
    function BeeKeeper() {
    }
    return BeeKeeper;
}());
var ZooKeeper = (function () {
    function ZooKeeper() {
    }
    return ZooKeeper;
}());
var Animal2 = (function () {
    function Animal2() {
    }
    return Animal2;
}());
var Bee = (function (_super) {
    __extends(Bee, _super);
    function Bee() {
        _super.apply(this, arguments);
    }
    return Bee;
}(Animal2));
var Lion = (function (_super) {
    __extends(Lion, _super);
    function Lion() {
        _super.apply(this, arguments);
    }
    return Lion;
}(Animal2));
function findKeeper(a) {
    return a.prototype.keeper;
}
findKeeper(Lion).nameTag;
findKeeper(Bee).hasMask;
var Direction;
(function (Direction) {
    Direction[Direction["Up"] = 1] = "Up";
    Direction[Direction["Down"] = 2] = "Down";
    Direction[Direction["Left"] = 3] = "Left";
    Direction[Direction["Right"] = 4] = "Right";
})(Direction || (Direction = {}));
var directions = [Direction.Up, Direction.Down, Direction.Left, Direction.Right];
var Enum;
(function (Enum) {
    Enum[Enum["A"] = 1] = "A";
    Enum[Enum["B"] = 2] = "B";
    Enum[Enum["C"] = 2] = "C";
})(Enum || (Enum = {}));
var a = Enum.A;
var nameOfA = Enum[Enum.A];
window.onmousedown = function (mouseEvent) {
    console.log(mouseEvent.button);
};
var x = function (a) { return a; };
var y = function (b, s) { return b; };
y = x;
var items = [1, 2, 3];
items.forEach(function (item, index, array) { return console.log(item); });
items.forEach(function (item) { return console.log(item); });
var xx = function () { return ({ name: "Alice" }); };
var yy = function () { return ({ name: "Alice", location: "Seattle" }); };
xx = yy;
var EventType;
(function (EventType) {
    EventType[EventType["Mouse"] = 0] = "Mouse";
    EventType[EventType["Keyboard"] = 1] = "Keyboard";
})(EventType || (EventType = {}));
;
;
function listenEvent(eventType, handler) {
}
listenEvent(EventType.Mouse, function (e) { return console.log(e.x1 + "," + e.y1); });
listenEvent(EventType.Mouse, function (e) { return console.log(e.x1 + "," + e.y1); });
function invokeLater(args, callback) {
}
invokeLater([1, 2], function (x, y) { return console.log(x + ", " + y); });
invokeLater([1, 2], function (x, y) { return console.log(x + ", " + y); });
var NewAnimal = (function () {
    function NewAnimal(name, numFeet) {
    }
    return NewAnimal;
}());
var NewSize = (function () {
    function NewSize(numFeet) {
    }
    return NewSize;
}());
var aAnimal, sSize;
aAnimal = sSize;
sSize = aAnimal;
var x3;
var y3;
x3 = y3;
y3 = x3;
var identity2 = function (x) {
    return x;
};
var reverse2 = function (y) {
    return y;
};
identity2 = reverse2;
