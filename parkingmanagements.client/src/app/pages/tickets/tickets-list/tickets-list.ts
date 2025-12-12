import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Ticket, TicketStatus } from '../../../core/models/ticket';
import { TicketService } from '../../../core/services/ticket.service';
import { VehicleType } from '../../../core/models/vehicle';

@Component({
  selector: 'app-tickets-list',
  imports: [RouterModule, CommonModule],
  templateUrl: './tickets-list.html',
  styleUrl: './tickets-list.scss',
})
export class TicketsList implements OnInit {

  tickets: Ticket[] = [];
  loading = true;
  error = false;

  VehicleType = VehicleType;
  TicketStatus = TicketStatus;

  constructor (private ticketService: TicketService) {}

  ngOnInit(): void {
    this.loadTicket();
  }

  loadTicket() {

  }

    getStatusLabel(status: TicketStatus): string {
      switch (status) {
        case TicketStatus.Open: return 'OPEN';
        case TicketStatus.Closed: return 'CLOSED';
        case TicketStatus.Lost: return 'LOST';
        default: return 'UNKNOWN';
      }
    }

    getVehicleTypeLabel(type?: VehicleType): string {
      switch (type) {
        case VehicleType.Car: return 'Car';
        case VehicleType.Van: return 'Van';
        case VehicleType.Motorcycle: return 'Motorcycle';
        default: return '';
      }
    }

}
