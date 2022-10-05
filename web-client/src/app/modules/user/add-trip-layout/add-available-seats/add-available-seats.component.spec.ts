import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAvailableSeatsComponent } from './add-available-seats.component';

describe('AddAvailableSeatsComponent', () => {
  let component: AddAvailableSeatsComponent;
  let fixture: ComponentFixture<AddAvailableSeatsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddAvailableSeatsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddAvailableSeatsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
