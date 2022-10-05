import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateNotificationDialogComponent } from './create-notification-dialog.component';

describe('CreateNotificationDialogComponent', () => {
  let component: CreateNotificationDialogComponent;
  let fixture: ComponentFixture<CreateNotificationDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateNotificationDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateNotificationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
