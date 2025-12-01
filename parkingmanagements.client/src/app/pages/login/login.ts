import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/services/auth.service';

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

    this.authService.login(this.email, this.password).subscribe({
      next: (response: any) => {
        // save token from backend
        localStorage.setItem('token', response.token);
        // save role
        if (response.user?.role) {
          localStorage.setItem('role', response.user.role);
      }
        //redirect
        this.router.navigate(['/dashboard']);
      },
      error: ()=> {
        this.errorMessage = 'Invalid email or password';
      }
    });
  }
}
