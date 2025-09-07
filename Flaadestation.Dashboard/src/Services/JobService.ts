import { HttpClient } from './HttpClient';
import { type Job, JobModel } from '../Models/Job';

// interface CreateJobRequest extends JobFormData {
//   companyId: string;
// }

const httpClient = new HttpClient({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  timeout: 30000,
  withCredentials: true
});

export class JobService {
  httpClient: HttpClient;

  constructor(httpClient: HttpClient) {
    this.httpClient = httpClient;
  }

  async getJobsByCompany(): Promise<JobModel[]> {
    const jobs = await this.httpClient.get<Job[]>(`/Job/company`);
    return jobs.map(emp => new JobModel(emp));
  }

  async deleteJob(id: string): Promise<void> {
    await this.httpClient.delete(`/Job/${id}`);
  }

 async getJobById(id: string): Promise<JobModel> {
    const job = await this.httpClient.get<Job>(`/Job/${id}`);
    return new JobModel(job);
  }

//   async createJob(jobData: JobFormData, companyId: string): Promise<JobModel> {
//     const requestData: CreateJobRequest = {
//       ...jobData,
//       companyId
//     };
    
//     const job = await this.httpClient.post<Job>('/Job', requestData);
//     return new JobModel(job);
//   }

//   async updateJob(id: string, jobData: JobFormData): Promise<JobModel> {
//     const job = await this.httpClient.put<Job>(`/Job/${id}`, jobData);
//     return new JobModel(job);
//   }
}


export const jobService = new JobService(httpClient);

export { HttpError } from './HttpClient';
export { JobModel } from '../Models/Job';
