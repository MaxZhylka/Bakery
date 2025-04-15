import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { Store } from '@ngxs/store';
import { first, Observable, Subject, takeUntil } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CommonModule } from '@angular/common';
import { LoanState } from '../../store/loan.state';
import { GetLoans, GetLoansByUserId, DeleteLoan } from '../../store/loan.actions';
import { DataByPagination, Loan, mockLoans, PaginationParams, Roles, User } from '../../interfaces';
import { UserState } from '../../store/app.state';
import { LoanItemComponent } from '../loan-item/loan-item.component';


@Component({
  selector: 'app-loans',
  standalone: true,
  imports: [
    MatTableModule,
    MatPaginator,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    CommonModule,
    RouterModule,
    LoanItemComponent
  ],
  templateUrl: './loans.component.html',
  styleUrls: ['./loans.component.scss']
})
export class LoansComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  public displayedColumns: string[] = [
    'id', 
    'userId', 
    'percent', 
    'valueToPayOnCurrentMonth', 
    'valueToPay', 
    'status', 
    'createdAt', 
    'actions'
  ];
  public dataSource: Loan[] = mockLoans;

  public loans$!: Observable<DataByPagination<Loan[]>>;
  public totalSize: number = 0;

  public paginationParams: PaginationParams = { size: 10, offset: 0 };
  public userData$!: Observable<User | null>;
  public user!: User;

  private readonly destroy$ = new Subject<void>();

  constructor(private readonly store: Store) {}

  public ngOnInit(): void {
    this.loans$ = this.store.select(LoanState.loans);
    this.userData$ = this.store.select(UserState.currentUser);

    this.userData$.pipe(first()).subscribe((user) => {
      if (user?.role === Roles.User) {
        this.store.dispatch(new GetLoansByUserId(this.paginationParams, user.id));
        this.user = user;
      } else {
        this.store.dispatch(new GetLoans(this.paginationParams));
        this.user = user as User;
      }
    });

    this.loans$.pipe(takeUntil(this.destroy$)).subscribe((value) => {
      this.dataSource = value.data;
      this.totalSize = value.total;
    });
  }

  public onPageChange(event: PageEvent): void {
    this.paginationParams.size = event.pageSize;
    this.paginationParams.offset = event.pageIndex;
    if (this.user?.role === Roles.User) {
      this.store.dispatch(new GetLoansByUserId(this.paginationParams, this.user.id));
    } else {
      this.store.dispatch(new GetLoans(this.paginationParams));
    }
  }

  public deleteLoan(loan: Loan): void {
    this.store.dispatch(new DeleteLoan(loan.id, this.paginationParams, this.user || null));
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
