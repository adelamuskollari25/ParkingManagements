import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CloseTicketRequest, CreateTicketRequest, Ticket, TicketPreview, TicketStatus } from '../models/ticket';

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
  createEntryTicket(data: CreateTicketRequest & { spotCode: string }) {
  return this.http.post<Ticket>(this.baseUrl, {
    lotId: data.lotId,
    spotId: data.spotId,
    spotCode: data.spotCode,
    vehicle: {
      plate: data.plateNumber,
      type: data.vehicleType,
      color: data.color
    },
    status: TicketStatus.Open,
    entryTime: new Date().toISOString(),
    exitTime: null,
    computedAmount: null,
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
