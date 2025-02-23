import { Component, HostListener, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { CommonModule } from '@angular/common';
import { MatMenuModule } from '@angular/material/menu';
import { Store } from '@ngxs/store';
import { UserState } from '../store/app.state';
import { filter, Observable, take, takeUntil } from 'rxjs';
import { User } from '../interfaces';

@Component({
  selector: 'app-header',
  imports: [
    CommonModule,
    MatButtonModule,
    RouterModule,
    MatToolbarModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatMenuModule
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class HeaderComponent implements OnInit {
  isMobile = window.innerWidth < 768;
  user$!: Observable<User | null>;
  userData!: User | null;

  @HostListener('window:resize', ['$event'])
  onResize() {
    this.isMobile = window.innerWidth < 768;
  }

  constructor(private readonly store: Store) {}

  public ngOnInit(): void {
    this.user$ = this.store.select(UserState.currentUser);
    this.user$.pipe(filter((user)=>Boolean(user)), take(1)).subscribe((user)=>this.userData = user);
  }
}
