import { NoopScrollStrategy } from '@angular/cdk/overlay';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { skip, Subject, Subscription, takeUntil } from 'rxjs';
import { CreateNotificationDialogComponent } from 'src/app/components/create-notification-dialog/create-notification-dialog.component';
import { NotificationStatus } from 'src/app/enums/notification-status';
import { NotificationsModel } from 'src/app/interfaces/notifications-model';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import { NotificationsService } from 'src/app/services/notificationsservice/notifications.service';

@Component({
  selector: 'app-admin-notifications',
  templateUrl: './admin-notifications.component.html',
  styleUrls: ['./admin-notifications.component.scss']
})
export class AdminNotificationsComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  globalNotificationsProp = {
    skip: 0,
    take: 5,
    isFullListDisplayed: false
  };
  globalNotifications: Array<NotificationsModel> = [];
  usersNotificationsProp = {
    skip: 0,
    take: 5,
    isFullListDisplayed: false
  };
  usersNotifications: Array<NotificationsModel> = [];
  notifiStatus = NotificationStatus;
  constructor(private notificationsService: NotificationsService, private router: Router,
    private imgSanitizer: ImgSanitizerService, private dialog: MatDialog) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }
  ngOnInit(): void {
    this.getGlobalNotitifications();
    this.getUsersNotitifications();
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  addGlobalNotitfication(): void {
    this.openNotificationDialog(NotificationStatus.Global, null);
  }
  private openNotificationDialog(notificationStatus: NotificationStatus, userId: string | null): void {
    const dialogConfig = new MatDialogConfig();
    // dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    // dialogConfig.autoFocus = true;
    dialogConfig.scrollStrategy = new NoopScrollStrategy()
    dialogConfig.data = {
      notificationStatus: notificationStatus,
      userId: userId,
      title: "Notification"
    };
    const dRef = this.dialog.open(CreateNotificationDialogComponent, dialogConfig);
    dRef.componentInstance.onSubmitReason.pipe(takeUntil(this.unsubscribe$)).subscribe(() => {
      this.globalNotifications = [];
      this.globalNotificationsProp.skip = 0;
      this.globalNotificationsProp.take = 5;
      this.usersNotificationsProp.skip = 0;
      this.usersNotificationsProp.take = 5
      this.getGlobalNotitifications();
      this.getUsersNotitifications();
    });

  }
  sanitizeImg(img: string): SafeUrl {
    return this.imgSanitizer.sanitiizeUserImg(img);
  }
  getGlobalNotitifications() {
    if (this.globalNotificationsProp.skip <= this.globalNotifications.length) {
      this.notificationsService.getGlobalNotifications(
        this.globalNotificationsProp.take,
        this.globalNotificationsProp.skip).pipe(takeUntil(this.unsubscribe$)).subscribe(
          response => {
            this.globalNotifications = this.globalNotifications.concat(response);
          },
          (error: HttpErrorResponse) => { console.error(error.error); }
        );
      this.globalNotificationsProp.skip += this.globalNotificationsProp.take;
    }
    else this.globalNotificationsProp.isFullListDisplayed = true;
  }
  getUsersNotitifications() {
    if (this.usersNotificationsProp.skip <= this.usersNotifications.length) {
      this.notificationsService.getUsersNotifications(
        this.usersNotificationsProp.take,
        this.usersNotificationsProp.skip).pipe(takeUntil(this.unsubscribe$)).subscribe(
          response => {
            this.usersNotifications = this.usersNotifications.concat(response);
          },
          (error: HttpErrorResponse) => { console.error(error.error); }
        );
      this.usersNotificationsProp.skip += this.usersNotificationsProp.take;
    }
    else this.usersNotificationsProp.isFullListDisplayed = true;
  }
}
