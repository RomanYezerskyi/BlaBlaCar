import { HttpClient, HttpErrorResponse, HttpResponse, HttpStatusCode } from '@angular/common/http';
import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { NotificationsModel } from 'src/app/core/models/notifications-models/notifications-model';
import { NotificationsService } from 'src/app/core/services/notifications-service/notifications.service';
import * as signalR from '@microsoft/signalr';
import { JwtHelperService } from '@auth0/angular-jwt';
import { SignalRService } from 'src/app/core/services/signalr-services/signalr.service';
import { NotificationStatus } from 'src/app/core/enums/notification-status';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { CreateNotificationDialogComponent } from '../create-notification-dialog/create-notification-dialog.component';
import { skip, Subject, takeUntil } from 'rxjs';
import { environment } from 'src/environments/environment';
import { MatSnackBar } from '@angular/material/snack-bar';
@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss'],
  providers: [SignalRService]
})
export class NotificationsComponent implements OnInit, OnDestroy {
  @Output() notReadedNotifiEvent = new EventEmitter<number>();
  private unsubscribe$: Subject<void> = new Subject<void>();
  notifications: NotificationsModel[] = [];
  notificationStatus = NotificationStatus;
  private Skip: number = 0;
  private Take: number = 5;
  private token: string | null = localStorage.getItem("jwt");
  private currentUserId: string = this.jwtHelper.decodeToken(this.token!).id;

  constructor(
    private _snackBar: MatSnackBar,
    private dialog: MatDialog,
    private notificationsService: NotificationsService,
    private jwtHelper: JwtHelperService,
    private signal: SignalRService) {
    this.setSignalRConnectionUrls();
  }

  ngOnInit(): void {
    this.connectToNotificationsSignalRHub();
    this.getUserNotifications();
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  setSignalRConnectionUrls() {
    this.signal.setConnectionUrl = environment.notificationsHubConnectionUrl;
    this.signal.setHubMethod = environment.notificationsHubMethod;
    this.signal.setHubMethodParams = this.currentUserId;
    this.signal.setHandlerMethod = environment.notificationsHubHandlerMethod;
  }
  connectToNotificationsSignalRHub() {
    this.signal.getDataStream<any>().pipe(takeUntil(this.unsubscribe$)).subscribe(message => {
      this.getUserNotifications();
    });
  }
  checkIfNotRead(): void {
    let notifi = this.notifications.filter(x => x.readNotificationStatus == 2).length;
    if (notifi != 0) {
      this.notReadedNotifiEvent.emit(notifi);
    }
  }
  getUserNotifications(): void {
    this.notificationsService.getUserUnreadNotifications().pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        if (response != null) {
          this.notifications = response;
          this.Skip = this.notifications.length;
          this.checkIfNotRead();
        }
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }

  loadMore(): void {
    if (this.Skip <= this.notifications.length) {
      this.notificationsService.getUserNotifications(this.Take, this.Skip).pipe(takeUntil(this.unsubscribe$)).subscribe(
        response => {
          if (response != null) {
            this.notifications = this.notifications.concat(response);
            this.checkIfNotRead();
          }
        },
        (error: HttpErrorResponse) => { console.error(error.error); }
      );
    }
    this.Skip += this.Take;
  }
  readNotifications(): void {
    let unreadNotififation = this.notifications.filter(x => x.readNotificationStatus != 1);
    this.notificationsService.readNotifications(unreadNotififation).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.notifications.forEach(x => x.readNotificationStatus = 1);
        this.notReadedNotifiEvent.emit(0);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
  AddFeedBackDialog(userId: string | null, description: string): void {
    const dialogConfig = new MatDialogConfig();
    // dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.data = {
      notificationStatus: NotificationStatus.FeedBack,
      userId: userId,
      title: "Feedback",
      description: description,
    };
    const dRef = this.dialog.open(CreateNotificationDialogComponent, dialogConfig);
    // dRef.componentInstance.onSubmitReason.subscribe(() => {
    //   this.changeUserStatus(this.userStatus.NeedMoreData);
    // });
  }
  private openSnackBar(message: string) {
    this._snackBar.open(message, "Close");
  }
}
