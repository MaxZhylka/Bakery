import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { Store } from '@ngxs/store';
import { filter, first, Observable, Subject, takeUntil } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { MatTooltipModule } from '@angular/material/tooltip';

import { LoanApplicationState } from '../../store/loan-application.state';
import {
  GetLoanApplications,
  GetLoanApplicationsByUserId,
  DeleteLoanApplication,
  CreateLoanApplication
} from '../../store/loan-application.actions';
import { LoanApplication, DataByPagination, PaginationParams, User, Roles, mockLoanApplications } from '../../interfaces';
import { UserState } from '../../store/app.state';
import { LoanApplicationItemComponent } from '../loan-application-item/loan-application-item.component';
import { CommonModule } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { CreateLoanApplicationComponent } from '../craete-loan-application/craete-loan-application.component';


@Component({
  selector: 'app-loan-applications',
  standalone: true,
  imports: [
    MatTableModule,
    MatPaginator,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
    LoanApplicationItemComponent,
    CommonModule,
    RouterModule
  ],
  templateUrl: './loan-applications.component.html',
  styleUrls: ['./loan-applications.component.scss']
})
export class LoanApplicationsComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  public displayedColumns: string[] = [
    'id',
    'userId',
    'value',
    'status',
    'term',
    'createdAt',
    'actions'
  ];

  public dataSource: LoanApplication[] = mockLoanApplications;

  public applications$!: Observable<DataByPagination<LoanApplication[]>>;
  public totalSize: number = mockLoanApplications.length;
  public paginationParams: PaginationParams = { size: 10, offset: 0 };

  public roles = Roles;
  public userData$!: Observable<User | null>;
  public user!: User;

  private readonly destroy$ = new Subject<void>();

  constructor(private readonly store: Store,
    private readonly dialog: MatDialog) { }

  public ngOnInit(): void {
    this.applications$ = this.store.select(LoanApplicationState.loanApplications);

    this.userData$ = this.store.select(UserState.currentUser);

    this.userData$.pipe(first(), filter((user) => Boolean(user))).subscribe((user) => {
      if (user?.role === Roles.User) {
        this.store.dispatch(new GetLoanApplicationsByUserId(this.paginationParams, user.id));
        this.user = user;
      } else {
        this.store.dispatch(new GetLoanApplications(this.paginationParams));
        this.user = user as User;
      }
    });

    this.applications$.pipe(takeUntil(this.destroy$)).subscribe((value) => {
      this.dataSource = value.data;
      this.totalSize = value.total;
    });
  }

  public onPageChange(event: PageEvent): void {
    this.paginationParams.size = event.pageSize;
    this.paginationParams.offset = event.pageIndex;

    if (this.user?.role === Roles.User) {
      this.store.dispatch(new GetLoanApplicationsByUserId(this.paginationParams, this.user.id));
    } else {
      this.store.dispatch(new GetLoanApplications(this.paginationParams));
    }
  }

  public deleteApplication(application: LoanApplication): void {
    this.store.dispatch(
      new DeleteLoanApplication(application.id, this.paginationParams, this.user || null)
    );
  }

  public createLoanApplication(): void {
    const dialogRef = this.dialog.open(CreateLoanApplicationComponent, {
      width: '600px',
      height: '600px',
      data: null,
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      this.store.dispatch(new CreateLoanApplication(result, this.paginationParams));
    }
  });
}

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
