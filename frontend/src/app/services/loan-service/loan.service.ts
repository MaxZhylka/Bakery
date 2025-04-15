import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { PaginationParams, DataByPagination, Loan } from '../../interfaces';

@Injectable({ providedIn: 'root' })
export class LoanService {
  private readonly apiUrl = environment.apiUrl + '/Loans';

  constructor(private readonly http: HttpClient) { }

  public getLoans(params: PaginationParams): Observable<DataByPagination<Loan[]>> {
    return this.http.get<DataByPagination<Loan[]>>(this.apiUrl, { params: { ...params } });
  }

  public createLoan(loanData: Loan): Observable<Loan> {
    return this.http.post<Loan>(this.apiUrl, loanData);
  }

  public updateLoan(loanId: string, updateData: Loan): Observable<Loan> {
    return this.http.put<Loan>(`${this.apiUrl}/${loanId}`, updateData);
  }

  public deleteLoan(loanId: string): Observable<Loan> {
    return this.http.delete<Loan>(`${this.apiUrl}/${loanId}`);
  }

  public getLoansByUserId(userId: string, params: PaginationParams): Observable<DataByPagination<Loan[]>> {
    return this.http.get<DataByPagination<Loan[]>>(`${this.apiUrl}/user/${userId}`, { params: { ...params } });
  }

  public getMoney(applicationId: string): Observable<Loan> {
    return this.http.post<Loan>(`${this.apiUrl}/${applicationId}`, {});
  }
}
