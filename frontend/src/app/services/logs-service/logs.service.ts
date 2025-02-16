import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginationParams, DataByPagination, Log } from '../../interfaces';


@Injectable({
  providedIn: 'root'
})
export class LogsService {
  private readonly apiUrl = '/api/logs';

  constructor(private readonly http: HttpClient) {}

  public getLogs(params: PaginationParams): Observable<DataByPagination<Log[]>> {
    const httpParams = new HttpParams({ fromObject: params as any });
    return this.http.get<DataByPagination<Log[]>>(this.apiUrl, { params: httpParams });
  }
}
