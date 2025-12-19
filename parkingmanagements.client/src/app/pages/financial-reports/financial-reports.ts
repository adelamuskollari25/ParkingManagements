import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Ticket } from '../../core/models/ticket';
import { TicketService } from '../../core/services/ticket.service';

@Component({
  selector: 'app-financial-reports',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './financial-reports.html',
  styleUrls: ['./financial-reports.scss'],
})

export class FinancialReports implements OnInit {

  paidTickets: Ticket[] = [];

  totalRevenue = 0;
  revenueToday = 0;
  revenueLast7Days = 0;
  revenueThisMonth = 0;

  loading = true;

  constructor(private ticketService: TicketService) {}

  ngOnInit(): void {
    this.loadRevenue();
  }

  loadRevenue() {
    this.ticketService.searchTickets().subscribe(tickets => {
      const paid = tickets.filter(
        t => t.status === 1 && t.isPaid && t.computedAmount
      );

      this.paidTickets = paid;

      this.calculateRevenue(paid);
      this.loading = false;
    });
  }

  calculateRevenue(tickets: Ticket[]) {
    const now = new Date();

    this.totalRevenue = tickets.reduce(
      (sum, t) => sum + (t.computedAmount || 0),
      0
    );

    this.revenueToday = tickets
      .filter(t => this.isSameDay(t.exitTime!, now))
      .reduce((s, t) => s + (t.computedAmount || 0), 0);

    this.revenueLast7Days = tickets
      .filter(t => this.isLast7Days(t.exitTime!, now))
      .reduce((s, t) => s + (t.computedAmount || 0), 0);

    this.revenueThisMonth = tickets
      .filter(t => this.isSameMonth(t.exitTime!, now))
      .reduce((s, t) => s + (t.computedAmount || 0), 0);
  }

  isSameDay(a: Date, b: Date) {
    const d1 = new Date(a);
    return d1.toDateString() === b.toDateString();
  }

  isLast7Days(date: Date, now: Date) {
    const d = new Date(date);
    const diff = now.getTime() - d.getTime();
    return diff <= 7 * 24 * 60 * 60 * 1000;
  }

  isSameMonth(date: Date, now: Date) {
    const d = new Date(date);
    return d.getMonth() === now.getMonth()
      && d.getFullYear() === now.getFullYear();
  }
}
