import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { ParkingSpot, SpotStatus, SpotType } from '../../core/models/parking-spot';
import { ParkingSpotService } from '../../core/services/parking-spot.service';
@Component({
  selector: 'app-spots',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './spots.html',
  styleUrls: ['./spots.scss'],
})
export class SpotsComponent implements OnInit {

  lotId!: string;

  spots: ParkingSpot[] = [];
  filteredSpots: ParkingSpot[] = [];

  statusFilter: SpotStatus | 'ALL' = 'ALL';
  typeFilter: SpotType | 'ALL' = 'ALL';

  loading = true;
  error = '';

  SpotStatus = SpotStatus;
  SpotType = SpotType;

  constructor(
    private route: ActivatedRoute,
    private parkingSpotService: ParkingSpotService
  ) {}

  ngOnInit(): void {
    this.lotId = this.route.snapshot.paramMap.get('lotId')!;
    this.loadSpots();
  }

  loadSpots() {
    this.loading = true;

    this.parkingSpotService.getParkingSpots(this.lotId).subscribe({
      next: spots=> {
        this.spots = spots;
        this.applyFilters();
        this.loading = false;
      },
      error: ()=> {
        this.error = 'Failer to load parking spots.';
        this.loading = false;
      }
    })
  }

  applyFilters() {
    this.filteredSpots = this.spots.filter(spot => {
      const statusOK =
        this.statusFilter === 'ALL' || spot.status === this.statusFilter;

      const typeOk =
        this.typeFilter === 'ALL' || spot.type === this.typeFilter;

      return statusOK && typeOk;
    })
  }

  toggleUnavailable(spot: ParkingSpot) {
    const newStatus =
      spot.status === SpotStatus.Unavailable
        ? SpotStatus.Free
        : SpotStatus.Unavailable;

    this.parkingSpotService.updateStatus(spot.id, newStatus).subscribe(()=> {
      spot.status = newStatus;
      this.applyFilters();
    })
  }

  getStatusLabel(status: SpotStatus) {
    return SpotStatus[status];
  }

  getTypeLabel(type: SpotType) {
    return SpotType[type];
  }

}
