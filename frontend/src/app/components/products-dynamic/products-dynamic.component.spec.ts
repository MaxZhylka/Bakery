import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductsDynamicComponent } from './products-dynamic.component';

describe('ProductsDynamicComponent', () => {
  let component: ProductsDynamicComponent;
  let fixture: ComponentFixture<ProductsDynamicComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductsDynamicComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductsDynamicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
