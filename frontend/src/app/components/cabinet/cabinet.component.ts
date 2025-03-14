import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card'
import { Store } from '@ngxs/store';
import { Logout } from '../../store/app.actions';
import { ReportService } from '../../services/report-service/report.service';
import { filter, Observable, Subject, takeUntil } from 'rxjs';
import { User, Roles } from '../../interfaces';
import { UserState } from '../../store/app.state';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cabinet',
  imports: [MatButtonModule, MatIconModule, MatCardModule, CommonModule],
  templateUrl: './cabinet.component.html',
  styleUrl: './cabinet.component.scss'
})
export class CabinetComponent implements OnInit {

  user$!: Observable<User | null>;
  destroy$: Subject<void> = new Subject();
  userData!: User | null;
  roles = Roles;

  constructor(private readonly store: Store, private readonly reportService: ReportService) { }

  public ngOnInit(): void {
    this.user$ = this.store.select(UserState.currentUser);
    this.user$.pipe(filter((user) => Boolean(user)), takeUntil(this.destroy$)).subscribe((user) => this.userData = user);
  }

  public logout(): void {
    this.store.dispatch(new Logout());
  }

  downloadProductReport(): void {
    this.reportService.saveProductReport();
  }

  downloadCustomerReport(): void {
    this.reportService.saveCustomerReport();
  }

  downloadAllOrdersReport(): void {
    this.reportService.saveAllOrdersReport();
  }

  downloadOrderTrendsByCustomer(): void {
    this.reportService.saveOrderTrendsByCustomer();
  }

  downloadOrderTrendsByProduct(): void {
    this.reportService.saveOrderTrendsByProduct();
  }
}
