import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetMoneyFromComponent } from './get-money-from.component';

describe('GetMoneyFromComponent', () => {
  let component: GetMoneyFromComponent;
  let fixture: ComponentFixture<GetMoneyFromComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GetMoneyFromComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GetMoneyFromComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
