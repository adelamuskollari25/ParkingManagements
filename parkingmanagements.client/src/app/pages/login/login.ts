import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/services/auth.service';
import { LogInRequest } from '../../core/models/user';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule
  ],
  templateUrl: './login.html',
  styleUrls: ['./login.scss'],
})
export class Login {
  //get the user data
  email: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private router: Router, private authService: AuthService) {}

  onLogIn() {

    const loginData: LogInRequest = {
      email: this.email,
      password: this.password
    };

    this.authService.login(loginData).subscribe({
      next: (response: any) => {
        // save token
        localStorage.setItem('token', response.token);
        // save role
        if (response.role) {
          localStorage.setItem('role', response.role);
        }
        //save email
        if (response.email) {
          localStorage.setItem('email', response.email);
        }
        //save userId
        if (response.userId) {
          localStorage.setItem('userId', response.userId);
        }
        //redirect
        this.router.navigate(['/dashboard']);
      },
      error: (err)=> {
        console.error('Login error:', err);
        this.errorMessage = 'Invalid email or password';
      }
    });
  }
}
