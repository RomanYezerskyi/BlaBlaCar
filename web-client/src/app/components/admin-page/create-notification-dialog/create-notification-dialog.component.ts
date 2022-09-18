import { HttpErrorResponse } from '@angular/common/http';
import { Component, EventEmitter, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormControl, NgForm, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription, timeout } from 'rxjs';
import { NotificationStatus } from 'src/app/enums/notification-status';
import { FeedBackModel } from 'src/app/interfaces/feed-back-model';
import { NotificationsModel } from 'src/app/interfaces/notifications';
import { NotificationsService } from 'src/app/services/notificationsservice/notifications.service';

@Component({
  selector: 'app-create-notification-dialog',
  templateUrl: './create-notification-dialog.component.html',
  styleUrls: ['./create-notification-dialog.component.scss']
})
export class CreateNotificationDialogComponent implements OnInit, OnDestroy {
  textFormControl = new FormControl('', [Validators.required]);
  notificationsSubscription!: Subscription;
  title: string = '';
  description: string = '';
  notification: NotificationsModel = {} as NotificationsModel;
  feedback: FeedBackModel = { rate: 0 } as FeedBackModel;
  onSubmitReason = new EventEmitter();
  notificationStatus: boolean | null = null;
  constructor(
    private dialogRef: MatDialogRef<CreateNotificationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any,
    private notificationsService: NotificationsService) {
    this.notification.notificationStatus = data.notificationStatus;
    this.notification.userId = data.userId;
    this.title = data.title;
    if (data.description) this.description = data.description;
  }

  ngOnInit(): void {
    this.dialogRef.updateSize('45%', '55%');
  }
  ngOnDestroy(): void {
    this.notificationsSubscription.unsubscribe();
  }
  createNotification(form: NgForm) {
    if (!form.valid) return;
    console.log(this.notification);
    if (this.notification.notificationStatus == NotificationStatus.Global || this.notification.notificationStatus == NotificationStatus.SpecificUser) {
      this.notificationsSubscription = this.notificationsService.createNotification(this.notification).subscribe(
        response => {
          console.log(response);
          this.onSubmitReason.emit();
          this.notificationStatus = true;
        },
        (error: HttpErrorResponse) => { console.error(error.error); this.notificationStatus = false; }
      );
    }
    else if (this.notification.notificationStatus == NotificationStatus.FeedBack) {
      if (this.feedback.rate == 0) { return }
      this.feedback.userId = this.notification.userId;
      this.notificationsSubscription = this.notificationsService.addFeedBack(this.feedback).subscribe(
        response => {
          console.log(response);
          this.onSubmitReason.emit();
          this.notificationStatus = true;
          this.closeWithTimeOut();
        },
        (error: HttpErrorResponse) => {
          console.error(error.error);
          this.notificationStatus = false;
          this.closeWithTimeOut();
        }
      );
    }
  }
  rate(rate: number) {
    this.feedback.rate = rate;
  }
  close() {
    this.dialogRef.close();
  }
  closeWithTimeOut() {
    setTimeout(() => this.close(), 2000);
  }
}
