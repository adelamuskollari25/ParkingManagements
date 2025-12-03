import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TicketService {

  private baseUrl = `${environment.apiUrl}/tickets`;

  constructor(private http: HttpClient) { }

  getTicketsById(ticketId: string) {
    return this.http.get(`${this.baseUrl}/${ticketId}/preview-details`)
  }

  createEntryTicket(data: any) {
    return this.http.post(`${this.baseUrl}/enter`, data);
  }

  closeTicket(data: any) {
    return this.http.post(`${this.baseUrl}/close`, data);
  }
}
