import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestDrivingLicenseComponent } from './request-driving-license.component';

describe('RequestDrivingLicenseComponent', () => {
  let component: RequestDrivingLicenseComponent;
  let fixture: ComponentFixture<RequestDrivingLicenseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RequestDrivingLicenseComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RequestDrivingLicenseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
