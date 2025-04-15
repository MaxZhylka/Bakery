import { PaginationParams, DataByPagination, Loan, User } from "../interfaces";


export class GetLoans {
  static readonly type = '[Loans] Get loans';
  constructor(public paginationParams: PaginationParams) {}
}

export class GetLoansSuccess {
  static readonly type = '[Loans] Get loans success';
  constructor(public loans: DataByPagination<Loan[]>) {}
}

export class GetLoansFail {
  static readonly type = '[Loans] Get loans fail';
  constructor(public error: string) {}
}

export class GetLoansByUserId {
  static readonly type = '[Loans] Get loans by user id';
  constructor(public paginationParams: PaginationParams, public userId: string) {}
}

export class CreateLoan {
  static readonly type = '[Loans] Create loan';
  constructor(public loanData: Loan, public paginationParams: PaginationParams) {}
}

export class CreateLoanSuccess {
  static readonly type = '[Loans] Create loan success';
  constructor(public createdLoan: Loan, public paginationParams: PaginationParams) {}
}

export class CreateLoanFail {
  static readonly type = '[Loans] Create loan fail';
  constructor(public error: string, public paginationParams: PaginationParams) {}
}

export class UpdateLoan {
  static readonly type = '[Loans] Update loan';
  constructor(public loanId: string, public updateData: Loan) {}
}

export class UpdateLoanSuccess {
  static readonly type = '[Loans] Update loan success';
}

export class UpdateLoanFail {
  static readonly type = '[Loans] Update loan fail';
  constructor(public error: string) {}
}

export class DeleteLoan {
  static readonly type = '[Loans] Delete loan';
  constructor(public loanId: string, public paginationParams: PaginationParams, public user: User | null) {}
}

export class DeleteLoanSuccess {
  static readonly type = '[Loans] Delete loan success';
  constructor(public paginationParams: PaginationParams, public user: User | null) {}
}

export class DeleteLoanFail {
  static readonly type = '[Loans] Delete loan fail';
  constructor(public error: string) {}
}
