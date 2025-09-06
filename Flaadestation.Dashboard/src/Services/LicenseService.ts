import { type License, LicenseModel } from "../Models/License";
import { HttpClient } from "./HttpClient";

const httpClient = new HttpClient({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  timeout: 30000,
  withCredentials: true
});

export class LicenseService {
  httpClient: HttpClient;

  constructor(httpClient: HttpClient) {
    this.httpClient = httpClient;
  }

  async getLicensesByCompany(): Promise<LicenseModel[]> {
    const licenses = await this.httpClient.get<License[]>(`/License/company`)
    return licenses.map(license => new LicenseModel(license));
  }
}

export const licenseService = new LicenseService(httpClient);

export { HttpError } from './HttpClient';