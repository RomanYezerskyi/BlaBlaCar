import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditCarModalDialogComponent } from './edit-car-modal-dialog.component';

describe('EditCarModalDialogComponent', () => {
  let component: EditCarModalDialogComponent;
  let fixture: ComponentFixture<EditCarModalDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditCarModalDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditCarModalDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
