import { DataByPagination, PaginationParams, User } from '../interfaces';

export class GetUsers {
  static readonly type = '[Users] Get Users';
  constructor(public paginationParams: PaginationParams) {}
}

export class GetUsersSuccess {
  static readonly type = '[Users] Get Users Success';
  constructor(public payload: DataByPagination<User[]>) {}
}

export class GetUsersFail {
  static readonly type = '[Users] Get Users Fail';
  constructor(public error: string) {}
}

export class CreateUser {
  static readonly type = '[Users] Create User';
  constructor(public user: User) {}
}

export class CreateUserSuccess {
  static readonly type = '[Users] Create User Success';
  constructor(public user: User) {}
}

export class CreateUserFail {
  static readonly type = '[Users] Create User Fail';
  constructor(public error: string) {}
}

export class UpdateUser {
  static readonly type = '[Users] Update User';
  constructor(public user: User) {}
}

export class UpdateUserSuccess {
  static readonly type = '[Users] Update User Success';
  constructor(public user: User) {}
}

export class UpdateUserFail {
  static readonly type = '[Users] Update User Fail';
  constructor(public error: string) {}
}

export class DeleteUser {
  static readonly type = '[Users] Delete User';
  constructor(public userId: string) {}
}

export class DeleteUserSuccess {
  static readonly type = '[Users] Delete User Success';
  constructor(public userId: string) {}
}

export class DeleteUserFail {
  static readonly type = '[Users] Delete User Fail';
  constructor(public error: string) {}
}
