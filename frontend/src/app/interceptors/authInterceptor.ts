import { Injectable } from '@angular/core';
import {
    HttpEvent,
    HttpHandler,
    HttpInterceptor,
    HttpRequest,
    HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthService } from '../services/auth-service/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(private readonly authService: AuthService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (req.url.includes('/refresh')) {
            return next.handle(req);
        }
        const token = this.authService.getToken();
        const clonedRequest = token

            ? req.clone({ setHeaders: { authorization: `Bearer ${token}` } })
            : req;

        return next.handle(clonedRequest).pipe(
            catchError((error) => {
                if (error instanceof HttpErrorResponse && error.status === 401) {
                    return this.handle401Error(req, next);
                }
                return throwError(() => error);
            })
        );
    }

    private handle401Error(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return this.authService.refreshToken().pipe(
            switchMap((newToken) => {
                this.authService.setToken(newToken);
                const newRequest = req.clone({ setHeaders: { Authorization: `Bearer ${newToken}` } });
                return next.handle(newRequest);
            }),
            catchError((refreshError) => {
                return throwError(() => refreshError);
            }),
        );
    }
}
