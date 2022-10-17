import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SafeUrl } from '@angular/platform-browser';
import { Subject, Subscription, takeUntil } from 'rxjs';
import { UserModel } from 'src/app/core/models/user-models/user-model';
import { ChatService } from 'src/app/core/services/chat-service/chat.service';
import { ImgSanitizerService } from 'src/app/core/services/image-sanitizer-service/img-sanitizer.service';
import { UserService } from 'src/app/core/services/user-service/user.service';
import { UserRequestInfoComponent } from '../user-request-info/user-request-info.component';
import { UsersRequestsComponent } from '../users-requests/users-requests.component';

@Component({
  selector: 'app-users-management',
  templateUrl: './users-management.component.html',
  styleUrls: ['./users-management.component.scss']
})
export class UsersManagementComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  users: Array<UserModel> = [];
  userId: string = '';
  userData: string = '';
  constructor(private userService: UserService, private sanitizeImgService: ImgSanitizerService, private _snackBar: MatSnackBar,) { }

  ngOnInit(): void {
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  sanitizeImg(img: string): SafeUrl {
    return this.sanitizeImgService.sanitiizeUserImg(img);
  }

  searchUsers(): void {
    if (this.userData == '') return;
    this.userService.searchUsers(this.userData).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        if (response != null) {
          this.users = response;
        }
        else {
          this.openSnackBar("No users found!");
          this.users = [];
        }

      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
  getUser(userId: string): void {
    this.userId = userId;
  }
  private openSnackBar(message: string) {
    this._snackBar.open(message, "Close");
  }
}
