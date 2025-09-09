import type { Image } from "./Image";
import type { Occupation } from "./Occupation";
import type { StorageItem } from "./StorageItem";

export interface EmployeeStorageItem extends StorageItem {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  occupation: Occupation;
  image: Image | null;
  storageItemNote: string;
  itemNote: string;
}
