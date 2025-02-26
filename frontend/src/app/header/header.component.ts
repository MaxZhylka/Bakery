import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { CommonModule } from '@angular/common';
import { MatMenuModule } from '@angular/material/menu';
import { Store } from '@ngxs/store';
import { UserState } from '../store/app.state';
import { filter, Observable, Subject, take, takeUntil } from 'rxjs';
import { Roles, User } from '../interfaces';

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
export class HeaderComponent implements OnInit, OnDestroy {
  isMobile = window.innerWidth < 768;
  user$!: Observable<User | null>;
  destroy$: Subject<void> = new Subject();
  userData!: User | null;
  roles = Roles;

  @HostListener('window:resize', ['$event'])
  onResize() {
    this.isMobile = window.innerWidth < 768;
  }

  constructor(private readonly store: Store, private readonly router: Router) {}

  public ngOnInit(): void {
    this.user$ = this.store.select(UserState.currentUser);
    this.user$.pipe(filter((user)=>Boolean(user)), takeUntil(this.destroy$)).subscribe((user)=>this.userData = user);
  }

  public displayHeader(): boolean {
    const currentUrl = this.router.url;
    return !(currentUrl.includes('login') || currentUrl.includes('registration'));
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
