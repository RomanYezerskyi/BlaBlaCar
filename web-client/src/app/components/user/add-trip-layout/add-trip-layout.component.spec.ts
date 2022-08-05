import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddTripLayoutComponent } from './add-trip-layout.component';

describe('AddTripLayoutComponent', () => {
  let component: AddTripLayoutComponent;
  let fixture: ComponentFixture<AddTripLayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddTripLayoutComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddTripLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
