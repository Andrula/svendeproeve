import type { Employee } from "./Employee";
import type { EmployeeStorageItem } from "./EmployeeStorageItem";
import type { Machinery } from "./Machinery";
import type { MachineryStorageItem } from "./MachineryStorageItem";
import type { Tool } from "./Tool";
import type { ToolStorageItem } from "./ToolStorageItem";
import type { Vehicle } from "./Vehicle";
import type { VehicleStorageItem } from "./VehicleStorageItem";

export interface Storage {
  relevantId: string;
  storageType: number;
  name: string;
  storageId: string;
  addressId: string;
  employees: EmployeeStorageItem[];
  defaultEmployees: Employee[];
  vehicles: VehicleStorageItem[];
  defaultVehicles: Vehicle[];
  tools: ToolStorageItem[];
  defaultTools: Tool[];
  machines: MachineryStorageItem[];
  defaultMachines: Machinery[];
}