import type { VehicleFormData } from "../Components/VehicleComponent/VehicleModal";
import { type Vehicle, VehicleModel } from "../Models/Vehicle";
import { HttpClient } from "./HttpClient";

interface CreateVehicleRequest extends VehicleFormData {
  companyId: string;
}

const httpClient = new HttpClient({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  timeout: 30000,
  withCredentials: true
});

export class VehicleService {
  httpClient: HttpClient;

  constructor(httpClient: HttpClient) {
    this.httpClient = httpClient;
  }

  async getAllVehicles(): Promise<VehicleModel[]> {
    const vehicles = await this.httpClient.get<Vehicle[]>(
      `/Vehicle/company`
    );
    return vehicles.map((vehicle) => new VehicleModel(vehicle));
  }

  async deleteVehicle(id: string): Promise<void> {
    await this.httpClient.delete(`/Vehicle/${id}`);
  }

    async createVehicle(vehicleData: VehicleFormData, companyId: string): Promise<VehicleModel> {
      const requestData: CreateVehicleRequest = {
        ...vehicleData,
        companyId
      };
  
      const vehicle = await this.httpClient.post<Vehicle>('/Vehicle', requestData);
      return new VehicleModel(vehicle);
    }
  
    async updateVehicle(id: string, vehicleData: VehicleFormData): Promise<VehicleModel> {
      const vehicle = await this.httpClient.put<Vehicle>(`/Vehicle/${id}`, vehicleData);
      return new VehicleModel(vehicle);
    }
}

export const vehicleService = new VehicleService(httpClient);

export { HttpError } from './HttpClient';
export { VehicleModel } from "../Models/Vehicle"
