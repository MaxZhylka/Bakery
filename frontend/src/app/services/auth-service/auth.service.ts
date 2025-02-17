import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';
import { LoginPayload, RegisterPayload } from '../../interfaces';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = environment.apiUrl;
  private readonly TOKEN_KEY = 'token';
  private readonly isAuthenticated$ = new BehaviorSubject<boolean>(false);

  constructor(private readonly http: HttpClient,
    private readonly router: Router) { }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  setToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
    this.isAuthenticated$.next(true);
  }

  login(loginPayload: LoginPayload): Observable<any> {
    return this.http.post(`${this.apiUrl}/Auth/login`, loginPayload, { withCredentials: true }).pipe(
      tap((response: any) => {
        this.setToken(response.accessToken);
      })
    );
  }

  register(registerPayload: RegisterPayload): Observable<any> {
    return this.http.post(`${this.apiUrl}/Auth/registration`, registerPayload, { withCredentials: true }).pipe(
      tap((response: any) => {
        this.setToken(response.accessToken);
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    this.isAuthenticated$.next(false);
    this.router.navigate(['/login']);
  }

  refreshToken(): Observable<string> {
    return this.http.get<{ accessToken: string }>(`${this.apiUrl}/Auth/refresh`, { withCredentials: true }).pipe(
      tap((response) => {
        this.setToken(response.accessToken);
      }),
      map((response) => response.accessToken)
    );
  }

  getIsAuthenticated(): Observable<boolean> {
    return this.isAuthenticated$.asObservable();
  }

  checkAuth(): Observable<any> {
    return this.http.get(`${this.apiUrl}/Auth/refresh`, { withCredentials: true }).pipe(
      tap((response: any) => {
        this.isAuthenticated$.next(true);
        localStorage.setItem('token', response.accessToken);
      })
    );
  }
}