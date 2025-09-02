import { HttpClient } from '../Services/httpClient';
import { type Employee, EmployeeModel } from '../Models/Employee';
import type { EmployeeFormData } from '../Components/EmployeeComponent/EmployeeModal';

interface CreateEmployeeRequest extends EmployeeFormData {
  companyId: string;
}

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

 async getEmployeeById(id: string): Promise<EmployeeModel> {
    const employee = await this.httpClient.get<Employee>(`/Employee/${id}`);
    return new EmployeeModel(employee);
  }

  async createEmployee(employeeData: EmployeeFormData, companyId: string): Promise<EmployeeModel> {
    const requestData: CreateEmployeeRequest = {
      ...employeeData,
      companyId
    };
    
    const employee = await this.httpClient.post<Employee>('/Employee', requestData);
    return new EmployeeModel(employee);
  }

  async updateEmployee(id: string, employeeData: EmployeeFormData): Promise<EmployeeModel> {
    const employee = await this.httpClient.put<Employee>(`/Employee/${id}`, employeeData);
    return new EmployeeModel(employee);
  }
}


export const employeeService = new EmployeeService(httpClient);

export const DEFAULT_COMPANY_ID = import.meta.env.VITE_COMPANY_ID || '';
export { HttpError } from './HttpClient';
export { EmployeeModel } from '../Models/Employee';