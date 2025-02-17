import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User, PaginationParams, DataByPagination } from '../../interfaces';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private readonly apiUrl = '/api/users';

  constructor(private readonly http: HttpClient) { }

  getUsers(params: PaginationParams): Observable<DataByPagination<User[]>> {
    return this.http.get<DataByPagination<User[]>>(this.apiUrl, { params: { ...params } });
  }

  createUser(user: User): Observable<User> {
    return this.http.post<User>(this.apiUrl, user);
  }

  updateUser(user: User): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/${user.id}`, user);
  }

  deleteUser(userId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${userId}`);
  }
}
