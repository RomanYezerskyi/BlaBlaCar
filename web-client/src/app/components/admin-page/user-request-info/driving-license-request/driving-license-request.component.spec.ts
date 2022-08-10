import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DrivingLicenseRequestComponent } from './driving-license-request.component';

describe('DrivingLicenseRequestComponent', () => {
  let component: DrivingLicenseRequestComponent;
  let fixture: ComponentFixture<DrivingLicenseRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DrivingLicenseRequestComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DrivingLicenseRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
