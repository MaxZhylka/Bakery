import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginationParams, ICreateOrder, IUpdateOrder, DataByPagination, Order } from '../../interfaces';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class OrdersService {

  private readonly apiUrl = environment.apiUrl+'/Orders';
  constructor(private readonly http: HttpClient) { }

  public getOrders(params: PaginationParams): Observable<DataByPagination<Order[]>> {
    return this.http.get<DataByPagination<Order[]>>(`${this.apiUrl}`, { params: { ...params } });
  }

  public createOrder(orderData: ICreateOrder): Observable<Order> {
    return this.http.post<Order>(`${this.apiUrl}`, orderData);
  }

  public updateOrder(orderId: string, updateData: IUpdateOrder): Observable<Order> {
    return this.http.put<Order>(`${this.apiUrl}/${orderId}`, updateData);
  }

  public deleteOrder(orderId: string): Observable<Order> {
    return this.http.delete<Order>(`${this.apiUrl}/${orderId}`);
  }

  public getOrderDynamic(): void { };
}
