import { HttpClient, HttpErrorResponse, HttpResponse, HttpStatusCode } from '@angular/common/http';
import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { NotificationsModel } from 'src/app/interfaces/notifications-model';
import { NotificationsService } from 'src/app/services/notificationsservice/notifications.service';
import * as signalR from '@microsoft/signalr';
import { JwtHelperService } from '@auth0/angular-jwt';
import { SignalRService } from 'src/app/services/signalr-services/signalr.service';
import { NotificationStatus } from 'src/app/enums/notification-status';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { CreateNotificationDialogComponent } from '../create-notification-dialog/create-notification-dialog.component';
import { Subject, takeUntil } from 'rxjs';
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
    this.signal.setConnectionUrl = "https://localhost:6001/notify";
    this.signal.setHubMethod = 'JoinToNotificationsHub';
    this.signal.setHubMethodParams = this.currentUserId;
    this.signal.setHandlerMethod = "BroadcastNotification";
  }
  connectToNotificationsSignalRHub() {
    this.signal.getDataStream<any>().pipe(takeUntil(this.unsubscribe$)).subscribe(message => {
      console.log(message.data);
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
        this.notifications = response;
        this.checkIfNotRead();
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }

  loadMore(): void {
    if (this.Skip <= this.notifications.length) {
      this.notificationsService.getUserNotifications(this.Take, this.Skip).pipe(takeUntil(this.unsubscribe$)).subscribe(
        response => {
          this.notifications = response;
          this.checkIfNotRead();
          console.log(response);
        },
        (error: HttpErrorResponse) => { console.error(error.error); }
      );
    }
    this.Skip += this.Take;
  }
  readNotifications(): void {
    this.notificationsService.readNotifications(this.notifications).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        console.log(response);
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
}
