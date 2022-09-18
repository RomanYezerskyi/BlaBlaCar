import { HttpErrorResponse } from '@angular/common/http';
import { Component, EventEmitter, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormControl, NgForm, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject, Subscription, takeUntil, timeout } from 'rxjs';
import { NotificationStatus } from 'src/app/enums/notification-status';
import { FeedBackModel } from 'src/app/interfaces/feed-back-model';
import { NotificationsModel } from 'src/app/interfaces/notifications-model';
import { NotificationsService } from 'src/app/services/notificationsservice/notifications.service';

@Component({
  selector: 'app-create-notification-dialog',
  templateUrl: './create-notification-dialog.component.html',
  styleUrls: ['./create-notification-dialog.component.scss']
})
export class CreateNotificationDialogComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  textFormControl = new FormControl('', [Validators.required]);
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
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  createNotification(form: NgForm): void {
    if (!form.valid) return;
    console.log(this.notification);
    if (this.notification.notificationStatus == NotificationStatus.Global || this.notification.notificationStatus == NotificationStatus.SpecificUser) {
      this.notificationsService.createNotification(this.notification).pipe(takeUntil(this.unsubscribe$)).subscribe(
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
      this.notificationsService.addFeedBack(this.feedback).pipe(takeUntil(this.unsubscribe$)).subscribe(
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
  rate(rate: number): void {
    this.feedback.rate = rate;
  }
  close(): void {
    this.dialogRef.close();
  }
  closeWithTimeOut(): void {
    setTimeout(() => this.close(), 2000);
  }
}
