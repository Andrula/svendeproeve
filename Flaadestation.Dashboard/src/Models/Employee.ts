export interface Occupation {
  occupationId: string;
  name: string;
}

export interface Storage {
  relevantId: string;
  storageType: number;
  name: string;
  storageId: string;
  addressId: string;
}

export interface StorageItem {
  storageItemId: string;
  scheduledStart: string;
  scheduledEnd: string;
  note: string;
  itemId: string;
  storage: Storage;
}

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
  imageId: string | null;
  imageValue: string | null;
  storageItems: StorageItem[];
  vehicle: any | null;
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
    return this.employee.storageItems.find(item => {
      const start = new Date(item.scheduledStart);
      const end = new Date(item.scheduledEnd);
      return now >= start && now <= end;
    }) || null;
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