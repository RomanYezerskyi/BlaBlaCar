import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { UserListOrderby } from 'src/app/enums/user-list-orderby';
import { UsersListRequest } from 'src/app/interfaces/admin-interfaces/users-list-request';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { AdminService } from 'src/app/services/admin/admin.service';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';

@Component({
  selector: 'app-top-users-list',
  templateUrl: './top-users-list.component.html',
  styleUrls: ['./top-users-list.component.scss']
})
export class TopUsersListComponent implements OnInit {
  private Skip: number = 0;
  private Take: number = 5;
  public isFullListDisplayed: boolean = false;
  users: Array<UserModel> = [];
  orderBy = UserListOrderby;
  usersReuest: UsersListRequest = {
    orderBy: UserListOrderby.Trips,
    skip: 0,
    take: 5
  };
  constructor(private adminService: AdminService, private imgSanitizer: ImgSanitizerService) { }

  ngOnInit(): void {
    this.getUsers();
  }
  sanitizeImg(img: string): SafeUrl {
    return this.imgSanitizer.sanitiizeUserImg(img);
  }
  usersOrderBy = (orderBy: UserListOrderby) => {
    this.usersReuest.orderBy = orderBy;
    this.usersReuest.skip = this.Skip;
    this.usersReuest.take = this.Take;
    this.users = [];
    this.getUsers();
  }
  getUsers() {
    if (this.usersReuest.skip <= this.users.length) {
      this.adminService.getTopListUsers(this.usersReuest).subscribe(
        response => {
          this.users = this.users.concat(response);
        },
        (error: HttpErrorResponse) => { console.error(error.error); }
      );
      this.usersReuest.skip += this.usersReuest.take;
    }
    else this.isFullListDisplayed = true;
  }

}
