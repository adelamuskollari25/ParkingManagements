import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  private baseUrl = `${environment.apiUrl}/admin/users`;

  constructor(private http: HttpClient) {}

  getAllUsers() {
    return this.http.get(`${this.baseUrl}/all`);
  }

  deactivateUser(userId: string) {
    return this.http.post(`${this.baseUrl}/deactivate/${userId}`, {})
  }

  reactivateUser(userId: string) {
    return this.http.post(`${this.baseUrl}/reactivate/${userId}`, {})
  }
}
