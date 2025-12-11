import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TicketService } from '../../../core/services/ticket.service';
import { Ticket, TicketPreview } from '../../../core/models/ticket';

@Component({
  selector: 'app-exit-ticket',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './ticket-exit.html',
  styleUrls: ['./ticket-exit.scss'],
})
export class TicketExit {

  // SEARCH INPUT
  searchPlate = '';
  searchTicketId = '';

  // RESULTS
  foundTicket?: Ticket;
  preview?: TicketPreview;

  loading = false;
  errorMsg = '';

  paymentMethod = '0'; // 0 = Cash, 1 = Card, 2 = Other
  isLostTicket = false;

  constructor(
    private ticketService: TicketService,
    private router: Router
  ) {}

  // STEP 1 — SEARCH OPEN TICKET
  searchTicket() {
    this.errorMsg = '';
    this.foundTicket = undefined;
    this.preview = undefined;

    if (!this.searchPlate && !this.searchTicketId) {
      this.errorMsg = 'Enter plate number or ticket ID.';
      return;
    }

    this.loading = true;

    this.ticketService.searchTickets({
      status: 'Open',
      plate: this.searchPlate || undefined
    }).subscribe({
      next: res => {
        const tickets = res.data;

        if (this.searchTicketId) {
          this.foundTicket = tickets.find(t => t.id === this.searchTicketId);
        } else {
          this.foundTicket = tickets[0]; // first match
        }

        if (!this.foundTicket) {
          this.errorMsg = 'No open ticket found.';
        }

        this.loading = false;
      },
      error: err => {
        this.errorMsg = 'Error searching tickets.';
        this.loading = false;
      }
    });
  }

  // STEP 2 — PREVIEW EXIT
  previewExit() {
    if (!this.foundTicket) return;

    this.loading = true;
    this.ticketService.getTicketPreview(this.foundTicket.id).subscribe({
      next: res => {
        this.preview = res;
        this.loading = false;
      },
      error: err => {
        this.errorMsg = 'Failed to load preview.';
        this.loading = false;
      }
    });
  }

  // STEP 3 — CLOSE TICKET
  closeTicket() {
    if (!this.foundTicket || !this.preview) return;

    this.loading = true;
    this.ticketService.closeTicket({
      ticketId: this.foundTicket.id,
      paymentMethod: Number(this.paymentMethod),
      isLostTicket: this.isLostTicket
    }).subscribe({
      next: () => {
        this.loading = false;
        alert('Ticket closed successfully!');
        this.router.navigate(['/dashboard/ticket']);
      },
      error: err => {
        this.errorMsg = 'Failed to close ticket.';
        this.loading = false;
      }
    });
  }

}
