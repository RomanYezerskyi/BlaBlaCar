import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserBookedTripsComponent } from './user-booked-trips.component';

describe('UserBookedTripsComponent', () => {
  let component: UserBookedTripsComponent;
  let fixture: ComponentFixture<UserBookedTripsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserBookedTripsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserBookedTripsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
