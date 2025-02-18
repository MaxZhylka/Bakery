import { State, Action, StateContext, Selector } from '@ngxs/store';
import { CheckAuth, Login, LoginFail, LoginSuccess, Logout, LogoutFail, LogoutSuccess, Register, RegisterFail, RegisterSuccess, SetLoading, SetUser } from './app.actions';
import { AppStateModel, User, UserStateModel } from '../interfaces';
import { Inject, Injectable } from '@angular/core';
import { MAT_SNACK_BAR_DEFAULT_OPTIONS, MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../services/auth-service/auth.service';
import { catchError, finalize, of, tap } from 'rxjs';
import { Router } from '@angular/router';

@Injectable()
@State<AppStateModel>({
    name: 'app',
    defaults: {
        isLoading: false
    },
})
export class AppState {

    @Selector()
    static isLoading(state: AppStateModel): boolean {
        return state.isLoading;
    }

    @Action(SetLoading)
    setLoading(ctx: StateContext<AppStateModel>, action: SetLoading) {
        ctx.patchState({ isLoading: action.payload });
    }

}

@State<UserStateModel>({
    name: 'user',
    defaults: {
        currentUser: null,
    },
})

@Injectable()
export class UserState {
    constructor(
        @Inject(MAT_SNACK_BAR_DEFAULT_OPTIONS) public data: any,
        private readonly snackBar: MatSnackBar,
        private readonly authService: AuthService,
        private readonly router: Router
    ) { }

    @Selector()
    static currentUser(state: UserStateModel): User | null {
        return state.currentUser;
    }

    @Action(Login)
    login(ctx: StateContext<UserStateModel>, action: Login) {
        ctx.dispatch(new SetLoading(true));

        return this.authService.login(action.payload).pipe(
            tap((response) => {
                console.log(response);
                ctx.dispatch(new LoginSuccess(response));
            }),
            catchError((error) => {
                ctx.dispatch(new LoginFail(error.error.message || 'Login failed'));
                return of(null);
            })
        );
    }

    @Action(Register)
    register(ctx: StateContext<UserStateModel>, action: Register) {
        ctx.dispatch(new SetLoading(true));

        return this.authService.register(action.payload).pipe(
            tap((response) => {
                ctx.dispatch(new RegisterSuccess(response));
            }),
            catchError((error) => {
                ctx.dispatch(new RegisterFail(error.error.message || 'Registration failed'));
                return of(null);
            })
        );
    }

    @Action(LoginSuccess)
    loginSuccess(ctx: StateContext<UserStateModel>, { user }: LoginSuccess) {
        console.log(user);
        ctx.patchState({ currentUser: user });
        ctx.dispatch(new SetLoading(false));
        this.router.navigate(['']);
        this.snackBar.open('Login successful!', 'Close', { panelClass: 'success-snackbar' });
    }

    @Action(LoginFail)
    loginFail(ctx: StateContext<UserStateModel>, action: LoginFail) {
        ctx.dispatch(new SetLoading(false));
        this.snackBar.open(action.payload, 'Close', { panelClass: 'error-snackbar' });
    }

    @Action(RegisterSuccess)
    registerSuccess(ctx: StateContext<UserStateModel>, action: RegisterSuccess) {
        ctx.dispatch(new SetLoading(false));
        this.router.navigate(['']);
        ctx.patchState({
            currentUser: action.payload
        });
        this.snackBar.open('Registration successful!', 'Close', { panelClass: 'success-snackbar' });
    }

    @Action(RegisterFail)
    registerFail(ctx: StateContext<UserStateModel>, action: RegisterFail) {
        this.snackBar.open(action.payload, 'Close', { panelClass: 'error-snackbar' });
        ctx.dispatch(new SetLoading(false));
    }


    @Action(CheckAuth)
    checkAuth(ctx: StateContext<UserStateModel>) {
        ctx.dispatch(new SetLoading(true));

        return this.authService.checkAuth().pipe(
            tap((response) => {
                ctx.dispatch(new SetUser({ ...response }));
            }),
            finalize(() => {
                ctx.dispatch(new SetLoading(false));
            })
        );
    }

    @Action(SetUser)
    setUser(ctx: StateContext<UserStateModel>, action: SetUser) {
        ctx.patchState({ currentUser: action.payload });
    }

    @Action(Logout)
    logout(ctx: StateContext<UserStateModel>) {
        ctx.patchState({ currentUser: null });
        return this.authService.logout().pipe(tap(() => { ctx.dispatch(new LogoutSuccess()) }),
            catchError((error) => {
                ctx.dispatch(new LogoutFail());
                return of(null);
            }));
    }

    @Action(LogoutSuccess)
    logoutSuccess(ctx: StateContext<UserStateModel>) {
        this.router.navigate(['/login']);
        this.snackBar.open('Ви успішно вийшли з аккаунту', 'Close', { panelClass: 'success-snackbar' });
    }

    @Action(LogoutFail)
    logoutFail(ctx: StateContext<UserStateModel>) {
        this.snackBar.open('Помилка виходу', 'Close', { panelClass: 'error-snackbar' });
    }
}
