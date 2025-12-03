import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Tariff } from '../models/tariff';

@Injectable({
  providedIn: 'root'
})
export class TariffService {

  private baseUrl = `${environment.apiUrl}/lots`;

  constructor(private http: HttpClient) { }

  getCurrentTariff(lotId: string) {
    return this.http.get<Tariff>(`${this.baseUrl}/${lotId}/Tariff/current`);
  }

  getTariffHistory(lotId: string) {
    return this.http.get<Tariff[]>(`${this.baseUrl}/${lotId}/Tariff/history`);
  }

  createTariff(lotId: string, data: Partial<Tariff>) {
    return this.http.post<Tariff>(`${this.baseUrl}/${lotId}/Tariff`, data);
  }
}
