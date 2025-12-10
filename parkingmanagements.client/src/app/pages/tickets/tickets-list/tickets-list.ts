import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Ticket } from '../../../core/models/ticket';
import { TicketService } from '../../../core/services/ticket.service';

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

  constructor (private ticketService: TicketService) {}

  ngOnInit(): void {
    this.loadTicket();
  }

  loadTicket() {
    this.ticketService.searchTickets({ status: 'Open'}).subscribe({
      next: res => {
        this.tickets = res.data;
        this.loading = false;
      },
      error: err => {
        console.log('Failed to load tickets', err);
        this.error = true;
        this.loading = false;
      }
    })
  }

}
