import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { Store } from '@ngxs/store';
import { Observable, Subject, takeUntil } from 'rxjs';
import { GetLogs } from '../../store/logs.actions';
import { DataByPagination, Log, Roles, actionsList, rolesList, LogsPaginationParams } from '../../interfaces';
import { DatePipe } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { LogsState } from '../../store/logs.state';
import { MatSelectModule } from '@angular/material/select';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-logs',
  imports: [MatTableModule, MatPaginator, DatePipe, MatIconModule, MatButtonModule, MatSelectModule, ReactiveFormsModule],
  templateUrl: './logs.component.html',
  styleUrls: ['./logs.component.scss'],
})
export class LogsComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  public displayedColumns: string[] = ['id', 'userName', 'userRole', 'operation', 'timestamp'];
  public dataSource: Log[] = [{ id: '1', userName: 'Max Zhylka', userRole: Roles.Admin, operation: 'create', details: '', timestamp: '12-04-2025' }];
  public paginationParams: LogsPaginationParams = { size: 10, offset: 0 };
  public userActions = actionsList;
  public userRoles = rolesList;
  public logs$!: Observable<DataByPagination<Log[]>>;
  public totalCount: number = 0;
  public form: FormGroup
  private readonly destroy$: Subject<void> = new Subject<void>();

  constructor(private readonly store: Store, private readonly fb: FormBuilder) {
    this.form = this.fb.group({
      action: "",
      role: "",
    })
  }

  public ngOnInit(): void {
    this.logs$ = this.store.select(LogsState.logs);
    this.store.dispatch(new GetLogs(this.paginationParams));
    this.logs$.pipe(takeUntil(this.destroy$)).subscribe((value) => {
      this.dataSource = value.data;
      this.totalCount = value.total;
    });

    this.form.get('action')?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((value) => {
      this.paginationParams.operation = value;
      this.store.dispatch(new GetLogs(this.paginationParams));
    });

    this.form.get('role')?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe((value) => {
      this.paginationParams.userRole = value;
      this.store.dispatch(new GetLogs(this.paginationParams));
    });
  }

  public onPageChange(event: PageEvent): void {
    this.paginationParams.size = event.pageSize;
    this.paginationParams.offset = event.pageIndex;
    this.store.dispatch(new GetLogs(this.paginationParams));
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

}
