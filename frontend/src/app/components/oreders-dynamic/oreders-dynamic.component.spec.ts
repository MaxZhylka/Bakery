import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OredersDynamicComponent } from './oreders-dynamic.component';

describe('OredersDynamicComponent', () => {
  let component: OredersDynamicComponent;
  let fixture: ComponentFixture<OredersDynamicComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OredersDynamicComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OredersDynamicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
