import { HttpClient } from './HttpClient';
import { type Base, BaseModel } from '../Models/Base';

// interface CreateBaseRequest extends BaseFormData {
//   companyId: string;
// }

const httpClient = new HttpClient({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  timeout: 30000,
  withCredentials: true
});

export class BaseService {
  httpClient: HttpClient;

  constructor(httpClient: HttpClient) {
    this.httpClient = httpClient;
  }

  async getBasesByCompany(): Promise<BaseModel[]> {
    const bases = await this.httpClient.get<Base[]>(`/Base/company`);
    return bases.map(emp => new BaseModel(emp));
  }

  async deleteBase(id: string): Promise<void> {
    await this.httpClient.delete(`/Base/${id}`);
  }

 async getBaseById(id: string): Promise<BaseModel> {
    const base = await this.httpClient.get<Base>(`/Base/${id}`);
    return new BaseModel(base);
  }

//   async createBase(baseData: BaseFormData, companyId: string): Promise<BaseModel> {
//     const requestData: CreateBaseRequest = {
//       ...baseData,
//       companyId
//     };
    
//     const base = await this.httpClient.post<Base>('/Base', requestData);
//     return new BaseModel(base);
//   }

//   async updateBase(id: string, baseData: BaseFormData): Promise<BaseModel> {
//     const base = await this.httpClient.put<Base>(`/Base/${id}`, baseData);
//     return new BaseModel(base);
//   }
}


export const baseService = new BaseService(httpClient);

export { HttpError } from './HttpClient';
export { BaseModel } from '../Models/Base';
