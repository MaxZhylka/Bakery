import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { PaginationParams, DataByPagination, LoanApplication, ILoanApplicationCreate } from '../../interfaces';


@Injectable({ providedIn: 'root' })
export class LoanApplicationService {
  private readonly apiUrl = environment.apiUrl + '/Loan-Applications';

  constructor(private readonly http: HttpClient) { }

  public getLoanApplications(params: PaginationParams): Observable<DataByPagination<LoanApplication[]>> {
    return this.http.get<DataByPagination<LoanApplication[]>>(this.apiUrl, {
      params: { ...params },
    });
  }

  public createLoanApplication(data: ILoanApplicationCreate): Observable<LoanApplication> {
    return this.http.post<LoanApplication>(this.apiUrl, data);
  }

  public updateLoanApplication(applicationId: string, updateData: LoanApplication): Observable<LoanApplication> {
    return this.http.put<LoanApplication>(`${this.apiUrl}/${applicationId}`, updateData);
  }

  public deleteLoanApplication(applicationId: string): Observable<LoanApplication> {
    return this.http.delete<LoanApplication>(`${this.apiUrl}/${applicationId}`);
  }

  public getLoanApplicationsByUserId(userId: string, params: PaginationParams): Observable<DataByPagination<LoanApplication[]>> {
    return this.http.get<DataByPagination<LoanApplication[]>>(`${this.apiUrl}/user/${userId}`, {
      params: { ...params },
    });
  }

  public approveLoanApplication(applicationId: string): Observable<LoanApplication> {
    return this.http.post<LoanApplication>(`${this.apiUrl}/approve/${applicationId}`, {});
  }

  public rejectLoanApplication(applicationId: string): Observable<LoanApplication> {
    return this.http.post<LoanApplication>(`${this.apiUrl}/reject/${applicationId}`, {});
  }


}
