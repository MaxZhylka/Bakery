import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  PaginationParams,
  DataByPagination,
  Payment
} from '../../interfaces';

@Injectable({ providedIn: 'root' })
export class PaymentService {
  private readonly apiUrl = environment.apiUrl + '/Payments';

  constructor(private readonly http: HttpClient) { }

  public getPayments(params: PaginationParams): Observable<DataByPagination<Payment[]>> {
    return this.http.get<DataByPagination<Payment[]>>(this.apiUrl, {
      params: { ...params },
    });
  }

  public createPayment(paymentData: Payment): Observable<Payment> {
    return this.http.post<Payment>(this.apiUrl, paymentData);
  }

  public updatePayment(paymentId: string, updateData: Payment): Observable<Payment> {
    return this.http.put<Payment>(`${this.apiUrl}/${paymentId}`, updateData);
  }

  public deletePayment(paymentId: string): Observable<Payment> {
    return this.http.delete<Payment>(`${this.apiUrl}/${paymentId}`);
  }

  public getPaymentsByUserId(userId: string, params: PaginationParams): Observable<DataByPagination<Payment[]>> {
    return this.http.get<DataByPagination<Payment[]>>(`${this.apiUrl}/user/${userId}`, {
      params: { ...params },
    });
  }
}
