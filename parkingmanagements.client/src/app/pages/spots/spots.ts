import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ParkingSpotService } from '../../core/services/parking-spot.service';
import { ParkingSpot } from '../../core/models/parking-spot';
import { ParkingLotService } from '../../core/services/parking-lot.service';
import { ParkingLot } from '../../core/models/parking-lot';
import { ParkingMetrics } from '../../core/models/parking-spot';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-spots',
  imports: [CommonModule],
  templateUrl: './spots.html',
  styleUrl: './spots.scss',
})
export class SpotsComponent implements OnInit {

  lotId!: string;

  lot?: ParkingLot;
  metrics?: ParkingMetrics;
  spots: ParkingSpot[] = [];

  loading = true;
  error = false;

  constructor(
    private route: ActivatedRoute,
    private parkingLotService: ParkingLotService,
    private parkingSpotService: ParkingSpotService
  ) {}

  ngOnInit(): void {
    this.lotId = this.route.snapshot.paramMap.get('lotId')!;
    this.loadData();
    console.log('LotId from URL =', this.lotId);
  }

  loadData() {
    this.loading = true;
    this.error = false;

    // load lot details
    this.parkingLotService.getById(this.lotId).subscribe({
      next: lot => this.lot = lot,
      error: () => this.error = true
    });

    // load occupancy metrics
    this.parkingLotService.getOccupancy(this.lotId).subscribe({
      next: metrics => this.metrics = metrics,
      error: () => this.error = true
    });

    // load parking spots
    this.parkingSpotService.getParkingSpots(this.lotId).subscribe({
      next: res => {
        this.spots = res.data;
        this.loading = false;
      },
      error: err => {
        console.error('Failed to load spots:', err);
        this.error = true;
        this.loading = false;
      }
    });
  }

}
