import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngxs/store';
import { GetProductsSales } from '../../store/products.actions';
import { map, Observable, Subject, takeUntil } from 'rxjs';
import { ProductSales } from '../../interfaces';
import { ProductsState } from '../../store/products.state';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-products-dynamic',
  imports: [NgxChartsModule, CommonModule],
  templateUrl: './products-dynamic.component.html',
  styleUrl: './products-dynamic.component.scss'
})
export class ProductsDynamicComponent implements OnInit, OnDestroy {
  public productSales$!: Observable<ProductSales[]>;
  public productSales!: any[];
  public view: [number, number] = [700, 400];
  public destroy$: Subject<void> = new Subject();
  constructor(private readonly store: Store) { }

  public ngOnInit(): void {
    this.store.dispatch(new GetProductsSales);
    this.productSales$ = this.store.select(ProductsState.productsSales);
    this.productSales$.pipe(
      takeUntil(this.destroy$),
      map((productSales) => 
        productSales.map(product => ({
          name: product.productName,
          value: product.totalSold
        }))
      )
    ).subscribe((productSales) => {
      this.productSales = productSales;
    });
    this.updateViewSize();
  }

  @HostListener('window:resize', ['$event'])
  onResize(): void {
    this.updateViewSize();
  }

  private updateViewSize(): void {
    const width = window.innerWidth * 0.8;
    const height = window.innerHeight * 0.8;
    this.view = [width, height];
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
