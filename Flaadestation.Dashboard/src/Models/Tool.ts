import type { Image } from "./Image";
import type { Storage } from "./Storage";
import type { StorageItem } from "./StorageItem";

export interface Tool {
  itemId: string;
  name: string;
  note: string;
  companyId: string;
  image: Image | null;
  defaultStorage?: Storage;
  storageItems: StorageItem[];
  vehicle: {
    itemId: string;
    model: string;
    licensePlate: string;
    note: string;
    storageItems: StorageItem[];
  } | null;
}

export class ToolModel {
  tool: Tool;

  constructor(tool: Tool) {
    this.tool = tool;
  }

  get name(): string {
    return this.tool.name;
  }

  get note(): string {
    return this.tool.note;
  }

 get currentAssignment(): StorageItem | null {
    const now = new Date();
    
    if (this.tool.vehicle?.storageItems) {
      return this.tool.vehicle.storageItems.find(item => {
        const start = new Date(item.scheduledStart);
        const end = new Date(item.scheduledEnd);
        return now >= start && now <= end;
      }) || null;
    }
    

    return this.tool.storageItems.find(item => {
      const start = new Date(item.scheduledStart);
      const end = new Date(item.scheduledEnd);
      return now >= start && now <= end;
    }) || null;
  }

  get nextAssignment(): StorageItem | null {
    const now = new Date();
    let assignmentsToCheck: StorageItem[];
    
    if (this.tool.vehicle?.storageItems) {
      assignmentsToCheck = this.tool.vehicle.storageItems;
    } else {
      assignmentsToCheck = this.tool.storageItems;
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

  get data(): Tool {
    return this.tool;
  }
}