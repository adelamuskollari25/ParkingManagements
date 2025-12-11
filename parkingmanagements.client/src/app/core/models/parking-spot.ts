export enum SpotType {
  Regular = 'Regular',
  EV = 'EV',
  Disabled = 'Disabled'
}

export enum SpotStatus {
  Free = 'Free',
  Occupied = 'Occupied',
  Unavailable = 'Unavailable'
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
