import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CraetePaymentComponent } from './craete-payment.component';

describe('CraetePaymentComponent', () => {
  let component: CraetePaymentComponent;
  let fixture: ComponentFixture<CraetePaymentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CraetePaymentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CraetePaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
