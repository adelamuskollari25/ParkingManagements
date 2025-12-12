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



}
