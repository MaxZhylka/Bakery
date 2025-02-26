import { Injectable } from '@angular/core';
import { State, Action, StateContext, Store, Selector } from '@ngxs/store';
import { UsersService } from '../services/user-service/user.service';
import {
  GetUsers, GetUsersSuccess, GetUsersFail,
  CreateUser, CreateUserSuccess, CreateUserFail,
  UpdateUser, UpdateUserSuccess, UpdateUserFail,
  DeleteUser, DeleteUserSuccess, DeleteUserFail
} from './users.actions';
import { User, DataByPagination } from '../interfaces';
import { catchError, tap } from 'rxjs/operators';
import { of } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SetLoading } from './app.actions';

export interface UsersStateModel {
  users: DataByPagination<User[]>;
  error: string | null;
}

@State<UsersStateModel>({
  name: 'users',
  defaults: {
    users: { data: [], total: 0 },
    error: null,
  }
})
@Injectable()
export class UsersState {

  constructor(
    private readonly usersService: UsersService,
    private readonly snackBar: MatSnackBar,
    private readonly store: Store
  ) { }

  @Selector()
  static users(state: UsersStateModel): DataByPagination<User[]> {
    return state.users;
  }
  

  @Action(GetUsers)
  getUsers(ctx: StateContext<UsersStateModel>, { paginationParams }: GetUsers) {
    this.store.dispatch(new SetLoading(true));
    return this.usersService.getUsers(paginationParams).pipe(
      tap((data: DataByPagination<User[]>) => {
        ctx.dispatch(new GetUsersSuccess(data));
      }),
      catchError((error) => {
        ctx.dispatch(new GetUsersFail(error.message));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(GetUsersSuccess)
  getUsersSuccess(ctx: StateContext<UsersStateModel>, { payload }: GetUsersSuccess) {
    ctx.patchState({ users: payload });
  }

  @Action(GetUsersFail)
  getUsersFail(ctx: StateContext<UsersStateModel>, { error }: GetUsersFail) {
    ctx.patchState({ error });
  }

  @Action(CreateUser)
  createUser(ctx: StateContext<UsersStateModel>, { user, paginationParams }: CreateUser) {
    this.store.dispatch(new SetLoading(true));
    return this.usersService.createUser(user).pipe(
      tap((createdUser: User) => {
        ctx.dispatch(new CreateUserSuccess(createdUser, paginationParams));
        this.snackBar.open('Користувача створено успішно', 'Закрити', { duration: 3000 });
      }),
      catchError((error) => {
        ctx.dispatch(new CreateUserFail(error.message));
        this.snackBar.open('Помилка створення користувача', 'Закрити', { duration: 3000 });
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(CreateUserSuccess)
  createUserSuccess(ctx: StateContext<UsersStateModel>, { paginationParams }: CreateUserSuccess) {
    this.store.dispatch(new GetUsers(paginationParams))
  }

  @Action(CreateUserFail)
  createUserFail(ctx: StateContext<UsersStateModel>, { error }: CreateUserFail) {
    ctx.patchState({ error });
  }

  @Action(UpdateUser)
  updateUser(ctx: StateContext<UsersStateModel>, { user, paginationParams }: UpdateUser) {
    this.store.dispatch(new SetLoading(true));
    return this.usersService.updateUser(user).pipe(
      tap((updatedUser: User) => {
        ctx.dispatch(new UpdateUserSuccess(updatedUser, paginationParams));
        this.snackBar.open('Дані користувача оновлено', 'Закрити', { duration: 3000 });
      }),
      catchError((error) => {
        ctx.dispatch(new UpdateUserFail(error.message));
        this.snackBar.open('Помилка оновлення користувача', 'Закрити', { duration: 3000 });
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(UpdateUserSuccess)
  updateUserSuccess(ctx: StateContext<UsersStateModel>, { paginationParams }: UpdateUserSuccess) {
    this.store.dispatch(new GetUsers(paginationParams))
  }

  @Action(UpdateUserFail)
  updateUserFail(ctx: StateContext<UsersStateModel>, { error }: UpdateUserFail) {
    ctx.patchState({ error });
  }

  @Action(DeleteUser)
  deleteUser(ctx: StateContext<UsersStateModel>, { userId }: DeleteUser) {
    this.store.dispatch(new SetLoading(true));
    return this.usersService.deleteUser(userId).pipe(
      tap(() => {
        ctx.dispatch(new DeleteUserSuccess(userId));
        this.snackBar.open('Користувача видалено', 'Закрити', { duration: 3000 });
      }),
      catchError((error) => {
        ctx.dispatch(new DeleteUserFail(error.message));
        this.snackBar.open('Помилка видалення користувача', 'Закрити', { duration: 3000 });
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(DeleteUserSuccess)
  deleteUserSuccess(ctx: StateContext<UsersStateModel>, { userId }: DeleteUserSuccess) {
    const state = ctx.getState();
    ctx.patchState({
      users: {data:state.users.data.filter(user => user.id !== userId), total: state.users.data.length}
    });
  }

  @Action(DeleteUserFail)
  deleteUserFail(ctx: StateContext<UsersStateModel>, { error }: DeleteUserFail) {
    ctx.patchState({ error });
  }
}
