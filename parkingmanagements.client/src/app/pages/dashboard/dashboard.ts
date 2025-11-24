import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatIconModule, RouterModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.scss'],
})
export class Dashboard {

  //get date
  today: Date = new Date();
  greeting: string = '';
  icon: string = '';

  onClickButton() {
    //
  }

  ngOnInit() {
    this.setGreeting();
  }

  // set the icon and greeting depending on time
  setGreeting() {
    const hour = this.today.getHours();

    if (hour < 5) {
      this.icon = 'star';
      this.greeting = 'Heyyy';
    } else if (hour < 12) {
      this.icon = 'wb_sunny';
      this.greeting = 'Good Morning!';
    } else if (hour < 17) {
      this.icon = 'light_mode';
      this.greeting = 'Good Afternoon!';
    } else if (hour < 21) {
      this.icon = 'nightlight';
      this.greeting = 'Good Evening!';
    } else {
      this.icon = 'star';
      this.greeting = 'Heyyy';
    }
  }
}
