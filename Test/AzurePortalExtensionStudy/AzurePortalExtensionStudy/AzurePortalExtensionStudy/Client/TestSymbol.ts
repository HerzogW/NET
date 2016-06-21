//let sym1 = Symbol();
//let sym2 = Symbol("NewKey");
//let sym3 = Symbol("newKey1");

//const getClassNameSymbol = Symbol();

//class C {
//    [getClassNameSymble]() {
//        return "C";
//    }
//}

//let c = new C();
//let className = c[getClassNameSymbol]();

let list = [4, 5, 6];
for (let i in list) {
    console.log(i);//1,2,3
}

for (let i of list) {
    console.log(i);//4,5,6
}

//let pets = new Set(["Car", "Dog", "Hamster"]);
//pets["species"] = "mammals";

//for (let pet in pets) {
//    console.log(pet);
//}

//for (let pet of pets) {
//    console.log(pet);
//}














