import { Injectable } from '@angular/core';
import { State, Action, StateContext, Store } from '@ngxs/store';
import { ProductsService } from '../services/product-service/product.service';
import {
  GetProducts, GetProductsWhereCountMoreThan100, GetProductsWherePriceMoreThan250, GetProductsWhereCountMoreThan50AndPriceLessThan100,
  CreateProduct, UpdateProduct, DeleteProduct
} from './products.actions';
import { DataByPagination, Product } from '../interfaces';
import { catchError, tap } from 'rxjs/operators';
import { of } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SetLoading } from './app.actions';

export interface ProductsStateModel {
  products: DataByPagination<Product[]>;
  error: string | null;
}

@State<ProductsStateModel>({
  name: 'products',
  defaults: {
    products: { data: [], total: 0 },
    error: null,
  }
})
@Injectable()
export class ProductsState {
  constructor(
    private readonly productsService: ProductsService,
    private readonly store: Store,
    private readonly snackBar: MatSnackBar
  ) { }

  @Action(GetProducts)
  getProducts(ctx: StateContext<ProductsStateModel>, { paginationParams }: GetProducts) {
    this.store.dispatch(new SetLoading(true));
    return this.productsService.getProducts(paginationParams).pipe(
      tap(products => ctx.patchState({ products })),
      catchError(error => {
        ctx.patchState({ error: error.message });
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(GetProductsWhereCountMoreThan100)
  getProductsWhereCountMoreThan100(ctx: StateContext<ProductsStateModel>, { payload }: GetProductsWhereCountMoreThan100) {
    this.store.dispatch(new SetLoading(true));
    return this.productsService.getProductsByCount(payload).pipe(
      tap(products => ctx.patchState({ products })),
      catchError(error => {
        ctx.patchState({ error: error.message });
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(GetProductsWherePriceMoreThan250)
  getProductsWherePriceMoreThan250(ctx: StateContext<ProductsStateModel>, { payload }: GetProductsWherePriceMoreThan250) {
    this.store.dispatch(new SetLoading(true));
    return this.productsService.getProductsByPrice(payload).pipe(
      tap(products => ctx.patchState({ products })),
      catchError(error => {
        ctx.patchState({ error: error.message });
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(GetProductsWhereCountMoreThan50AndPriceLessThan100)
  getProductsWhereCountMoreThan50AndPriceLessThan100(
    ctx: StateContext<ProductsStateModel>,
    { payload }: GetProductsWhereCountMoreThan50AndPriceLessThan100
  ) {
    this.store.dispatch(new SetLoading(true));
    return this.productsService.getFilteredProducts(payload).pipe(
      tap(products => ctx.patchState({ products })),
      catchError(error => {
        ctx.patchState({ error: error.message });
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(CreateProduct)
  createProduct(ctx: StateContext<ProductsStateModel>, { product }: CreateProduct) {
    this.store.dispatch(new SetLoading(true));
    return this.productsService.createProduct(product).pipe(
      tap(() => this.snackBar.open('Продукт створено', 'Закрити', { duration: 3000 })),
      catchError(error => {
        this.snackBar.open('Помилка створення продукту', 'Закрити', { duration: 3000 });
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(UpdateProduct)
  updateProduct(ctx: StateContext<ProductsStateModel>, { product }: UpdateProduct) {
    this.store.dispatch(new SetLoading(true));
    return this.productsService.updateProduct(product).pipe(
      tap(() => this.snackBar.open('Продукт оновлено', 'Закрити', { duration: 3000 })),
      catchError(error => {
        this.snackBar.open('Помилка оновлення продукту', 'Закрити', { duration: 3000 });
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }

  @Action(DeleteProduct)
  deleteProduct(ctx: StateContext<ProductsStateModel>, { productId }: DeleteProduct) {
    this.store.dispatch(new SetLoading(true));
    return this.productsService.deleteProduct(productId).pipe(
      tap(() => this.snackBar.open('Продукт видалено', 'Закрити', { duration: 3000 })),
      catchError(error => {
        this.snackBar.open('Помилка видалення продукту', 'Закрити', { duration: 3000 });
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }
}
