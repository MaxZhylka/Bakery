import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card'
import { Store } from '@ngxs/store';
import { Logout } from '../../store/app.actions';

@Component({
  selector: 'app-cabinet',
  imports: [MatButtonModule, MatIconModule, MatCardModule],
  templateUrl: './cabinet.component.html',
  styleUrl: './cabinet.component.scss'
})
export class CabinetComponent {

  constructor(private readonly store: Store) {}

  public logout(): void {
    this.store.dispatch(new Logout());
  }
}
