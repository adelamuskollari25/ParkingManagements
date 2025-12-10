export enum VehicleType {
  Car = 0,
  Van = 1,
  Motorcycle = 2
}

export interface Vehicle {
  plate: string;
  type: VehicleType;
  color?: string;
}
