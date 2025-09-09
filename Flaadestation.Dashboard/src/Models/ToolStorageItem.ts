import type { Image } from "./Image";
import type { StorageItem } from "./StorageItem";

export interface ToolStorageItem extends StorageItem {
    name: string;
    image: Image | null;
    storageItemNote: string;
    itemNote: string;
}