import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { ParkingLot } from '../../../core/models/parking-lot';
import { ParkingSpot, SpotStatus } from '../../../core/models/parking-spot';
import { VehicleType } from '../../../core/models/vehicle';

import { ParkingLotService } from '../../../core/services/parking-lot.service';
import { ParkingSpotService } from '../../../core/services/parking-spot.service';
import { TicketService } from '../../../core/services/ticket.service';

@Component({
  selector: 'app-ticket-entry',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './ticket-entry.html',
  styleUrls: ['./ticket-entry.scss'],
})
export class TicketEntry implements OnInit {

  selectedLot?: ParkingLot;
  freeSpots: ParkingSpot[] = [];
  selectedSpotId = '';

  vehiclePlate = '';
  vehicleType: VehicleType = VehicleType.Car;
  vehicleColor = '';

  loading = false;

  VehicleType = VehicleType; //use it in the template

  constructor(
    private lotService: ParkingLotService,
    private spotService: ParkingSpotService,
    private ticketService: TicketService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadLotAndSpots();
  }

  isFreeSpot(spot: ParkingSpot): boolean {
    // backend enum: 0 = Free
    return spot.status === SpotStatus.Free;
  }

  loadLotAndSpots() {

  }

  createTicket() {
    if (!this.selectedLot || !this.selectedSpotId || !this.vehiclePlate) {
      alert('Please fill all required fields');
      return;
    }

    this.loading = true;

    this.ticketService.createEntryTicket({
      lotId: this.selectedLot.id,
      spotId: this.selectedSpotId,
      plateNumber: this.vehiclePlate,
      vehicleType: this.vehicleType,
      color: this.vehicleColor
    }).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/dashboard/ticket']);
      },
      error: err => {
        console.error('Failed to create ticket', err);
        this.loading = false;
      }
    });
  }

}
