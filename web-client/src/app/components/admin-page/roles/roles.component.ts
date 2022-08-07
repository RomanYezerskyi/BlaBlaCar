import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { CheckboxControlValueAccessor, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { RoleModel } from 'src/app/interfaces/role';
import { UserModel } from 'src/app/interfaces/user-model';
import { UserStatus } from 'src/app/interfaces/user-status';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss']
})
export class RolesComponent implements OnInit {
  isCheckedName = '';
  rolesList: Array<RoleModel> = [];
  userEmail = '';
  searchUser:UserModel = { id:'', email:'',firstName:'',phoneNumber:'', roles:[], drivingLicense:'', 
  userStatus:UserStatus.WithoutCar, cars:[]};
  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.getAllRoles();
    
  }
  getAllRoles =  () => {
    const url ='https://localhost:5001/api/User/roles'
    this.http.get(url)
      .subscribe({
        next: (res) => {
          this.rolesList = res as RoleModel[];
          console.log(this.rolesList);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      })
  }
  getUser = (form:NgForm) => {
    const url ='https://localhost:5001/api/User/'
    this.http.get(url + this.userEmail)
      .subscribe({
        next: (res: any) => {
          this.searchUser = res as UserModel;
          this.isCheckedName = this.searchUser.roles[0].name;
          console.log(this.searchUser);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      })
  }
 
  changeRole(e:any, role:string, userId:string){       
    this.isCheckedName = role;
    const data = {roleName: role, userId: userId};
    const url ='https://localhost:5001/api/User/'
    this.http.post(url, data)
      .subscribe({
        next: (res: any) => {
          console.log(res);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      })
  }
}
