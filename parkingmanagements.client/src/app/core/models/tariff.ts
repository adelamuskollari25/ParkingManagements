export interface Tariff {
  id: string;
  lotId: string;
  ratePerHour: number;
  billingPeriodMinutes: number;
  gracePeriodMinutes: number;
  lostTicketFee?: number;
  dailyMaximum?: number;
  effectiveFrom: Date;
  createdAt?: Date;
  createdBy?: string;
}
