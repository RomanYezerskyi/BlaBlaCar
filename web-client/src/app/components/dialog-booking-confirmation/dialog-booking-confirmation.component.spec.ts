import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogBookingConfirmationComponent } from './dialog-booking-confirmation.component';

describe('DialogBookingConfirmationComponent', () => {
  let component: DialogBookingConfirmationComponent;
  let fixture: ComponentFixture<DialogBookingConfirmationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DialogBookingConfirmationComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DialogBookingConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
