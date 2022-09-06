import { HttpErrorResponse } from '@angular/common/http';
import { Component, EventEmitter, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormControl, NgForm, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { NotificationStatus } from 'src/app/enums/notification-status';
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
  title = '';
  notification: NotificationsModel = {
    text: '',

  };
  onSubmitReason = new EventEmitter();
  constructor(private dialogRef: MatDialogRef<CreateNotificationDialogComponent>, @Inject(MAT_DIALOG_DATA) data: any, private notificationsService: NotificationsService) {
    this.notification.notificationStatus = data.notificationStatus;
    this.notification.userId = data.userId;
    this.title = data.title;
  }

  ngOnInit(): void {
    this.dialogRef.updateSize('45%', '55%');
  }
  ngOnDestroy(): void {
    this.notificationsSubscription.unsubscribe();
  }
  createNotification(form: NgForm) {
    if (!form.valid) alert("no");
    this.notificationsSubscription = this.notificationsService.createNotification(this.notification).subscribe(
      response => {
        console.log(response);
        this.onSubmitReason.emit();
        // this.dialogRef.close();
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
  close() {
    this.dialogRef.close();
  }
}
