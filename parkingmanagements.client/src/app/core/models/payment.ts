export enum PaymentMethod {
  Cash = 'Cash',
  Card = 'Card',
  Other = 'Other'
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
