export enum PaymentMethod {
  Cash = 0,
  Card = 1,
  Other = 2
}

export interface Payment {
  id: string;
  ticketId: string;
  amount: number;
  method: PaymentMethod;
  paidAt: Date;
  reference?: string;
}

export interface PaymentReq {
  amount: number;
  method: PaymentMethod;
  reference?: string;
}
