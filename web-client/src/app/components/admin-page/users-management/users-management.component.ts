import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { Subscription } from 'rxjs';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import { UserService } from 'src/app/services/userservice/user.service';

@Component({
  selector: 'app-users-management',
  templateUrl: './users-management.component.html',
  styleUrls: ['./users-management.component.scss']
})
export class UsersManagementComponent implements OnInit {
  users: Array<UserModel> = [];
  selectedUser: UserModel = {
    id: '', cars: [], email: '', firstName: '', phoneNumber: '', roles: [], userDocuments: [], userStatus: -1,
  };
  userData = '';
  usersSubscription!: Subscription;
  constructor(private userService: UserService, private sanitizeImgService: ImgSanitizerService) { }

  ngOnInit(): void {
  }
  sanitizeImg(img: string): SafeUrl {
    return this.sanitizeImgService.sanitiizeUserImg(img);
  }

  searchUsers() {
    if (this.userData == '') return;
    this.usersSubscription = this.userService.searchUser(this.userData).subscribe(
      response => {
        console.log(response);
        this.users = response;
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
  getUser(userId: string) {
    this.selectedUser = this.users[0]
  }
}
