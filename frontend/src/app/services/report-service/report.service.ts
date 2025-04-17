import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ReportService {
  private readonly apiUrl = environment.apiUrl + '/reports';

  constructor(private readonly http: HttpClient) {}

  downloadLoanApplicationsReport(from: Date, to: Date): Observable<Blob> {
    const params = new HttpParams()
      .set('from', from.toISOString())
      .set('to', to.toISOString());
    return this.http.get(`${this.apiUrl}/loan-applications-month`, { params, responseType: 'blob' });
  }

  downloadMoneyFlowReport(from: Date, to: Date): Observable<Blob> {
    const params = new HttpParams()
      .set('from', from.toISOString())
      .set('to', to.toISOString());
    return this.http.get(`${this.apiUrl}/money-flow`, { params, responseType: 'blob' });
  }

  downloadUserApplicationsReport(from: Date, to: Date): Observable<Blob> {
    const params = new HttpParams()
      .set('from', from.toISOString())
      .set('to', to.toISOString());
    return this.http.get(`${this.apiUrl}/applications-per-user`, { params, responseType: 'blob' });
  }

  downloadMonthlyPaymentsReport(from: Date, to: Date): Observable<Blob> {
    const params = new HttpParams()
      .set('from', from.toISOString())
      .set('to', to.toISOString());
    return this.http.get(`${this.apiUrl}/monthly-payments`, { params, responseType: 'blob' });
  }

  downloadCompletedLoansReport(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/completed-loans`, { responseType: 'blob' });
  }

  private downloadFile(blob: Blob, fileName: string): void {
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = fileName;
    link.click();
  }


  saveLoanApplicationsReport(from: Date, to: Date): void {
    this.downloadLoanApplicationsReport(from, to)
      .subscribe(blob => this.downloadFile(blob, `LoanApplications_${from.toISOString().slice(0,10)}_${to.toISOString().slice(0,10)}.pdf`));
  }

  saveMoneyFlowReport(from: Date, to: Date): void {
    this.downloadMoneyFlowReport(from, to)
      .subscribe(blob => this.downloadFile(blob, `MoneyFlow_${from.toISOString().slice(0,10)}_${to.toISOString().slice(0,10)}.pdf`));
  }

  saveUserApplicationsReport(from: Date, to: Date): void {
    this.downloadUserApplicationsReport(from, to)
      .subscribe(blob => this.downloadFile(blob, `UserApplications_${from.toISOString().slice(0,10)}_${to.toISOString().slice(0,10)}.pdf`));
  }

  saveMonthlyPaymentsReport(from: Date, to: Date): void {
    this.downloadMonthlyPaymentsReport(from, to)
      .subscribe(blob => this.downloadFile(blob, `MonthlyPayments_${from.toISOString().slice(0,10)}_${to.toISOString().slice(0,10)}.pdf`));
  }

  saveCompletedLoansReport(): void {
    this.downloadCompletedLoansReport()
      .subscribe(blob => this.downloadFile(blob, `CompletedLoansReport.pdf`));
  }
}
