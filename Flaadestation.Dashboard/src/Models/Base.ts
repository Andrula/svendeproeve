import type { Address } from "./Address";

export interface Base {
    baseId: string;
    name: string;
    addressId: string;
    address: Address;
    storage: Storage;
}

export class BaseModel {
    base: Base

    constructor(job: Base) {
        this.base = job
    }

    get data(): Base {
        return this.base
    }
}