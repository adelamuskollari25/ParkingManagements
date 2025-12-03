import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ParkingSpotService {

  private baseUrl = `${environment.apiUrl}/lots`

  constructor(private http: HttpClient) { }

  getParkingSpots(lotId: string) {
    return this.http.get(`${this.baseUrl}/${lotId}/ParkingSpot`);
  }

  getById(lotId: string, spotId: string) {
    return this.http.get(`${this.baseUrl}/${lotId}/ParkingSpot/${spotId}`);
  }

  update(lotId: string, spotId: string, data: any) {
    return this.http.put(`${this.baseUrl}/${lotId}/ParkingSpot/${spotId}`, data);
  }

  updateStatus(lotId: string, spotId: string, status: string) {
    return this.http.patch(`${this.baseUrl}/${lotId}/ParkingSpot/${spotId}/status`, { status });
  }
}
