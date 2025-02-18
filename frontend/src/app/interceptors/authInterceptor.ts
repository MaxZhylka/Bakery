import {
    HttpEvent,
    HttpErrorResponse,
    HttpHandlerFn,
    HttpRequest,
    HttpInterceptorFn
} from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthService } from '../services/auth-service/auth.service';

export const authInterceptor: HttpInterceptorFn = (
    req: HttpRequest<unknown>,
    next: HttpHandlerFn
): Observable<HttpEvent<unknown>> => {

    const authService = inject(AuthService);

    if (req.url.includes('/refresh')) {
        return next(req);
    }

    const token = authService.getToken();
    const clonedRequest = token
        ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
        : req;

    return next(clonedRequest).pipe(
        catchError((error) => {
            if (error instanceof HttpErrorResponse && error.status === 401) {
                return handle401Error(req, next, authService);
            }
            return throwError(() => error);
        })
    );
};

function handle401Error(
    req: HttpRequest<unknown>,
    next: HttpHandlerFn,
    authService: AuthService
): Observable<HttpEvent<unknown>> {
    return authService.refreshToken().pipe(
        switchMap((newToken) => {
            // Сохраняем новый токен
            authService.setToken(newToken);
            // Повторяем запрос с обновлённым токеном
            const newRequest = req.clone({
                setHeaders: { Authorization: `Bearer ${newToken}` },
            });
            return next(newRequest);
        }),
        catchError((refreshError) => throwError(() => refreshError))
    );
}
