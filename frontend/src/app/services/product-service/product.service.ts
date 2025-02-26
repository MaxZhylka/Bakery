import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DataByPagination, PaginationParams, Product, ProductSales } from '../../interfaces';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  private readonly apiUrl = environment.apiUrl + '/Products';

  constructor(private readonly http: HttpClient) { }

  getProducts(params: PaginationParams): Observable<DataByPagination<Product[]>> {
    return this.http.get<DataByPagination<Product[]>>(`${this.apiUrl}/`, { params: { ...params } });
  }

  getProductsByCount(params: PaginationParams): Observable<DataByPagination<Product[]>> {
    return this.http.get<DataByPagination<Product[]>>(`${this.apiUrl}/count-more-than`, { params: { ...params } });
  }

  getProductsByPrice(params: PaginationParams): Observable<DataByPagination<Product[]>> {
    return this.http.get<DataByPagination<Product[]>>(`${this.apiUrl}/price-more-than`, { params: { ...params } });
  }

  getFilteredProducts(params: PaginationParams): Observable<DataByPagination<Product[]>> {
    return this.http.get<DataByPagination<Product[]>>(`${this.apiUrl}/price-more-count-less`, { params: { ...params } });
  }

  createProduct(product: Product): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, product);
  }

  updateProduct(product: Product): Observable<Product> {
    return this.http.put<Product>(`${this.apiUrl}/${product.id}`, product);
  }

  deleteProduct(productId: string): Observable<Product> {
    return this.http.delete<Product>(`${this.apiUrl}/${productId}`);
  }

  getProductsSales(): Observable<ProductSales[]> {
    return this.http.get<ProductSales[]>(`${this.apiUrl}/Sales`);
  }
}
