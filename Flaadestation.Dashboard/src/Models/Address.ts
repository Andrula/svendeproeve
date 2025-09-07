import type { DawaMiniResponse } from "../Services/DawaService";

export interface Address {
  addressId: string;
  street: string;
  streetNumber: string;
  floor: number | null;
  door: string | null;
  postalCode: number;
  postalName: string;
  x: number;
  y: number;
}

export class AddressModel implements Address {
  addressId: string;
  street: string;
  streetNumber: string;
  floor: number | null;
  door: string | null;
  postalCode: number;
  postalName: string;
  x: number;
  y: number;

  constructor(dawaResponse: DawaMiniResponse) {

    this.addressId = dawaResponse.id;
    this.street = dawaResponse.vejnavn;
    this.streetNumber = dawaResponse.husnr;
    this.floor = dawaResponse.etage ? Number(dawaResponse.etage) || null : null;
    this.door = dawaResponse.dør ?? null;
    this.postalCode = Number(dawaResponse.postnr);
    this.postalName = dawaResponse.postnrnavn;
    this.x = dawaResponse.x;
    this.y = dawaResponse.y;
  }
}
