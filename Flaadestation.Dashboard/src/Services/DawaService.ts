import { AddressModel } from "../Models/Address";
import type { BaseModel } from "./BaseService";
import { HttpClient } from "./HttpClient";
import type { JobModel } from "./JobService";

const httpClient = new HttpClient({
  baseURL: import.meta.env.VITE_DAWA_BASE_URL,
  timeout: 30000
});

export interface DawaSearchResponse {
  tekst: string;
  adresse: DawaMiniResponse;
}

export interface DawaMiniResponse {
    id: string;
    status: number;
    darstatus: number;
    vejkode: string;
    vejnavn: string;
    adresseringsvejnavn: string;
    husnr: string;
    etage: string | null;
    dør: string | null;
    supplerendebynavn: string | null;
    postnr: string;
    postnrnavn: string;
    stormodtagerpostnr: string | null;
    stormodtagerpostnrnavn: string | null;
    kommunekode: string;
    adgangsadresseid: string;
    x: number;
    y: number;
    href: string;
}

export class DawaService {
    httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
        this.httpClient = httpClient;
    }

    async getAutoComplete(query: string): Promise<AddressModel[]> {
        const response = await this.httpClient.get<DawaSearchResponse[]>(`/adresser/autocomplete?q=${query}&per_side=10`);
        return response.map(res => new AddressModel(res.adresse))
    }

    async getAddressById(addressId: string): Promise<AddressModel> {
        const response = await this.httpClient.get<DawaSearchResponse>(`/adresser/${addressId}`);
        return new AddressModel(response.adresse)
    }

    async fillAddressesOnJobs(jobs: JobModel[]): Promise<void> {
        const addressIds = jobs.map(j => j.data.addressId);
        const response = await this.httpClient.get<DawaMiniResponse[]>(`/adresser?id=${addressIds.join('|')}&struktur=mini`)
        
        jobs.forEach(job => {
            const address = response.find(dr => dr.id == job.data.addressId)
            
            if (address) {
                job.data.address = new AddressModel(address);
            }
        });
    }

    async fillAddressesOnBases(bases: BaseModel[]): Promise<void> {
        const addressIds = bases.map(b => b.data.addressId);
        const response = await this.httpClient.get<DawaMiniResponse[]>(`/adresser?id=${addressIds.join('|')}&struktur=mini`)
        
        bases.forEach(base => {
            const address = response.find(dr => dr.id == base.data.addressId)
            
            if (address) {
                base.data.address = new AddressModel(address);
            }
        });
    }
    

    

}

export const dawaService = new DawaService(httpClient);

export { HttpError } from './HttpClient';
export { AddressModel } from '../Models/Address'