import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { Store } from '@ngxs/store';
import { DataByPagination, ICreateOrder, PaginationParams, Product, Roles, User } from '../../interfaces';
import { filter, Observable, Subject, take, takeUntil } from 'rxjs';
import { GetProducts, GetProductsWhereCountMoreThan100, GetProductsWhereCountMoreThan50AndPriceLessThan100, GetProductsWherePriceMoreThan250, CreateProduct, UpdateProduct, DeleteProduct } from '../../store/products.actions';
import { CommonModule, DatePipe } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { RouterModule } from '@angular/router';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialog } from '@angular/material/dialog';
import { CreateProductComponent } from '../create-product/create-product.component';
import { CreateOrderComponent } from '../create-order/create-order.component';
import { CreateOrder } from '../../store/orders.actions';
import { UserState } from '../../store/app.state';
import { ProductsState } from '../../store/products.state';

export enum OrderRequestTypes {
  COUNT_MORE_THAN_100 = "Продукти, яких більше 100",
  PRICE_MORE_THAN_250 = "Продукти, ціна яких більше 250",
  COUNT_MORE_THAN_50_AND_PRICE_LESS_THAN_100 = "Продукти, у який ціна меньше 100 і кількість більше 50"
}

@Component({
  selector: 'app-products',
  imports: [CommonModule, MatTableModule, MatPaginator, DatePipe, MatIconModule, MatTooltipModule, MatButtonModule, MatFormFieldModule, MatSelectModule, RouterModule],
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss',
})
export class ProductsComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  public displayedColumns: string[] = ['name', 'price', 'count', 'createdAt', 'actions'];
  public dataSource: Product[] = [{
    id: '1',
    name: 'Шоколадний торт',
    price: 450,
    productCount: 5,
    createdAt: '2025-02-16T10:30:00.000Z'
  },
  {
    id: '2',
    name: 'Фруктовий рулет',
    price: 320,
    productCount: 3,
    createdAt: '2025-02-16T11:15:00.000Z'
  },];
  public paginationParams: PaginationParams = { size: 10, offset: 0 };
  public products$!: Observable<DataByPagination<Product[]>>;
  public requestTypes = Object.values(OrderRequestTypes);
  public totalProductCount: number = 0;
  public user$!: Observable<User | null>;
  public userData!: User | null;
  public role = Roles;

  private readonly destroy$: Subject<void> = new Subject<void>();

  constructor(private readonly store: Store,
    private readonly dialog: MatDialog) { }

  public ngOnInit(): void {
    this.store.dispatch(new GetProducts(this.paginationParams));
    this.products$ = this.store.select(ProductsState.products);
    this.products$.pipe(takeUntil(this.destroy$)).subscribe((value) => { this.dataSource = value.data; this.totalProductCount = value.total });
    this.user$ = this.store.select(UserState.currentUser);
    this.user$.pipe(filter((user) => Boolean(user)), take(1)).subscribe((user) => this.userData = user);
  }

  public onPageChange(event: PageEvent): void {
    this.paginationParams.size = event.pageSize;
    this.paginationParams.offset = event.pageIndex;
    this.store.dispatch(new GetProducts(this.paginationParams));
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  public deleteProduct(product: Product) {
    this.store.dispatch(new DeleteProduct(product.id, this.paginationParams));
  };

  public getNewData(event: MatSelectChange): void {
    switch (event.value) {
      case OrderRequestTypes.COUNT_MORE_THAN_100:
        this.store.dispatch(new GetProductsWhereCountMoreThan100(this.paginationParams));
        break;
      case OrderRequestTypes.PRICE_MORE_THAN_250:
        this.store.dispatch(new GetProductsWherePriceMoreThan250(this.paginationParams));
        break;
      case OrderRequestTypes.COUNT_MORE_THAN_50_AND_PRICE_LESS_THAN_100:
        this.store.dispatch(new GetProductsWhereCountMoreThan50AndPriceLessThan100(this.paginationParams));
        break;
    }
  }

  public createProduct(): void {
    const dialogRef = this.dialog.open(CreateProductComponent, {
      width: '500px',
      height: '380px',
      data: null,
    });

    dialogRef.afterClosed().subscribe((result: Product | undefined) => {
      if (result) {
        this.store.dispatch(new CreateProduct(result, this.paginationParams));
      }
    });
  }

  public editProduct(product: Product): void {
    const dialogRef = this.dialog.open(CreateProductComponent, {
      width: '500px',
      height: '380px',
      data: product,
    });

    dialogRef.afterClosed().subscribe((result: Product | undefined) => {
      if (result) {
        this.store.dispatch(new UpdateProduct(result, this.paginationParams));
      }
    });
  }

  public createOrder(product: Product): void {
    const dialogRef = this.dialog.open(CreateOrderComponent, {
      width: '500px',
      height: '270px',
      data: product,
    })

    dialogRef.afterClosed().subscribe((result: ICreateOrder) => {
      if (result) {
        this.store.dispatch(new CreateOrder(result));
      }
    });
  }
}
