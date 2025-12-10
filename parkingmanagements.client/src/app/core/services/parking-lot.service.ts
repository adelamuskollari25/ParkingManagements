import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { ParkingLot } from '../models/parking-lot';
import { ParkingMetrics } from '../models/parking-spot';

@Injectable({
  providedIn: 'root'
})
export class ParkingLotService {

  private baseUrl = `${environment.apiUrl}/ParkingLot`;

  constructor(private http: HttpClient) { }

  getParkingLots() {
    return this.http.get<{ data: ParkingLot[] }>(this.baseUrl);
  }

  getById(id: string) {
    return this.http.get<ParkingLot>(`${this.baseUrl}/${id}`);
  }

  create(data: Partial<ParkingLot>) {
    return this.http.post<ParkingLot>(this.baseUrl, data);
  }

  getOccupancy(id: string) {
    return this.http.get<ParkingMetrics>(`${this.baseUrl}/${id}/occupancy`);
  }
}
