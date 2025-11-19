import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

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

  constructor(private router: Router) {}

  onLogIn() {
    if (this.email && this.password) {
      console.log("Login successful!");
      this.router.navigate(['/dashboard']);
    } else {
      this.errorMessage = 'Please enter username and password'
    }
  }
}
