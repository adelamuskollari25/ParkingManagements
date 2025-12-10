export enum SpotType {
  Regular = 0,
  EV = 1,
  Disabled = 2
}

export enum SpotStatus {
  Free = 0,
  Occupied = 1,
  Unavailable = 2
}

export interface ParkingSpot {
  id: string,
  lotId: string,
  spotCode: string;
  type: SpotType;
  status: SpotStatus;
  createdAt?: Date;
  updatedAt?: Date;
}

export interface ParkingMetrics {
  totalSpots: number;
  freeSpots: number;
  occupiedSpots: number;
  unavailableSpots: number;
}
