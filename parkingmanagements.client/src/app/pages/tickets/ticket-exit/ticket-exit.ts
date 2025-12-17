import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TicketService } from '../../../core/services/ticket.service';
import { Ticket, TicketStatus } from '../../../core/models/ticket';
import { PaymentMethod } from '../../../core/models/payment';
import { ParkingSpotService } from '../../../core/services/parking-spot.service';
@Component({
  selector: 'app-exit-ticket',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './ticket-exit.html',
  styleUrls: ['./ticket-exit.scss'],
})
export class TicketExit {

  // search
  searchPlate = '';
  searchTicketId = '';

  // data
  foundTicket?: Ticket;
  preview?: {
    durationMinutes: number,
    billingPeriods: number,
    appliedFee: number;
  };

  // payment
  paymentMethod: PaymentMethod = PaymentMethod.Cash;
  isLostTicket = false;

  loading = false;
  errorMsg = '';


  constructor(
    private ticketService: TicketService,
    private parkingSpotService: ParkingSpotService,
    private router: Router
  ) {}

  // search open ticket method
  searchTicket() {
    this.errorMsg = '';
    this.foundTicket = undefined;
    this.preview = undefined;
    this.loading = true;

    // Search by Ticket ID (highest priority)
    if (this.searchTicketId) {
      this.ticketService.searchTickets({ id: this.searchTicketId } as any)
        .subscribe({
          next: tickets => {
            if (!tickets.length) {
              this.errorMsg = 'Ticket not found.';
            } else {
              this.foundTicket = tickets[0];
            }
            this.loading = false;
          },
          error: () => this.handleError()
        });
      return;
    }

    // Search by Plate (only OPEN tickets)
    if (this.searchPlate) {
      this.ticketService.searchTickets({
        status: TicketStatus.Open
      }).subscribe({
        next: tickets => {
          const match = tickets.find(
            t => t.vehicle?.plate === this.searchPlate
          );

          if (!match) {
            this.errorMsg = 'No open ticket found for this plate.';
          } else {
            this.foundTicket = match;
          }

          this.loading = false;
        },
        error: () => this.handleError()
      });
      return;
    }

    this.errorMsg = 'Please enter plate number or ticket ID.';
    this.loading = false;
  }


  private handleError() {
    this.errorMsg = 'Search failed. Please try again.';
    this.loading = false;
  }

  previewExit() {
    if (!this.foundTicket || !this.foundTicket.entryTime) return;

    const entry = new Date(this.foundTicket.entryTime);
    const now = new Date();

    const durationMinutes = Math.ceil(
      (now.getTime()-entry.getTime()) / 60000
    );

    const grace = 15;
    const ratePerHour = 2;
    const billingPeriod = 60;

    let billableMinutes = Math.max(0, durationMinutes-grace);
    const billingPeriods = Math.ceil(billableMinutes/billingPeriod);
    const fee = billingPeriods*ratePerHour;

    this.preview = {
      durationMinutes,
      billingPeriods,
      appliedFee: this.isLostTicket ? 50 : fee
    };
  }

  // close ticket
  closeTicket() {
    if (!this.foundTicket || !this.preview) return;

    this.loading = true;

    const updatedTicket: Partial<Ticket> = {
      status: TicketStatus.Closed,
      exitTime: new Date(),
      computedAmount: this.preview.appliedFee,
      isPaid: true
    };

    this.ticketService.closeTicket(
      this.foundTicket.id, updatedTicket
    ).subscribe({
      next: ()=> {
        // set the spot free
        this.parkingSpotService.updateStatus(
          this.foundTicket!.spotId,
          0 // free
        ).subscribe({
          next: ()=> {
            this.loading = false;
            alert('Ticket closed successfully!');
            this.router.navigate(['/dashboard/ticket']);
          },
          error: ()=> {
            this.errorMsg = 'Ticket closed, but failed to free spot.';
            this.loading = false;
          }
        });
      },
      error: ()=> {
        this.errorMsg = 'Failed to close ticket.';
        this.loading = false;
      }
    })
  }
}
