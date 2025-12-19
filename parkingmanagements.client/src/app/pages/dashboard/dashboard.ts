import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';

import { ParkingLot } from '../../core/models/parking-lot';
import { ParkingSpot, SpotStatus } from '../../core/models/parking-spot';
import { Ticket } from '../../core/models/ticket';

import { ParkingSpotService } from '../../core/services/parking-spot.service';
import { ParkingLotService } from '../../core/services/parking-lot.service';
import { TicketService } from '../../core/services/ticket.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatIconModule, RouterModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.scss'],
})
export class Dashboard implements OnInit {

  // Data
  spotsInfo: ParkingSpot[] = [];
  lotsInfo: ParkingLot[] = [];
  tickets: Ticket[] = [];
  selectedLot?: ParkingLot;

  // KPIs
  totalSpots = 0;
  freeSpots = 0;
  occupiedSpots = 0;
  unavailableSpots = 0;
  openTickets = 0;
  revenueToday = 0;

  // get date
  today: Date = new Date();
  greeting: string = '';
  icon: string = '';

  loading = true;


  constructor(
    private parkingSpotService: ParkingSpotService,
    private parkingLotService: ParkingLotService,
    private ticketService: TicketService
  ) {}

  ngOnInit() {
    this.setGreeting();
    this.loadDashboard();
  }

  loadDashboard() {
    this.parkingLotService.getParkingLots().subscribe(lots => {
      if (!lots.length) return;

      this.lotsInfo = lots;
      this.selectedLot = lots[0];

      this.loadSpots();
      this.loadTickets();
    })
  }

  loadSpots() {
    if (!this.selectedLot) return;

    this.parkingSpotService.getParkingSpots(this.selectedLot.id).subscribe(spots => {
      this.spotsInfo = spots;

      this.totalSpots = spots.length;
      this.freeSpots = spots.filter(s=> s.status === SpotStatus.Free).length;
      this.occupiedSpots = spots.filter(s=> s.status === SpotStatus.Occupied).length;
      this.unavailableSpots = spots.filter(s=> s.status === SpotStatus.Unavailable).length;
    })
  }

  loadTickets() {
    if (!this.selectedLot) return;

    this.ticketService.searchTickets({ lotId: this.selectedLot.id }).subscribe(tickets => {
      this.tickets = tickets;
      this.openTickets = tickets.filter(
        t=> t.status === 0
      ).length;

      // revenue
      const today = new Date();
      today.setHours(0,0,0,0);

      this.revenueToday = tickets.filter(
        t=> t.status === 1 &&
        t.isPaid === true &&
        t.exitTime &&
        new Date(t.exitTime).setHours(0,0,0,0) === today.getTime()
      ).reduce((sum, t)=> sum += (t.computedAmount ?? 0), 0);
      this.loading = false;
    });
  }

  // set the icon and greeting depending on time
  setGreeting() {
    const hour = this.today.getHours();

    if (hour < 5) {
      this.icon = 'star';
      this.greeting = 'Heyyy';
    } else if (hour < 12) {
      this.icon = 'wb_sunny';
      this.greeting = 'Good Morning!';
    } else if (hour < 17) {
      this.icon = 'light_mode';
      this.greeting = 'Good Afternoon!';
    } else if (hour < 21) {
      this.icon = 'nightlight';
      this.greeting = 'Good Evening!';
    } else {
      this.icon = 'star';
      this.greeting = 'Heyyy';
    }
  }

}
