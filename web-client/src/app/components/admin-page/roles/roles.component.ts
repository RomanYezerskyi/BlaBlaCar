import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { CheckboxControlValueAccessor, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { RoleModel } from 'src/app/interfaces/role-model';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { UserStatus } from 'src/app/interfaces/user-interfaces/user-status';
import { AdminService } from 'src/app/services/admin/admin.service';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss']
})
export class RolesComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  isCheckedName: string = '';
  rolesList: Array<RoleModel> = [];
  userEmail: string = '';
  searchUser: UserModel = {} as UserModel;
  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.getAllRoles();
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  getAllRoles(): void {
    this.adminService.getRoles().pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.rolesList = response;
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
  getUser(form: NgForm): void {
    this.adminService.getUserByEmail(this.userEmail).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.searchUser = response;
        this.isCheckedName = this.searchUser.roles[0].name;
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }

  changeRole(event: any, role: string, userId: string): void {
    this.isCheckedName = role;
    const data = { roleName: role, userId: userId };
    this.adminService.changeUserRole(data).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
}
