import { PaymentReq } from "./payment";
import { VehicleType } from "./vehicle";

export enum TicketStatus {
  Open = 0,
  Closed = 1,
  Lost = 2
}

export interface Ticket {
  id: string;
  lotId: string;
  spotId: string;
  spotCode: string;      // NEW
  vehicle:
    {
      plate: string;
      type?: VehicleType;  // NEW
      color?: string;      // NEW
    }
  entryTime?: Date;
  exitTime?: Date;
  status: TicketStatus;
  computedAmount?: number;
  isPaid: boolean;
  createdAt?: Date;
  updatedAt?: Date;
}

export interface TicketPreview {
  ticketId: string;
  entryTime: Date;
  currentTime: Date;
  durationMinutes: number;
  gracePeriodMinutes: number;
  billableMinutes: number;
  billingPeriods: number;
  ratePerHour: number;
  computedFee: number;
  dailyMaximum?: number;
  appliedFee: number;
}

export interface CreateTicketRequest {
  lotId: string;
  spotId: string;
  plateNumber: string;
  vehicleType?: VehicleType; // NEW
  color?: string; // NEW
}

export interface CloseTicketRequest {
  ticketId: string;
  paymentMethod: number;
  isLostTicket: boolean;
}
