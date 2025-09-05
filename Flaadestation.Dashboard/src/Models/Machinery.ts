import type { Image } from "./Image";
import type { Storage } from "./Storage";
import type { StorageItem } from "./StorageItem";

export interface Machinery {
  itemId: string;
  name: string;
  note: string;
  companyId: string;
  image: Image | null;
  defaultStorage?: Storage;
  storageItems: StorageItem[];
}

export class MachineryModel {
  machinery: Machinery;

  constructor(machinery: Machinery) {
    this.machinery = machinery;
  }

  get name(): string {
    return this.machinery.name;
  }

  get note(): string {
    return this.machinery.note;
  }

  get currentAssignment(): StorageItem | null {
    const now = new Date();
    return this.machinery.storageItems.find(item => {
      const start = new Date(item.scheduledStart);
      const end = new Date(item.scheduledEnd);
      return now >= start && now <= end;
    }) || null;
  }

  get nextAssignment(): StorageItem | null {
    const now = new Date();
    const futureAssignments = this.machinery.storageItems.filter(item => {
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

  get data(): Machinery {
    return this.machinery;
  }
}