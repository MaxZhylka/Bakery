import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { Store } from '@ngxs/store';
import { first, Observable, Subject, takeUntil } from 'rxjs';
import { DatePipe } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { MatTooltipModule } from '@angular/material/tooltip';


import { Payment, PaginationParams, DataByPagination, User, Roles } from '../../interfaces';

import { UserState } from '../../store/app.state';
import { GetPaymentsByUserId, GetPayments, DeletePayment } from '../../store/payment.actions';
import { PaymentState } from '../../store/payment.state';

@Component({
  selector: 'app-payments',
  standalone: true,
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.scss'],
  imports: [
    MatTableModule,
    MatPaginator,
    DatePipe,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    RouterModule
  ]
})
export class PaymentComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  public displayedColumns: string[] = ['id', 'userId', 'value', 'status', 'createdAt', 'actions'];
  public dataSource: Payment[] = [];
  public paginationParams: PaginationParams = { size: 10, offset: 0 };
  public payments$!: Observable<DataByPagination<Payment[]>>;
  public totalSize: number = 0;
  public userData$!: Observable<User | null>;
  public user!: User;

  private readonly destroy$: Subject<void> = new Subject<void>();

  constructor(private readonly store: Store) { }

  public ngOnInit(): void {
    this.payments$ = this.store.select(PaymentState.payments);

    this.userData$ = this.store.select(UserState.currentUser);

    this.userData$.pipe(first()).subscribe((user) => {
      if (user?.role === Roles.User) {
        this.store.dispatch(new GetPaymentsByUserId(this.paginationParams, user.id));
        this.user = user;
      } else {
        this.store.dispatch(new GetPayments(this.paginationParams));
      }
    });

    this.payments$.pipe(takeUntil(this.destroy$)).subscribe((value) => {
      this.dataSource = value.data;
      this.totalSize = value.total;
    });
  }

  public onPageChange(event: PageEvent): void {
    this.paginationParams.size = event.pageSize;
    this.paginationParams.offset = event.pageIndex;
    if (this.user?.role === Roles.User) {
      this.store.dispatch(new GetPaymentsByUserId(this.paginationParams, this.user.id));
    } else {
      this.store.dispatch(new GetPayments(this.paginationParams));
    }
  }

  public deletePayment(payment: Payment): void {
    this.store.dispatch(new DeletePayment(payment.id, this.paginationParams, this.user));
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
