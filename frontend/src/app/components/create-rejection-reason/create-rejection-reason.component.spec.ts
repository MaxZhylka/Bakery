import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateRejectionReasonComponent } from './create-rejection-reason.component';

describe('CreateRejectionReasonComponent', () => {
  let component: CreateRejectionReasonComponent;
  let fixture: ComponentFixture<CreateRejectionReasonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateRejectionReasonComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateRejectionReasonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
