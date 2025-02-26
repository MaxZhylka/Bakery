import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { Store } from '@ngxs/store';
import { first, Observable, Subject, takeUntil } from 'rxjs';
import { GetOrders, GetOrdersByUserId } from '../../store/orders.actions';
import { PaginationParams, DataByPagination, Order, User, Roles } from '../../interfaces';
import { DatePipe } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { MatTooltipModule } from '@angular/material/tooltip';
import { OrdersState } from '../../store/orders.state';
import { UserState } from '../../store/app.state';

@Component({
  selector: 'app-orders',
  imports: [MatTableModule, MatPaginator, DatePipe, MatIconModule, MatButtonModule, MatTooltipModule, DatePipe, RouterModule],
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss'],
})
export class OrdersComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  public displayedColumns: string[] = ['id', 'customerName', 'productName', 'productCount', 'price', 'createdAt', 'actions'];
  public dataSource: Order[] = [
    { id: '1', productName: 'P123', productCount: 123, price: 250, createdAt: '2025-04-12T14:23:00', customerName: 'Іван Петренко' }
  ];
  public paginationParams: PaginationParams = { size: 10, offset: 0 };
  public orders$!: Observable<DataByPagination<Order[]>>;
  public totalSize: number = 0;
  public userData$!: Observable<User | null>;
  private readonly destroy$: Subject<void> = new Subject<void>();

  constructor(private readonly store: Store) { }

  public ngOnInit(): void {
    this.orders$ = this.store.select(OrdersState.orders);
    this.userData$ = this.store.select(UserState.currentUser);
    this.userData$.pipe(first()).subscribe((user) => {
      if (user?.role === Roles.User) {
        this.store.dispatch(new GetOrdersByUserId(this.paginationParams, user.id));
      } else {
        this.store.dispatch(new GetOrders(this.paginationParams));
      }
    });

    this.orders$.pipe(takeUntil(this.destroy$)).subscribe((value) => {
      this.dataSource = value.data;
      this.totalSize = value.total;
    });
  }

  public onPageChange(event: PageEvent): void {
    this.paginationParams.size = event.pageSize;
    this.paginationParams.offset = event.pageIndex;
    this.store.dispatch(new GetOrders(this.paginationParams));
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  public deleteOrder(event: Event): void {
    console.log('Delete order clicked:', event);
  }

  public editOrder(event: Event): void {
    console.log('Edit order clicked:', event);
  }
}
