import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TripPageInfoComponent } from './trip-page-info.component';

describe('TripPageInfoComponent', () => {
  let component: TripPageInfoComponent;
  let fixture: ComponentFixture<TripPageInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TripPageInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TripPageInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
