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

  getTicketPreview(ticketId: string) {
    return this.http.get<TicketPreview>(`${this.baseUrl}/${ticketId}/preview-exit`);
  }

  // CREATE ENTRY TICKET
  createEntryTicket(data: CreateTicketRequest) {
    return this.http.post<Ticket>(this.baseUrl, {
      ...data,
      status: 'Open',
      entryTime: new Date().toISOString(),
      isPaid: false
    });
  }

  // CLOSE TICKET
  closeTicket(ticketId: string, update: Partial<Ticket>) {
    return this.http.patch<Ticket>(
      `${this.baseUrl}/${ticketId}`,
      update
    );
  }

  // SEARCH OPEN TICKETS
  searchTickets(params?: {
    status?: number;
    plate?: string;
    lotId?: string;
  }) {
    return this.http.get<Ticket[]>(
      this.baseUrl,
      { params: params as any }
    );
  }


}
