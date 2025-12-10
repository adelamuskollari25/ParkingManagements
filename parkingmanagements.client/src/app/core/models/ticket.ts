import { PaymentReq } from "./payment";
import { Vehicle } from "./vehicle";

export enum TicketStatus {
  Open = 'Open',
  Closed = 'Closed',
  Lost = 'Lost'
}

export interface Ticket {
  id: string;
  lotId: string;
  spotId: string;
  vehicle: Vehicle;
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
}

export interface CloseTicketRequest {
  ticketId: string;
  payment: PaymentReq;
}
