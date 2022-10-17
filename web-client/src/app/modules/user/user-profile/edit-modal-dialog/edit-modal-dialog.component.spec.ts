import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditModalDialogComponent } from './edit-modal-dialog.component';

describe('EditModalDialogComponent', () => {
  let component: EditModalDialogComponent;
  let fixture: ComponentFixture<EditModalDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditModalDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditModalDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
