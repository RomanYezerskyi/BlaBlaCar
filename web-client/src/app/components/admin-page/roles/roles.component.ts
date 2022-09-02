import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { CheckboxControlValueAccessor, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { RoleModel } from 'src/app/interfaces/role';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { UserStatus } from 'src/app/interfaces/user-interfaces/user-status';
import { AdminService } from 'src/app/services/admin/admin.service';
import { UserService } from 'src/app/services/userservice/user.service';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss']
})
export class RolesComponent implements OnInit {
  isCheckedName = '';
  rolesList: Array<RoleModel> = [];
  userEmail = '';
  searchUser: UserModel = {
    id: '', email: '', firstName: '', phoneNumber: '', roles: [], userDocuments: [] = [],
    userStatus: UserStatus.WithoutCar, cars: []
  };
  constructor(private http: HttpClient,
    private router: Router,
    private adminService: AdminService) { }

  ngOnInit(): void {
    this.getAllRoles();

  }
  getAllRoles = () => {
    this.adminService.getRoles().pipe().subscribe(
      response => {
        this.rolesList = response;
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
    // const url = 'https://localhost:5001/api/User/roles'
    // this.http.get(url)
    //   .subscribe({
    //     next: (res) => {
    //       this.rolesList = res as RoleModel[];
    //       console.log(this.rolesList);
    //     },
    //     error: (err: HttpErrorResponse) => console.error(err),
    //   })
  }
  getUser = (form: NgForm) => {
    this.adminService.getUserByEmail(this.userEmail).pipe().subscribe(
      response => {
        this.searchUser = response;
        this.isCheckedName = this.searchUser.roles[0].name;
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
    // const url = 'https://localhost:5001/api/User/'
    // this.http.get(url + this.userEmail)
    //   .subscribe({
    //     next: (res: any) => {
    //       this.searchUser = res as UserModel;
    //       this.isCheckedName = this.searchUser.roles[0].name;
    //       console.log(this.searchUser);
    //     },
    //     error: (err: HttpErrorResponse) => console.error(err),
    //   })
  }

  changeRole(event: any, role: string, userId: string) {
    this.isCheckedName = role;
    const data = { roleName: role, userId: userId };
    this.adminService.changeUserRole(data);
    //const url = 'https://localhost:5001/api/User/';
    // this.http.post(url, data)
    //   .subscribe({
    //     next: (res: any) => {
    //       console.log(res);
    //     },
    //     error: (err: HttpErrorResponse) => console.error(err),
    //   })
  }
}
