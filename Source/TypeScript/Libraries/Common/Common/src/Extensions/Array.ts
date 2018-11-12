declare global {
    export interface Array<T> {
        minimum(this: number[]): number;
    }
}

Array.prototype.minimum = function minimum(this: number[]): number {
    let output: number = this.reduce((previousValue: number, currentValue: number) => {
        if (previousValue < currentValue) {
            return previousValue;
        } else {
            return currentValue;
        }
    });
    return output;
}

export {};