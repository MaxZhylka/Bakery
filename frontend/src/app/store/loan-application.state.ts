import { Injectable } from '@angular/core';
import { State, Action, StateContext, Store, Selector } from '@ngxs/store';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, first, tap } from 'rxjs/operators';
import { of } from 'rxjs';


import {
  GetLoanApplications, GetLoanApplicationsSuccess, GetLoanApplicationsFail,
  GetLoanApplicationsByUserId,
  CreateLoanApplication, CreateLoanApplicationSuccess, CreateLoanApplicationFail,
  UpdateLoanApplication, UpdateLoanApplicationSuccess, UpdateLoanApplicationFail,
  DeleteLoanApplication, DeleteLoanApplicationSuccess, DeleteLoanApplicationFail,
  ApproveApplication,
  RejectApplication,
  CreateApplicationDraft,
  GetMoney
} from './loan-application.actions';
import { DataByPagination, ILoanApplicationCreate, LoanApplication, Roles } from '../interfaces';
import { LoanApplicationService } from '../services/loan-application-service/loan-application.service';
import { SetLoading } from './app.actions';
import { UserState } from './app.state';



export interface LoanApplicationStateModel {
  applications: DataByPagination<LoanApplication[]>;
  draftApplication: ILoanApplicationCreate | null;
  error: string | null;
}

@State<LoanApplicationStateModel>({
  name: 'loanApplications',
  defaults: {
    applications: { data: [], total: 0 },
    error: null,
    draftApplication: null
  }
})

@Injectable()
export class LoanApplicationState {
  constructor(
    private readonly loanApplicationService: LoanApplicationService,
    private readonly snackBar: MatSnackBar,
    private readonly store: Store
  ) { }

  @Selector()
  static loanApplications(state: LoanApplicationStateModel): DataByPagination<LoanApplication[]> {
    return state.applications;
  }

  @Selector()
  static getLoanAppDraft(state: LoanApplicationStateModel): ILoanApplicationCreate | null {
    return state.draftApplication;
  }

  @Action(GetLoanApplications)
  getLoanApplications(ctx: StateContext<LoanApplicationStateModel>, { paginationParams }: GetLoanApplications) {
    this.store.dispatch(new SetLoading(true));
    return this.loanApplicationService.getLoanApplications(paginationParams).pipe(
      tap((applications) => ctx.dispatch(new GetLoanApplicationsSuccess(applications))),
      catchError((error) => {
        ctx.dispatch(new GetLoanApplicationsFail(error.message));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(GetLoanApplicationsByUserId)
  getLoanApplicationsByUserId(ctx: StateContext<LoanApplicationStateModel>, { paginationParams, userId }: GetLoanApplicationsByUserId) {
    this.store.dispatch(new SetLoading(true));
    return this.loanApplicationService.getLoanApplicationsByUserId(userId, paginationParams).pipe(
      tap((applications) => ctx.dispatch(new GetLoanApplicationsSuccess(applications))),
      catchError((error) => {
        ctx.dispatch(new GetLoanApplicationsFail(error.message));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(GetLoanApplicationsSuccess)
  getLoanApplicationsSuccess(ctx: StateContext<LoanApplicationStateModel>, { loanApplications }: GetLoanApplicationsSuccess) {
    ctx.patchState({ applications: loanApplications, error: null });
  }

  @Action(GetLoanApplicationsFail)
  getLoanApplicationsFail(ctx: StateContext<LoanApplicationStateModel>, { error }: GetLoanApplicationsFail) {
    ctx.patchState({ error });
    this.snackBar.open('Помилка завантаження заявок', 'Закрити', { duration: 3000 });
  }

  @Action(CreateLoanApplication)
  createLoanApplication(ctx: StateContext<LoanApplicationStateModel>, { applicationData, paginationParams }: CreateLoanApplication) {
    this.store.dispatch(new SetLoading(true));
    return this.loanApplicationService.createLoanApplication(applicationData).pipe(
      tap((createdApplication) => ctx.dispatch(new CreateLoanApplicationSuccess(createdApplication, paginationParams))),
      catchError((error) => {
        ctx.dispatch(new CreateLoanApplicationFail(error.message, paginationParams));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(CreateLoanApplicationSuccess)
  createLoanApplicationSuccess(ctx: StateContext<LoanApplicationStateModel>, { createdApplication, paginationParams }: CreateLoanApplicationSuccess) {
    this.snackBar.open('Заявка створена успішно', 'Закрити', { duration: 3000 });
    this.store.select(UserState.currentUser).pipe(first()).subscribe((user) => {
      if (user?.role == Roles.User) {
        this.store.dispatch(new GetLoanApplicationsByUserId(paginationParams, user.id));
      } else {
        this.store.dispatch(new GetLoanApplications(paginationParams))
      }
    });
  }

  @Action(CreateLoanApplicationFail)
  createLoanApplicationFail(ctx: StateContext<LoanApplicationStateModel>, { error, paginationParams }: CreateLoanApplicationFail) {
    ctx.patchState({ error });
    this.snackBar.open('Помилка створення заявки', 'Закрити', { duration: 3000 });

    this.store.dispatch(new GetLoanApplications(paginationParams));
  }

  @Action(UpdateLoanApplication)
  updateLoanApplication(ctx: StateContext<LoanApplicationStateModel>, { applicationId, updateData }: UpdateLoanApplication) {
    this.store.dispatch(new SetLoading(true));
    return this.loanApplicationService.updateLoanApplication(applicationId, updateData).pipe(
      tap(() => ctx.dispatch(new UpdateLoanApplicationSuccess())),
      catchError((error) => {
        ctx.dispatch(new UpdateLoanApplicationFail(error.message));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(UpdateLoanApplicationSuccess)
  updateLoanApplicationSuccess() {
    this.snackBar.open('Заявка оновлена успішно', 'Закрити', { duration: 3000 });
  }

  @Action(UpdateLoanApplicationFail)
  updateLoanApplicationFail(ctx: StateContext<LoanApplicationStateModel>, { error }: UpdateLoanApplicationFail) {
    ctx.patchState({ error });
    this.snackBar.open('Помилка оновлення заявки', 'Закрити', { duration: 3000 });
  }

  @Action(DeleteLoanApplication)
  deleteLoanApplication(ctx: StateContext<LoanApplicationStateModel>, { applicationId, paginationParams, user }: DeleteLoanApplication) {
    this.store.dispatch(new SetLoading(true));
    return this.loanApplicationService.deleteLoanApplication(applicationId).pipe(
      tap(() => ctx.dispatch(new DeleteLoanApplicationSuccess(paginationParams, user))),
      catchError((error) => {
        ctx.dispatch(new DeleteLoanApplicationFail(error.message));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(DeleteLoanApplicationSuccess)
  deleteLoanApplicationSuccess(ctx: StateContext<LoanApplicationStateModel>, { paginationParams, user }: DeleteLoanApplicationSuccess) {
    this.snackBar.open('Заявку видалено', 'Закрити', { duration: 3000 });
    if (user && user.role === Roles.User) {
      this.store.dispatch(new GetLoanApplicationsByUserId(paginationParams, user.id));
    } else {
      this.store.dispatch(new GetLoanApplications(paginationParams));
    }
  }

  @Action(DeleteLoanApplicationFail)
  deleteLoanApplicationFail(ctx: StateContext<LoanApplicationStateModel>, { error }: DeleteLoanApplicationFail) {
    ctx.patchState({ error });
    this.snackBar.open('Помилка видалення заявки', 'Закрити', { duration: 3000 });
  }

  @Action(ApproveApplication)
  approveLoanApplication(ctx: StateContext<LoanApplicationStateModel>, { applicationId }: ApproveApplication) {
    this.store.dispatch(new SetLoading(true));
    return this.loanApplicationService.approveLoanApplication(applicationId).pipe(
      tap(() => {
        ctx.patchState({ error: null });
        this.snackBar.open('Заявка схвалена', 'Закрити', { duration: 3000 });
        this.store.select(UserState.currentUser).pipe(first()).subscribe((user) => {
          if (user?.role == Roles.User) {
            this.store.dispatch(new GetLoanApplicationsByUserId({ size: 10, offset: 0 }, user.id));
          } else {
            this.store.dispatch(new GetLoanApplications({ size: 10, offset: 0 }));
          }
        });
      }),
      catchError((error) => {
        ctx.patchState({ error });
        this.snackBar.open('Помилка схвалення заявки', 'Закрити', { duration: 3000 });
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(RejectApplication)
  rejectLoanApplication(ctx: StateContext<LoanApplicationStateModel>, { applicationId, reason }: RejectApplication) {
    this.store.dispatch(new SetLoading(true));
    return this.loanApplicationService.rejectLoanApplication(applicationId, reason).pipe(
      tap(() => {
        ctx.patchState({ error: null });
        this.snackBar.open('Заявка відхилена', 'Закрити', { duration: 3000 });
        this.store.select(UserState.currentUser).pipe(first()).subscribe((user) => {
          if (user?.role == Roles.User) {
            this.store.dispatch(new GetLoanApplicationsByUserId({ size: 10, offset: 0 }, user.id));
          } else {
            this.store.dispatch(new GetLoanApplications({ size: 10, offset: 0 }));
          }
        });
      }),
      catchError((error) => {
        ctx.patchState({ error });
        this.snackBar.open('Помилка відхилення заявки', 'Закрити', { duration: 3000 });
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(CreateApplicationDraft)
  createLoanApplicationDraft(ctx: StateContext<LoanApplicationStateModel>, { applicationData }: CreateApplicationDraft) {
    ctx.patchState({ draftApplication: applicationData });
  }
}
