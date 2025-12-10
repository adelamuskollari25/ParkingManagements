export enum SpotType {
  Regular = 'Regular',
  EV = 'EV',
  Disabled = 'Disabled'
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
  status: number;
  createdAt?: Date;
  updatedAt?: Date;
}

export interface ParkingMetrics {
  totalSpots: number;
  freeSpots: number;
  occupiedSpots: number;
  unavailableSpots: number;
}
