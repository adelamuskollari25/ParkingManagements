import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-users-list',
  imports: [MatIconModule, MatFormFieldModule, MatInputModule],
  templateUrl: './users-list.html',
  styleUrl: './users-list.scss',
})
export class UsersList {
  pwdHide = true;

}
