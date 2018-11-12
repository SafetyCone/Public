import * as TL2 from "@rivetanomalytics/testlib2"
// import "../../node_modules/@rivetanomalytics/common/dist/Extensions/Array";
// import "@rivetanomalytics/common/dist/Extensions/Array"
import "@rivetanomalytics/common"

// import { SayHello } from "@rivetanomalytics/common"

TL2.sayHello();
TL2.sayGoodbye();

let numberArray: number[] = [1, 2, 3, -1];
let min = numberArray.minimum();
console.log(min);

// SayHello();

// let stringArray: string[];
// stringArray.min