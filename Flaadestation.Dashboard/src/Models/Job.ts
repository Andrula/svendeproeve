import type { Address } from "./Address";
import type { Customer } from "./Customer";

export interface Job {
    jobId: string;
    title: string;
    description: string;
    addressId: string;
    address: Address;
    scheduledStart: string;
    scheduledEnd: string;
    customers: Customer[];
    storage: Storage;
}

export class JobModel {
    job: Job

    constructor(job: Job) {
        this.job = job
    }

    get data(): Job {
        return this.job
    }
}