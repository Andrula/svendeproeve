import type { Employee } from "./Employee";
import type { Image } from "./Image";
import type { StorageItem } from "./StorageItem";
import type { Tool } from "./Tool";

export interface VehicleStorageItem extends StorageItem {
  model: string;
  licensePlate: string;
  image: Image | null;
  employees: Employee[];
  tools: Tool[];
  storageItemNote: string;
  itemNote: string;
}
