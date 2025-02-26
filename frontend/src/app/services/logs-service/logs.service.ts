import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginationParams, DataByPagination, Log } from '../../interfaces';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class LogsService {
  private readonly apiUrl = environment.apiUrl + '/Logger';

  constructor(private readonly http: HttpClient) {}

  public getLogs(params: PaginationParams): Observable<DataByPagination<Log[]>> {
    return this.http.get<DataByPagination<Log[]>>(this.apiUrl, { params: { ...params } });
  }
}
