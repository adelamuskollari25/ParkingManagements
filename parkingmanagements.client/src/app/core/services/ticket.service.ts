import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CloseTicketRequest, CreateTicketRequest, Ticket, TicketPreview } from '../models/ticket';

@Injectable({
  providedIn: 'root'
})
export class TicketService {

  private baseUrl = `${environment.apiUrl}/tickets`;

  constructor(private http: HttpClient) { }

  getTicketsById(ticketId: string) {
    return this.http.get<TicketPreview>(`${this.baseUrl}/${ticketId}/preview-details`)
  }

  createEntryTicket(data: CreateTicketRequest) {
    return this.http.post<Ticket>(`${this.baseUrl}/enter`, data);
  }

  closeTicket(data: CloseTicketRequest) {
    return this.http.post<Ticket>(`${this.baseUrl}/close`, data);
  }
}
