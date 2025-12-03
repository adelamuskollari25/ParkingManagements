import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { ParkingLot } from '../models/parking-lot';

@Injectable({
  providedIn: 'root'
})
export class ParkingLotService {

  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getParkingLots() {
    return this.http.get<ParkingLot>(`${this.baseUrl}/ParkingLot`);
  }

  getById(id: string) {
    return this.http.get(`${this.baseUrl}/${id}`);
  }

  getOccupancy(id: string) {
    return this.http.get(`${this.baseUrl}/${id}/occupancy`);
  }
}
