import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { Store } from '@ngxs/store';
import { filter, Observable, Subject, take, takeUntil } from 'rxjs';
import { CreateUser, DeleteUser, GetUsers, UpdateUser } from '../../store/users.actions';
import { PaginationParams, DataByPagination, User, Roles } from '../../interfaces';
import { CommonModule, DatePipe } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialog } from '@angular/material/dialog';
import { CreateUserComponent } from '../create-user/create-user.component';
import { UserState } from '../../store/app.state';
import { UsersState } from '../../store/users.state';

@Component({
  selector: 'app-users',
  imports: [MatTableModule, MatPaginator, DatePipe, MatIconModule, MatButtonModule, MatTooltipModule, RouterModule, CommonModule],
  templateUrl: './users.component.html',
  styleUrl: './users.component.scss',
})
export class UsersComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  public displayedColumns: string[] = ['id', 'name', 'email', 'role', 'createdAt'];
  public dataSource: User[] = [{ id: '123daf', name: 'Oleg Vitaliovich', email: 'dexhonesta@gmail.com', role: Roles.Admin, createdAt: '12-04-2005' }];
  public paginationParams: PaginationParams = { size: 10, offset: 0 };
  user$!: Observable<User | null>;
  userData!: User | null;
  public users$!: Observable<DataByPagination<User[]>>;
  public userCount: number = 0;
  private readonly destroy$: Subject<void> = new Subject<void>();

  constructor(private readonly store: Store,
    private readonly dialog: MatDialog) { }

  public ngOnInit(): void {
    this.users$ = this.store.select(UsersState.users);
    this.store.dispatch(new GetUsers(this.paginationParams));
    this.users$.pipe(takeUntil(this.destroy$)).subscribe((value) => {
      this.dataSource = value.data;
      this.userCount = value.total;
    });
    this.user$ = this.store.select(UserState.currentUser);
    this.user$.pipe(filter((user) => Boolean(user)), take(1)).subscribe((user) => {
      this.userData = user; if (this.userData?.role === 'Admin') {
        this.displayedColumns.push('actions');
      }
    });
  }

  public onPageChange(event: PageEvent): void {
    this.paginationParams.size = event.pageSize;
    this.paginationParams.offset = event.pageIndex;
    this.store.dispatch(new GetUsers(this.paginationParams));
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  public deleteUser(user: User) {
    this.store.dispatch(new DeleteUser(user.id));
  };

  public openCreateDialog(): void {
    const dialogRef = this.dialog.open(CreateUserComponent, {
      width: '480px',
      height: '480px',
      data: null,
    });

    dialogRef.afterClosed().subscribe((result: User) => {
      if (result) {
        this.store.dispatch(new CreateUser(result, this.paginationParams))
      }
    });
  }

  public editUser(user: User): void {
    const dialogRef = this.dialog.open(CreateUserComponent, {
      width: '480px',
      height: '380px',
      data: user,
    });

    dialogRef.afterClosed().subscribe((result: User) => {
      if (result) {
        this.store.dispatch(new UpdateUser(result, this.paginationParams))
      }
    });
  }
}
