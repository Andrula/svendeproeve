import { HttpClient } from './httpClient';
import { type Employee, EmployeeModel } from '../Models/Employee';


const httpClient = new HttpClient({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  timeout: 30000,
});

export class EmployeeService {
  httpClient: HttpClient;

  constructor(httpClient: HttpClient) {
    this.httpClient = httpClient;
  }

  async getAllEmployees(companyId: string): Promise<EmployeeModel[]> {
    const employees = await this.httpClient.get<Employee[]>(`/Employee/company/${companyId}`);
    return employees.map(emp => new EmployeeModel(emp));
  }

  async deleteEmployee(id: string): Promise<void> {
    await this.httpClient.delete(`/Employee/${id}`);
  }
}


export const employeeService = new EmployeeService(httpClient);

export const DEFAULT_COMPANY_ID = import.meta.env.VITE_COMPANY_ID || '';
export { HttpError } from './httpClient';
export { EmployeeModel } from '../Models/Employee';