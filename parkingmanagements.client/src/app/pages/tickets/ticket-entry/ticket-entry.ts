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

  // Data
  lots: ParkingLot[] = [];
  freeSpots: ParkingSpot[] = [];

  selectedLotId = '';
  selectedSpotId = '';

  plate = '';
  vehicleType: VehicleType = VehicleType.Car;
  color = '';

  loading = false;
  error = '';
  success = false;

  VehicleType = VehicleType;

  constructor(
    private lotService: ParkingLotService,
    private spotService: ParkingSpotService,
    private ticketService: TicketService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadLots();
  }

  // load parking lot
  loadLots() {
    this.lotService.getParkingLots().subscribe({
      next: lots=> {
        this.lots = lots;

        if (lots.length) {
          this.selectedLotId = lots[0].id;
          this.loadFreeSpots();
        }
      },
      error: ()=> {
        this.error = 'Failed to load parking spots';
      }
    })
  }

  loadFreeSpots() {
    this.freeSpots = [];
    this.selectedSpotId = '';

    this.spotService.getParkingSpots(this.selectedLotId).subscribe({
      next: spots=> {
        this.freeSpots = spots.filter(
          s => s.status === SpotStatus.Free
        );

        console.log('FREE spots:', this.freeSpots);
      },
      error: ()=> {
        this.error = 'Failed to load parking spots.';
      }
    })
  }

  get selectedSpot(): ParkingSpot | undefined {
    return this.freeSpots.find(s => s.id === this.selectedSpotId);
  }


  // submit ticket
  createTicket() {
    this.error = '';
    this.loading = true;

    const spot = this.selectedSpot;

    if (!this.plate || !spot) {
      this.error = 'Please fill all the required fields.';
      this.loading = false;
      return;
    }

    // create ticket
    this.ticketService.createEntryTicket({
      lotId: this.selectedLotId,
      spotId: this.selectedSpotId,
      spotCode: spot.spotCode,
      plateNumber: this.plate,
      vehicleType: this.vehicleType,
      color: this.color || undefined
    }).subscribe({
      next: () => {

        // mark spot as occupied
        this.spotService.updateStatus(
          this.selectedSpotId,
          SpotStatus.Occupied
        ).subscribe({
          next: ()=> {
            this.loading = false;
            this.success = true;

            // redirect after short delay
            setTimeout(()=> {
              this.router.navigate(['/dashboard/ticket']);
            }, 800);
          },
          error: ()=> {
            this.error = 'Ticket created but failed to update spot.';
            this.loading = false;
          }
        });
      },
      error: ()=> {
        this.error = 'Failed to create ticket.';
        this.loading = false;
      }
    })
  }

}
