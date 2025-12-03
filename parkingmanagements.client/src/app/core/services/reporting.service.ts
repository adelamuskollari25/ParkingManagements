import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ReportingService {

  private baseUrl = `${environment.apiUrl}/reporting`;

  constructor(private http: HttpClient) { }

  getLotSnapShot() {
    return this.http.get(`${this.baseUrl}/lotsnapshot`);
  }

  getDailyRevenue() {
    return this.http.get(`${this.baseUrl}/dailyrevenue`);
  }
}
