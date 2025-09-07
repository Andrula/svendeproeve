import type { Company } from "./Company";
import type { User } from "./user";

export interface License {
    licenseId: string;
    licenseKey: string;
    validFrom: string;
    validTo: string;
    user?: User;
    company: Company
}

export class LicenseModel {
    license: License;

    constructor(license: License) {
        this.license = license;
    }

    get data(): License {
        return this.license;
    }
}