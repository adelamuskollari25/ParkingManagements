import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  private baseUrl = `${environment.apiUrl}/payments`;

  constructor(private http: HttpClient) { }

  payTicket(ticketId: string) {
    return this.http.post(`${this.baseUrl}/tickets/${ticketId}`, {})
  }
}
