import type { Image } from "./Image";
import type { Occupation } from "./Occupation";
import type { StorageItem } from "./StorageItem";

export interface Employee {
  itemId: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  note: string;
  occupation: Occupation;
  defaultStorage: Storage;
  companyId: string;
  image : Image | null;
  storageItems: StorageItem[];
  vehicle: {
    itemId: string;
    model: string;
    licensePlate: string;
    note: string;
    storageItems: StorageItem[];
  } | null;
}

export class EmployeeModel {
  employee: Employee;

  constructor(employee: Employee) {
    this.employee = employee;
  }

  get fullName(): string {
    return `${this.employee.firstName} ${this.employee.lastName}`;
  }

  get currentAssignment(): StorageItem | null {
    const now = new Date();
    

    if (this.employee.vehicle?.storageItems) {
      return this.employee.vehicle.storageItems.find(item => {
        const start = new Date(item.scheduledStart);
        const end = new Date(item.scheduledEnd);
        return now >= start && now <= end;
      }) || null;
    }
    
    return this.employee.storageItems.find(item => {
      const start = new Date(item.scheduledStart);
      const end = new Date(item.scheduledEnd);
      return now >= start && now <= end;
    }) || null;
  }

  get nextAssignment(): StorageItem | null {
    const now = new Date();
    let assignmentsToCheck: StorageItem[];
    

    if (this.employee.vehicle?.storageItems) {
      assignmentsToCheck = this.employee.vehicle.storageItems;
    } else {
      assignmentsToCheck = this.employee.storageItems;
    }
    
    const futureAssignments = assignmentsToCheck.filter(item => {
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

  formatPhoneNumber(): string {
    const phone = this.employee.phone.replace(/\D/g, '');
    if (phone.length === 8) {
      return `${phone.slice(0, 2)} ${phone.slice(2, 4)} ${phone.slice(4, 6)} ${phone.slice(6)}`;
    }
    return this.employee.phone;
  }

  get data(): Employee {
    return this.employee;
  }
}