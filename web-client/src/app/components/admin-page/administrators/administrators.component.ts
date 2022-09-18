import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Subject, Subscription, takeUntil } from 'rxjs';
import { RoleModel } from 'src/app/interfaces/role-model';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { UserStatus } from 'src/app/interfaces/user-interfaces/user-status';
import { AdminService } from 'src/app/services/admin/admin.service';
import { ChatService } from 'src/app/services/chatservice/chat.service';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';

@Component({
  selector: 'app-administrators',
  templateUrl: './administrators.component.html',
  styleUrls: ['./administrators.component.scss']
})
export class AdministratorsComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  admins: Array<UserModel> = [];
  rolesList: Array<RoleModel> = [];
  userEmail: string = '';
  searchUser: UserModel = {} as UserModel;
  isCheckedName: string = '';
  constructor(private sanitizeImgService: ImgSanitizerService,
    private adminService: AdminService, private chatService: ChatService, private router: Router,) { }

  ngOnInit(): void {
    this.getAdmins();
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  sanitizeImg(img: string): SafeUrl {
    return this.sanitizeImgService.sanitiizeUserImg(img);
  }

  getAdmins() {
    this.adminService.getAdmins().pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        console.log(response);
        this.admins = response;
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
  getUser = (form: NgForm) => {
    if (this.userEmail == '') return;
    this.adminService.getUserByEmail(this.userEmail).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.searchUser = response;
        this.isCheckedName = this.searchUser.roles[0].name;
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
  getChat(userId: string) {
    this.chatService.GetPrivateChat(userId).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.router.navigate(['/chat'], {
          queryParams: {
            chatId: response
          }
        });
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
}
