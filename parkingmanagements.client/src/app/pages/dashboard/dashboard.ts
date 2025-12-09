import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { DashboardMetrics } from '../../core/models/dashboardMetrics';
import { ReportingService } from '../../core/services/reporting.service';
import { ParkingSpot, SpotStatus } from '../../core/models/parking-spot';
import { ParkingSpotService } from '../../core/services/parking-spot.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatIconModule, RouterModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.scss'],
})
export class Dashboard implements OnInit {

  // get dashboard metrics
  metrics: DashboardMetrics[] = [];
  loadingMetrics = true;

  //get Spots info
  spotsInfo: ParkingSpot[] = [];

  //get date
  today: Date = new Date();
  greeting: string = '';
  icon: string = '';


  constructor(
    private reportingService: ReportingService,
    private parkingSpotService: ParkingSpotService) {}

  ngOnInit() {
    this.setGreeting();

    this.reportingService.getLotSnapShot().subscribe({
      next: res => {
        this.metrics = res.data;
        this.loadingMetrics = false;
        if(this.metrics.length > 0){
        this.loadParkingSpots(this.metrics[0].lotId); // load first lot's spots
      }
        console.log('Metrics loaded: ', this.metrics);
      },
      error: err => {
        this.loadingMetrics = false;
        console.error('Snapshot error: ', err)
      }
    })
  }

  //load parking spots
  loadParkingSpots(lotId: string) {
    this.parkingSpotService.getParkingSpots(lotId).subscribe({
      next: res => {
        this.spotsInfo = res.data;
        console.log('Paking spots loaded: ', this.spotsInfo);
      },
      error: err => {
        console.error('Error loading parking spots: ', err);
      }
    })
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

  mapApiStatusToEnum(status: number): SpotStatus {
  switch(status){
    case 0: return SpotStatus.Free;
    case 1: return SpotStatus.Occupied;
    case 2: return SpotStatus.Unavailable;
    default: return SpotStatus.Free; // fallback
  }
}

  getSpotColor(spot: ParkingSpot): string {
    const enumStatus = this.mapApiStatusToEnum(spot.status);
    return this.setParkingSpotColor(enumStatus);
  }

  // set parking spot color based on parking spot status
  setParkingSpotColor(status: string): string {
    switch (status) {
      case 'Free':
        return 'green';
      case 'Occupied':
        return 'red';
      case 'Unavailable':
        return 'gray';
      default:
          return 'blue';
    }
  }
}
