import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { ParkingSpot } from '../models/parking-spot';

@Injectable({
  providedIn: 'root'
})
export class ParkingSpotService {

  private baseUrl = `${environment.apiUrl}/parkingSpots`

  constructor(private http: HttpClient) { }

  getParkingSpots(lotId: string) {
    return this.http.get<ParkingSpot[]>(
      `${this.baseUrl}?lotId=${lotId}`
    );
  }

  create(lotId: string, data: Partial<ParkingSpot>) {
    return this.http.post<ParkingSpot>(`${this.baseUrl}/${lotId}/ParkingSpot`, data);
  }

  getById(lotId: string, spotId: string) {
    return this.http.get<ParkingSpot>(`${this.baseUrl}/${lotId}/ParkingSpot/${spotId}`);
  }

  update(lotId: string, spotId: string, data: Partial<ParkingSpot>) {
    return this.http.put<ParkingSpot>(`${this.baseUrl}/${lotId}/ParkingSpot/${spotId}`, data);
  }

  updateStatus(spotId: string, status: number) {
    return this.http.patch<ParkingSpot>(
      `${this.baseUrl}/${spotId}`,
      { status }
    );
  }
}
