import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoanApplicationItemComponent } from './loan-application-item.component';

describe('LoanApplicationItemComponent', () => {
  let component: LoanApplicationItemComponent;
  let fixture: ComponentFixture<LoanApplicationItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoanApplicationItemComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoanApplicationItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
