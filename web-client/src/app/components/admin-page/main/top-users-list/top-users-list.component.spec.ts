import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopUsersListComponent } from './top-users-list.component';

describe('TopUsersListComponent', () => {
  let component: TopUsersListComponent;
  let fixture: ComponentFixture<TopUsersListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TopUsersListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TopUsersListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
