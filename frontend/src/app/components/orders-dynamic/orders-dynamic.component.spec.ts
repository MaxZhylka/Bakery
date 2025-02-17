import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdersDynamicComponent } from './orders-dynamic.component';

describe('OrdersDynamicComponent', () => {
  let component: OrdersDynamicComponent;
  let fixture: ComponentFixture<OrdersDynamicComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrdersDynamicComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(OrdersDynamicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
