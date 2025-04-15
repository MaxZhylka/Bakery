import { Injectable } from '@angular/core';
import { State, Action, StateContext, Store, Selector } from '@ngxs/store';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, first, tap } from 'rxjs/operators';
import { of } from 'rxjs';

import {
  GetPayments, GetPaymentsSuccess, GetPaymentsFail,
  CreatePayment, CreatePaymentSuccess, CreatePaymentFail,
  UpdatePayment, UpdatePaymentSuccess, UpdatePaymentFail,
  DeletePayment, DeletePaymentSuccess, DeletePaymentFail,
  GetPaymentsByUserId
} from './payment.actions';

import { PaymentService } from '../services/payment-service/payment.service';
import { Payment, DataByPagination, Roles } from '../interfaces';
import { SetLoading } from './app.actions';
import { UserState } from './app.state';
import { GetLoanApplications, GetLoanApplicationsByUserId } from './loan-application.actions';
import { GetLoans, GetLoansByUserId } from './loan.actions';

export interface PaymentStateModel {
  payments: DataByPagination<Payment[]>;
  error: string | null;
}

@State<PaymentStateModel>({
  name: 'payments',
  defaults: {
    payments: { data: [], total: 0 },
    error: null,
  }
})
@Injectable()
export class PaymentState {

  constructor(
    private readonly paymentService: PaymentService,
    private readonly snackBar: MatSnackBar,
    private readonly store: Store
  ) { }

  @Selector()
  static payments(state: PaymentStateModel): DataByPagination<Payment[]> {
    return state.payments;
  }

  @Action(GetPayments)
  getPayments(ctx: StateContext<PaymentStateModel>, { paginationParams }: GetPayments) {
    this.store.dispatch(new SetLoading(true));
    return this.paymentService.getPayments(paginationParams).pipe(
      tap((payments) => ctx.dispatch(new GetPaymentsSuccess(payments))),
      catchError((error) => {
        ctx.dispatch(new GetPaymentsFail(error.message));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(GetPaymentsByUserId)
  getPaymentsByUserId(ctx: StateContext<PaymentStateModel>, { paginationParams, userId }: GetPaymentsByUserId) {
    this.store.dispatch(new SetLoading(true));
    return this.paymentService.getPaymentsByUserId(userId, paginationParams).pipe(
      tap((payments) => ctx.dispatch(new GetPaymentsSuccess(payments))),
      catchError((error) => {
        ctx.dispatch(new GetPaymentsFail(error.message));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(GetPaymentsSuccess)
  getPaymentsSuccess(ctx: StateContext<PaymentStateModel>, { payments }: GetPaymentsSuccess) {
    ctx.patchState({ payments, error: null });
  }

  @Action(GetPaymentsFail)
  getPaymentsFail(ctx: StateContext<PaymentStateModel>, { error }: GetPaymentsFail) {
    ctx.patchState({ error });
    this.snackBar.open('Помилка завантаження платежів', 'Закрити', { duration: 3000 });
  }

  @Action(CreatePayment)
  createPayment(ctx: StateContext<PaymentStateModel>, { paymentData, paginationParams }: CreatePayment) {
    this.store.dispatch(new SetLoading(true));
    return this.paymentService.createPayment(paymentData).pipe(
      tap((createdPayment) => ctx.dispatch(new CreatePaymentSuccess(createdPayment, paginationParams))),
      catchError((error) => {
        ctx.dispatch(new CreatePaymentFail(error.message, paginationParams));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(CreatePaymentSuccess)
  createPaymentSuccess(ctx: StateContext<PaymentStateModel>, { createdPayment, paginationParams }: CreatePaymentSuccess) {
    this.snackBar.open('Платіж створено успішно', 'Закрити', { duration: 3000 });
    this.store.select(UserState.currentUser).pipe(first()).subscribe((user) => {
      if (user?.role == Roles.User) {
        this.store.dispatch(new GetLoansByUserId(paginationParams, user.id));
      } else {
        this.store.dispatch(new GetLoans(paginationParams))
      }
    });
  }

  @Action(CreatePaymentFail)
  createPaymentFail(ctx: StateContext<PaymentStateModel>, { error, paginationParams }: CreatePaymentFail) {
    ctx.patchState({ error });
    this.snackBar.open('Помилка створення платежу', 'Закрити', { duration: 3000 });
    this.store.dispatch(new GetPayments(paginationParams));
  }

  @Action(UpdatePayment)
  updatePayment(ctx: StateContext<PaymentStateModel>, { paymentId, updateData }: UpdatePayment) {
    this.store.dispatch(new SetLoading(true));
    return this.paymentService.updatePayment(paymentId, updateData).pipe(
      tap(() => ctx.dispatch(new UpdatePaymentSuccess())),
      catchError((error) => {
        ctx.dispatch(new UpdatePaymentFail(error.message));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(UpdatePaymentSuccess)
  updatePaymentSuccess() {
    this.snackBar.open('Платіж оновлено успішно', 'Закрити', { duration: 3000 });
  }

  @Action(UpdatePaymentFail)
  updatePaymentFail(ctx: StateContext<PaymentStateModel>, { error }: UpdatePaymentFail) {
    ctx.patchState({ error });
    this.snackBar.open('Помилка оновлення платежу', 'Закрити', { duration: 3000 });
  }

  @Action(DeletePayment)
  deletePayment(ctx: StateContext<PaymentStateModel>, { paymentId, paginationParams, user }: DeletePayment) {
    this.store.dispatch(new SetLoading(true));
    return this.paymentService.deletePayment(paymentId).pipe(
      tap(() => ctx.dispatch(new DeletePaymentSuccess(paginationParams, user))),
      catchError((error) => {
        ctx.dispatch(new DeletePaymentFail(error.message));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(DeletePaymentSuccess)
  deletePaymentSuccess(ctx: StateContext<PaymentStateModel>, { paginationParams, user }: DeletePaymentSuccess) {
    this.snackBar.open('Платіж видалено', 'Закрити', { duration: 3000 });
    if (user && user.role === Roles.User) {
      this.store.dispatch(new GetPaymentsByUserId(paginationParams, user.id));
    } else {
      this.store.dispatch(new GetPayments(paginationParams));
    }
  }

  @Action(DeletePaymentFail)
  deletePaymentFail(ctx: StateContext<PaymentStateModel>, { error }: DeletePaymentFail) {
    ctx.patchState({ error });
    this.snackBar.open('Помилка видалення платежу', 'Закрити', { duration: 3000 });
  }
}
