export interface StorageItem {
  storageItemId: string;
  scheduledStart: string;
  scheduledEnd: string;
  note: string;
  itemId: string;
  storage: Storage;
}

export class StorageItemModel {
  storageItem: StorageItem

  constructor(storageItem: StorageItem) {
    this.storageItem = storageItem;
  }

  get data(): StorageItem {
    return this.storageItem
  }
}