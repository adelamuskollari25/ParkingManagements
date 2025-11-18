export enum VehicleType {
  Car = 'Car',
  Van = 'Van',
  Motorcycle = 'Motorcycle'
}

export interface Vehicle {
  plate: string;
  type: VehicleType;
  color?: string;
}
