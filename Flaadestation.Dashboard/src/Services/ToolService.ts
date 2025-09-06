import { HttpClient } from './HttpClient';
import { type Tool, ToolModel } from '../Models/Tool';
import type { ToolFormData } from '../Components/ToolComponent/ToolModal';

interface CreateToolRequest extends ToolFormData {
  companyId: string;
}

const httpClient = new HttpClient({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  timeout: 30000,
  withCredentials: true
});

export class ToolService {
  httpClient: HttpClient;

  constructor(httpClient: HttpClient) {
    this.httpClient = httpClient;
  }

  async getAllTools(): Promise<ToolModel[]> {
    const tools = await this.httpClient.get<Tool[]>(`/Tool/company`);
    return tools.map(tool => new ToolModel(tool));
  }

  async deleteTool(id: string): Promise<void> {
    await this.httpClient.delete(`/Tool/${id}`);
  }

  async getToolById(id: string): Promise<ToolModel> {
    const tool = await this.httpClient.get<Tool>(`/Tool/${id}`);
    return new ToolModel(tool);
  }

  async createTool(toolData: ToolFormData, companyId: string): Promise<ToolModel> {
    const requestData: CreateToolRequest = {
      ...toolData,
      companyId
    };
    
    const tool = await this.httpClient.post<Tool>('/Tool', requestData);
    return new ToolModel(tool);
  }

  async updateTool(id: string, toolData: ToolFormData): Promise<ToolModel> {
    const tool = await this.httpClient.put<Tool>(`/Tool/${id}`, toolData);
    return new ToolModel(tool);
  }
}

export const toolService = new ToolService(httpClient);

export { HttpError } from './HttpClient';
export { ToolModel } from '../Models/Tool';