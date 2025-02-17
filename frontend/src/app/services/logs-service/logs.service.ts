import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginationParams, DataByPagination, Log } from '../../interfaces';


@Injectable({
  providedIn: 'root'
})
export class LogsService {
  private readonly apiUrl = '/api/logs';

  constructor(private readonly http: HttpClient) {}

  public getLogs(params: PaginationParams): Observable<DataByPagination<Log[]>> {
    return this.http.get<DataByPagination<Log[]>>(this.apiUrl, { params: { ...params } });
  }
}
