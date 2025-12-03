import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  private baseUrl = `${environment.apiUrl}/admin/users`;

  constructor(private http: HttpClient) {}

  getAllUsers() {
    return this.http.get<User[]>(`${this.baseUrl}/all`);
  }

  createUser(data: Partial<User>) {
    return this.http.post<User>(`${this.baseUrl}/create`, data);
  }

  deactivateUser(userId: string) {
    return this.http.post(`${this.baseUrl}/deactivate/${userId}`, {})
  }

  reactivateUser(userId: string) {
    return this.http.post(`${this.baseUrl}/reactivate/${userId}`, {})
  }
}
