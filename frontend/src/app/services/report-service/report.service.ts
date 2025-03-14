import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ReportService {
  private readonly apiUrl = environment.apiUrl + '/reports';

  constructor(private readonly http: HttpClient) {}

  downloadProductReport(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/by-product`, { responseType: 'blob' });
  }
  downloadCustomerReport(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/by-customer`, { responseType: 'blob' });
  }

  downloadAllOrdersReport(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/all-orders`, { responseType: 'blob' });
  }

  downloadOrderTrendsByCustomer(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/trends-by-customer`, { responseType: 'blob' });
  }

  downloadOrderTrendsByProduct(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/trends-by-product`, { responseType: 'blob' });
  }

  private downloadFile(blob: Blob, fileName: string): void {
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = fileName;
    link.click();
  }

  saveProductReport(): void {
    this.downloadProductReport().subscribe((blob) => this.downloadFile(blob, 'product-report.pdf'));
  }

  saveCustomerReport(): void {
    this.downloadCustomerReport().subscribe((blob) => this.downloadFile(blob, 'customer-report.pdf'));
  }

  saveAllOrdersReport(): void {
    this.downloadAllOrdersReport().subscribe((blob) => this.downloadFile(blob, 'all-orders-report.pdf'));
  }

  saveOrderTrendsByCustomer(): void {
    this.downloadOrderTrendsByCustomer().subscribe((blob) => this.downloadFile(blob, 'order-trends-by-customer.pdf'));
  }

  saveOrderTrendsByProduct(): void {
    this.downloadOrderTrendsByProduct().subscribe((blob) => this.downloadFile(blob, 'order-trends-by-product.pdf'));
  }
}
