import { Injectable } from '@angular/core';
import { State, Action, StateContext, Store, Selector } from '@ngxs/store';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, tap } from 'rxjs/operators';
import { of } from 'rxjs';

import {
  GetLoans, GetLoansSuccess, GetLoansFail,
  GetLoansByUserId,
  CreateLoan, CreateLoanSuccess, CreateLoanFail,
  UpdateLoan, UpdateLoanSuccess, UpdateLoanFail,
  DeleteLoan, DeleteLoanSuccess, DeleteLoanFail
} from './loan.actions';

import { SetLoading } from './app.actions';
import { DataByPagination, Loan, Roles } from '../interfaces';
import { LoanService } from '../services/loan-service/loan.service';
import { GetMoney, GetLoanApplications } from './loan-application.actions';

export interface LoanStateModel {
  loans: DataByPagination<Loan[]>;
  error: string | null;
}

@State<LoanStateModel>({
  name: 'loans',
  defaults: {
    loans: { data: [], total: 0 },
    error: null,
  }
})
@Injectable()
export class LoanState {
  constructor(
    private readonly loanService: LoanService,
    private readonly snackBar: MatSnackBar,
    private readonly store: Store
  ) {}

  @Selector()
  static loans(state: LoanStateModel): DataByPagination<Loan[]> {
    return state.loans;
  }

  @Action(GetLoans)
  getLoans(ctx: StateContext<LoanStateModel>, { paginationParams }: GetLoans) {
    this.store.dispatch(new SetLoading(true));
    return this.loanService.getLoans(paginationParams).pipe(
      tap((loans) => ctx.dispatch(new GetLoansSuccess(loans))),
      catchError((error) => {
        ctx.dispatch(new GetLoansFail(error.message));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(GetLoansByUserId)
  getLoansByUserId(ctx: StateContext<LoanStateModel>, { paginationParams, userId }: GetLoansByUserId) {
    this.store.dispatch(new SetLoading(true));
    return this.loanService.getLoansByUserId(userId, paginationParams).pipe(
      tap((loans) => ctx.dispatch(new GetLoansSuccess(loans))),
      catchError((error) => {
        ctx.dispatch(new GetLoansFail(error.message));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(GetLoansSuccess)
  getLoansSuccess(ctx: StateContext<LoanStateModel>, { loans }: GetLoansSuccess) {
    ctx.patchState({ loans, error: null });
  }

  @Action(GetLoansFail)
  getLoansFail(ctx: StateContext<LoanStateModel>, { error }: GetLoansFail) {
    ctx.patchState({ error });
    this.snackBar.open('Помилка завантаження займов', 'Закрити', { duration: 3000 });
  }

  @Action(CreateLoan)
  createLoan(ctx: StateContext<LoanStateModel>, { loanData, paginationParams }: CreateLoan) {
    this.store.dispatch(new SetLoading(true));
    return this.loanService.createLoan(loanData).pipe(
      tap((createdLoan) => ctx.dispatch(new CreateLoanSuccess(createdLoan, paginationParams))),
      catchError((error) => {
        ctx.dispatch(new CreateLoanFail(error.message, paginationParams));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(CreateLoanSuccess)
  createLoanSuccess(ctx: StateContext<LoanStateModel>, { createdLoan, paginationParams }: CreateLoanSuccess) {
    this.snackBar.open('Займ створено успішно', 'Закрити', { duration: 3000 });
    this.store.dispatch(new GetLoans(paginationParams));
  }

  @Action(CreateLoanFail)
  createLoanFail(ctx: StateContext<LoanStateModel>, { error, paginationParams }: CreateLoanFail) {
    ctx.patchState({ error });
    this.snackBar.open('Помилка створення займу', 'Закрити', { duration: 3000 });
    this.store.dispatch(new GetLoans(paginationParams));
  }

  @Action(UpdateLoan)
  updateLoan(ctx: StateContext<LoanStateModel>, { loanId, updateData }: UpdateLoan) {
    this.store.dispatch(new SetLoading(true));
    return this.loanService.updateLoan(loanId, updateData).pipe(
      tap(() => ctx.dispatch(new UpdateLoanSuccess())),
      catchError((error) => {
        ctx.dispatch(new UpdateLoanFail(error.message));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(UpdateLoanSuccess)
  updateLoanSuccess() {
    this.snackBar.open('Займ оновлено', 'Закрити', { duration: 3000 });
  }

  @Action(UpdateLoanFail)
  updateLoanFail(ctx: StateContext<LoanStateModel>, { error }: UpdateLoanFail) {
    ctx.patchState({ error });
    this.snackBar.open('Помилка оновлення займу', 'Закрити', { duration: 3000 });
  }

  @Action(DeleteLoan)
  deleteLoan(ctx: StateContext<LoanStateModel>, { loanId, paginationParams, user }: DeleteLoan) {
    this.store.dispatch(new SetLoading(true));
    return this.loanService.deleteLoan(loanId).pipe(
      tap(() => ctx.dispatch(new DeleteLoanSuccess(paginationParams, user))),
      catchError((error) => {
        ctx.dispatch(new DeleteLoanFail(error.message));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(DeleteLoanSuccess)
  deleteLoanSuccess(ctx: StateContext<LoanStateModel>, { paginationParams, user }: DeleteLoanSuccess) {
    this.snackBar.open('Займ видалено', 'Закрити', { duration: 3000 });

    if (user && user.role === Roles.User) {
      this.store.dispatch(new GetLoansByUserId(paginationParams, user.id));
    } else {
      this.store.dispatch(new GetLoans(paginationParams));
    }
  }

  @Action(DeleteLoanFail)
  deleteLoanFail(ctx: StateContext<LoanStateModel>, { error }: DeleteLoanFail) {
    ctx.patchState({ error });
    this.snackBar.open('Помилка видалення займу', 'Закрити', { duration: 3000 });
  }

  @Action(GetMoney)
  getMoney(ctx: StateContext<LoanStateModel>, { applicationId }: GetMoney) {
    this.store.dispatch(new SetLoading(true));
    return this.loanService.getMoney(applicationId).pipe(
      tap(() => {
        ctx.patchState({ error: null });
        this.snackBar.open('Гроші відправлені', 'Закрити', { duration: 3000 });
        ctx.dispatch(new GetLoans({ size: 10, offset: 0 }));
      }),
      catchError((error) => {
        ctx.patchState({ error });
        this.snackBar.open('Помилка відправки грошей', 'Закрити', { duration: 3000 });
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }
}
