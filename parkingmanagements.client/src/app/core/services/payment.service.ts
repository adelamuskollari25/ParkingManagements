import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Payment } from '../models/payment';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  private baseUrl = `${environment.apiUrl}/payments`;

  constructor(private http: HttpClient) { }

  payTicket(ticketId: string) {
    return this.http.post<Payment>(`${this.baseUrl}/tickets/${ticketId}`, {})
  }
}
