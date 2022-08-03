import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { RoleModel } from 'src/app/interfaces/role';
import { UserModel } from 'src/app/interfaces/user-model';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss']
})
export class RolesComponent implements OnInit {

  rolesList: Array<RoleModel> = [];
  userEmail = '';
  searchUser:UserModel = { id:'', email:'',firstName:'',phoneNumber:'', roles:[]};
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
          console.log(this.searchUser);
        },
        error: (err: HttpErrorResponse) => console.error(err),
      })
  }
  changeRole = (checkBox:any, roleName:string, userId:string) => {
    if(checkBox.checked == false){
      console.log("aaaaaa");
      return;
    }
    const data = {roleName: roleName, userId: userId};
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
