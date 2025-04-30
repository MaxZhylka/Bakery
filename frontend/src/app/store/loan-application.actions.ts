import { DataByPagination, ILoanApplicationCreate, LoanApplication, PaginationParams, User } from "../interfaces";

export class GetLoanApplications {
  static readonly type = '[LoanApplications] Get loan applications';
  constructor(public paginationParams: PaginationParams) { }
}

export class GetLoanApplicationsSuccess {
  static readonly type = '[LoanApplications] Get loan applications success';
  constructor(public loanApplications: DataByPagination<LoanApplication[]>) { }
}

export class GetLoanApplicationsFail {
  static readonly type = '[LoanApplications] Get loan applications fail';
  constructor(public error: string) { }
}

export class GetLoanApplicationsByUserId {
  static readonly type = '[LoanApplications] Get loan applications by userId';
  constructor(public paginationParams: PaginationParams, public userId: string) { }
}

export class CreateLoanApplication {
  static readonly type = '[LoanApplications] Create loan application';
  constructor(public applicationData: ILoanApplicationCreate, public paginationParams: PaginationParams) { }
}

export class CreateLoanApplicationSuccess {
  static readonly type = '[LoanApplications] Create loan application success';
  constructor(public createdApplication: LoanApplication, public paginationParams: PaginationParams) { }
}

export class CreateLoanApplicationFail {
  static readonly type = '[LoanApplications] Create loan application fail';
  constructor(public error: string, public paginationParams: PaginationParams) { }
}

export class UpdateLoanApplication {
  static readonly type = '[LoanApplications] Update loan application';
  constructor(public applicationId: string, public updateData: LoanApplication) { }
}

export class UpdateLoanApplicationSuccess {
  static readonly type = '[LoanApplications] Update loan application success';
}

export class UpdateLoanApplicationFail {
  static readonly type = '[LoanApplications] Update loan application fail';
  constructor(public error: string) { }
}

export class DeleteLoanApplication {
  static readonly type = '[LoanApplications] Delete loan application';
  constructor(public applicationId: string, public paginationParams: PaginationParams, public user: User | null) { }
}

export class DeleteLoanApplicationSuccess {
  static readonly type = '[LoanApplications] Delete loan application success';
  constructor(public paginationParams: PaginationParams, public user: User | null) { }
}

export class DeleteLoanApplicationFail {
  static readonly type = '[LoanApplications] Delete loan application fail';
  constructor(public error: string) { }
}

export class ApproveApplication {
  static readonly type = '[LoanApplications] Approve loan application';
  constructor(public applicationId: string) { }
}

export class RejectApplication {
  static readonly type = '[LoanApplications] Reject loan application';
  constructor(public applicationId: string, public reason: string) { }
}

export class CreateApplicationDraft {
  static readonly type = '[LoanApplications] Create application draft';
  constructor(public applicationData: ILoanApplicationCreate) { }
}

export class GetMoney {
  static readonly type = '[LoanApplications] Get money';
  constructor(public applicationId: string) { }
}