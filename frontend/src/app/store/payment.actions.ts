import { PaginationParams, DataByPagination, Payment, User } from '../interfaces';

export class GetPayments {
  static readonly type = '[Payments] Get payments';
  constructor(public paginationParams: PaginationParams) {}
}

export class GetPaymentsSuccess {
  static readonly type = '[Payments] Get payments success';
  constructor(public payments: DataByPagination<Payment[]>) {}
}

export class GetPaymentsFail {
  static readonly type = '[Payments] Get payments fail';
  constructor(public error: string) {}
}

export class CreatePayment {
  static readonly type = '[Payments] Create payment';
  constructor(public paymentData: any, public paginationParams: PaginationParams) {}
}

export class CreatePaymentSuccess {
  static readonly type = '[Payments] Create payment success';
  constructor(public createdPayment: Payment, public paginationParams: PaginationParams) {}
}

export class CreatePaymentFail {
  static readonly type = '[Payments] Create payment fail';
  constructor(public error: string, public paginationParams: PaginationParams) {}
}

export class UpdatePayment {
  static readonly type = '[Payments] Update payment';
  constructor(public paymentId: string, public updateData: any) {}
}

export class UpdatePaymentSuccess {
  static readonly type = '[Payments] Update payment success';
}

export class UpdatePaymentFail {
  static readonly type = '[Payments] Update payment fail';
  constructor(public error: string) {}
}

export class DeletePayment {
  static readonly type = '[Payments] Delete payment';
  constructor(public paymentId: string, public paginationParams: PaginationParams, public user: User | null) {}
}

export class DeletePaymentSuccess {
  static readonly type = '[Payments] Delete payment success';
  constructor(public paginationParams: PaginationParams, public user: User | null) {}
}

export class DeletePaymentFail {
  static readonly type = '[Payments] Delete payment fail';
  constructor(public error: string) {}
}

export class GetPaymentsByUserId {
  static readonly type = '[Payments] Get payments by user id';
  constructor(public paginationParams: PaginationParams, public userId: string) {}
}
