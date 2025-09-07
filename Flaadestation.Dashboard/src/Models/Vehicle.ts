import type { Employee } from "./Employee";
import type { Image } from "./Image";
import type { StorageItem } from "./StorageItem";
import type { Tool } from "./Tool";

export interface Vehicle {
  itemId: string;
  model: string;
  licensePlate: string;
  note: string;
  defaultStorage: Storage;
  companyId: string;
  image: Image | null;
  storageItems: StorageItem[];
  employees: Employee[];
  tools: Tool[];
}

export class VehicleModel {
  vehicle: Vehicle;

  constructor(vehicle: Vehicle) {
    this.vehicle = vehicle;
  }

  get model(): string {
    return this.vehicle.model;
  }

  get licensePlate(): string {
    return this.vehicle.licensePlate;
  }

  get employees(): Employee[] {
    return this.vehicle.employees;
  }

  get tools(): Tool[] {
    return this.vehicle.tools;
  }

  get currentAssignment(): StorageItem | null {
    const now = new Date();
    return (
      this.vehicle.storageItems.find((item) => {
        const start = new Date(item.scheduledStart);
        const end = new Date(item.scheduledEnd);
        return now >= start && now <= end;
      }) || null
    );
  }

  get nextAssignment(): StorageItem | null {
    const now = new Date();
    const futureAssignments = this.vehicle.storageItems.filter(item => {
      const start = new Date(item.scheduledStart);
      return start > now;
    });
    
    if (futureAssignments.length === 0) return null;
    
    return futureAssignments.sort((a, b) => 
      new Date(a.scheduledStart).getTime() - new Date(b.scheduledStart).getTime()
    )[0];
  }

  get isAvailable(): boolean {
    return !this.currentAssignment;
  }

  get data(): Vehicle {
    return this.vehicle;
  }
}
