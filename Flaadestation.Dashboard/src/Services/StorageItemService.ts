import { HttpClient } from './HttpClient';
import { type StorageItem, StorageItemModel } from '../Models/StorageItem';
import type { CreateStorageItemRequest } from '../Components/StorageItemModal/StorageItemModal';

const httpClient = new HttpClient({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  timeout: 30000,
  withCredentials: true
});

export class StorageItemService {
  httpClient: HttpClient;

  constructor(httpClient: HttpClient) {
    this.httpClient = httpClient;
  }

  async getAllStorageItems(): Promise<StorageItemModel[]> {
    const storageItems = await this.httpClient.get<StorageItem[]>(`/StorageItem/company`);
    return storageItems.map(emp => new StorageItemModel(emp));
  }

  async deleteStorageItem(id: string): Promise<void> {
    await this.httpClient.delete(`/StorageItem/${id}`);
  }

 async getStorageItemById(id: string): Promise<StorageItemModel> {
    const storageItem = await this.httpClient.get<StorageItem>(`/StorageItem/${id}`);
    return new StorageItemModel(storageItem);
  }

  async getStorageItemByItemId(itemId: string): Promise<StorageItemModel> {
    const storageItem = await this.httpClient.get<StorageItem>(`/StorageItem/Item/${itemId}`);
    return new StorageItemModel(storageItem);
  }

   async getStorageItemByStorageId(storageId: string): Promise<StorageItemModel> {
    const storageItem = await this.httpClient.get<StorageItem>(`/StorageItem/Storage/${storageId}`);
    return new StorageItemModel(storageItem);
  }

  async createStorageItem(storageItemRequest: CreateStorageItemRequest): Promise<StorageItemModel> {
    
    const storageItem = await this.httpClient.post<StorageItem>('/StorageItem', storageItemRequest);
    return new StorageItemModel(storageItem);
  }

  async updateStorageItem(id: string, storageItemRequest: CreateStorageItemRequest): Promise<StorageItemModel> {
    const storageItem = await this.httpClient.put<StorageItem>(`/StorageItem/${id}`, storageItemRequest);
    return new StorageItemModel(storageItem);
  }
}


export const storageItemService = new StorageItemService(httpClient);

export { HttpError } from './HttpClient';
export { StorageItemModel } from '../Models/StorageItem';
