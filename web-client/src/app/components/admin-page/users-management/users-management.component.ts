import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { Subject, Subscription } from 'rxjs';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import { UserService } from 'src/app/services/userservice/user.service';
import { UserRequestInfoComponent } from '../user-request-info/user-request-info.component';
import { UsersRequestsComponent } from '../users-requests/users-requests.component';

@Component({
  selector: 'app-users-management',
  templateUrl: './users-management.component.html',
  styleUrls: ['./users-management.component.scss']
})
export class UsersManagementComponent implements OnInit, OnDestroy {
  users: Array<UserModel> = [];
  userId = '';
  userData = '';
  usersSubscription!: Subscription;

  constructor(private userService: UserService, private sanitizeImgService: ImgSanitizerService,) { }

  ngOnInit(): void {
  }
  ngOnDestroy(): void {
    this.usersSubscription.unsubscribe();
  }
  sanitizeImg(img: string): SafeUrl {
    return this.sanitizeImgService.sanitiizeUserImg(img);
  }

  searchUsers() {
    if (this.userData == '') return;
    this.usersSubscription = this.userService.searchUsers(this.userData).subscribe(
      response => {
        console.log(response);
        this.users = response;
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }


  getUser(userId: string) {
    this.userId = userId;
  }
}
