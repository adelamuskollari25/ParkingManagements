import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { DashboardMetrics, RevenueSummary } from '../models/dashboardMetrics';

@Injectable({
  providedIn: 'root'
})
export class ReportingService {

  private baseUrl = `${environment.apiUrl}/reporting`;

  constructor(private http: HttpClient) { }

  getLotSnapShot() {
    return this.http.get<DashboardMetrics[]>(`${this.baseUrl}/lotsnapshot`);
  }

  getDailyRevenue() {
    return this.http.get<RevenueSummary[]>(`${this.baseUrl}/dailyrevenue`);
  }
}
