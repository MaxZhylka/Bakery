import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CraeteLoanApplicationComponent } from './craete-loan-application.component';

describe('CraeteLoanApplicationComponent', () => {
  let component: CraeteLoanApplicationComponent;
  let fixture: ComponentFixture<CraeteLoanApplicationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CraeteLoanApplicationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CraeteLoanApplicationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
