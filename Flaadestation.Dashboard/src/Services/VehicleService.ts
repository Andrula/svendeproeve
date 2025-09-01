import { type Vehicle, VehicleModel } from "../Models/Vehicle";
import { HttpClient } from "./HttpClient";

const httpClient = new HttpClient({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  timeout: 30000,
});

export class VehicleService {
  httpClient: HttpClient;

  constructor(httpClient: HttpClient) {
    this.httpClient = httpClient;
  }

  async getAllVehicles(companyId: string): Promise<VehicleModel[]> {
    const vehicles = await this.httpClient.get<Vehicle[]>(
      `/Vehicle/company/${companyId}`
    );
    return vehicles.map((vehicle) => new VehicleModel(vehicle));
  }

  async deleteVehicle(id: string): Promise<void> {
    await this.httpClient.delete(`/Vehicle/${id}`);
  }
}
export const vehicleService = new VehicleService(httpClient);

export const DEFAULT_COMPANY_ID = import.meta.env.VITE_COMPANY_ID || '';
export { HttpError } from './HttpClient';
export { VehicleModel } from "../Models/Vehicle"
