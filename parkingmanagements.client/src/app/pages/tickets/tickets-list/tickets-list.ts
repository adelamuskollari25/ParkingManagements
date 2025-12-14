import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Ticket, TicketStatus } from '../../../core/models/ticket';
import { TicketService } from '../../../core/services/ticket.service';
import { VehicleType } from '../../../core/models/vehicle';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-tickets-list',
  imports: [RouterModule, CommonModule, FormsModule],
  templateUrl: './tickets-list.html',
  styleUrl: './tickets-list.scss',
})
export class TicketsList implements OnInit {

  tickets: Ticket[] = [];
  loading = true;
  error = false;

  VehicleType = VehicleType;
  TicketStatus = TicketStatus;

  statusFilter: TicketStatus | 'ALL' = 'ALL';
  plateFilter = '';

  // pagination
  currentPage = 1;
  pageSize = 5;

  constructor (private ticketService: TicketService) {}

  ngOnInit(): void {
    this.loadTicket();
  }

  get pagedTickets(): Ticket[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.tickets.slice(start, start+this.pageSize);
  }

  get totalPages(): number {
    return Math.ceil(this.tickets.length / this.pageSize);
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  loadTicket() {
    this.loading = true;

    this.ticketService.searchTickets().subscribe({
      next: tickets=> {
        this.tickets = tickets;
        this.loading = false;
      },
      error: err=> {
        console.error('Failed to load tickets: ', err);
        this.error = true;
        this.loading = false;
      }
    })
  }

  loadFilteredTickets() {
    this.currentPage = 1;
    const params: any = {};

    if (this.statusFilter !== 'ALL') {
      params.status = this.statusFilter;
    }

    if (this.plateFilter) {
      params['vehicle.plate_like'] = this.plateFilter;
    }

    this.ticketService.searchTickets(params).subscribe({
      next: tickets => {
        this.tickets = tickets;
        this.loading = false;
      },
      error: err => {
        console.error('Failed to load tickets:', err);
        this.error = true;
        this.loading = false;
      }
    });
  }


  getStatusLabel(status: TicketStatus): string {
    return TicketStatus[status];
  }

  getVehicleTypeLabel(type?: VehicleType): string {
    return type !== undefined ? VehicleType[type] : 'Unknown';
  }

}
