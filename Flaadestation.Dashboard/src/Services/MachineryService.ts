import { useState, useEffect, useMemo } from 'react';
import { HttpClient } from './HttpClient';
import { type Machinery, MachineryModel } from '../Models/Machinery';
import type { MachineryFormData } from '../Components/MachineryComponent/MachineryModal';

interface CreateMachineryRequest extends MachineryFormData {
  companyId: string;
}

const httpClient = new HttpClient({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  timeout: 30000,
  withCredentials: true
});

export class MachineryService {
  httpClient: HttpClient;

  constructor(httpClient: HttpClient) {
    this.httpClient = httpClient;
  }

  async getAllMachinery(): Promise<MachineryModel[]> {
    const response = await this.httpClient.get<any>(`/Machinery/company`);
    console.log('Raw API response:', response);

    if (response.length > 0) {
      console.log('First machinery item:', response[0]);
      console.log('StorageItems for first item:', response[0].storageItems);
      console.log('Number of storage items:', response[0].storageItems?.length || 0);
    }

    const machinery = response as Machinery[];
    return machinery.map(item => new MachineryModel(item));
  }
  async deleteMachinery(id: string): Promise<void> {
    await this.httpClient.delete(`/Machinery/${id}`);
  }

  async getMachineryById(id: string): Promise<MachineryModel> {
    const machinery = await this.httpClient.get<Machinery>(`/Machinery/${id}`);
    return new MachineryModel(machinery);
  }

  async createMachinery(machineryData: MachineryFormData, companyId: string): Promise<MachineryModel> {
    const requestData: CreateMachineryRequest = {
      ...machineryData,
      companyId
    };

    const machinery = await this.httpClient.post<Machinery>('/Machinery', requestData);
    return new MachineryModel(machinery);
  }

  async updateMachinery(id: string, machineryData: MachineryFormData): Promise<MachineryModel> {
    const machinery = await this.httpClient.put<Machinery>(`/Machinery/${id}`, machineryData);
    return new MachineryModel(machinery);
  }
}

export const machineryService = new MachineryService(httpClient);

export { HttpError } from './HttpClient';
export { MachineryModel } from '../Models/Machinery';