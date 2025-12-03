import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { AuthResponse, LogInRequest, User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  login(data: LogInRequest) {
    return this.http.post<AuthResponse>(`${this.baseUrl}/login`, data);
  }

  getRole(): string | null {
    const token = localStorage.getItem('token');
    if (!token) return null;

    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.role || null;
  }

  getCurrentUser() {
    return this.http.get<User>(`${this.baseUrl}/current-user`);
  }
}
