import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DataByPagination, PaginationParams, Product } from '../../interfaces';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
    private readonly apiUrl = environment.apiUrl+'/Products';

  constructor(private readonly http: HttpClient) { }

  getProducts(params: PaginationParams): Observable<DataByPagination<Product[]>> {
    const httpParams = new HttpParams({ fromObject: params as unknown as { [param: string]: string } });
    return this.http.get<DataByPagination<Product[]>>(`${this.apiUrl}/`, { params: httpParams });
  }

  getProductsByCount(params: PaginationParams): Observable<DataByPagination<Product[]>> {
    const httpParams = new HttpParams({ fromObject: params as unknown as { [param: string]: string } });
    return this.http.get<DataByPagination<Product[]>>(`${this.apiUrl}/count-more-than`, { params: httpParams });
  }

  getProductsByPrice(params: PaginationParams): Observable<DataByPagination<Product[]>> {
    const httpParams = new HttpParams({ fromObject: params as unknown as { [param: string]: string } });
    return this.http.get<DataByPagination<Product[]>>(`${this.apiUrl}/price-more-than`, { params: httpParams });
  }

  getFilteredProducts(params: PaginationParams): Observable<DataByPagination<Product[]>> {
    const httpParams = new HttpParams({ fromObject: params as unknown as { [param: string]: string } });
    return this.http.get<DataByPagination<Product[]>>(`${this.apiUrl}/price-more-count-less`, { params: httpParams });
  }

  createProduct(product: Product): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, product);
  }

  updateProduct(product: Product): Observable<Product> {
    return this.http.put<Product>(`${this.apiUrl}/${product.id}`, product);
  }

  deleteProduct(productId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${productId}`);
  }
}
