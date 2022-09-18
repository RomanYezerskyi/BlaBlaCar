import { HttpClient, HttpErrorResponse, HttpResponse, HttpStatusCode } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { NotificationsModel } from 'src/app/interfaces/notifications';
import { NotificationsService } from 'src/app/services/notificationsservice/notifications.service';
import * as signalR from '@microsoft/signalr';
import { JwtHelperService } from '@auth0/angular-jwt';
import { SignalRService } from 'src/app/services/signalr-services/signalr.service';
import { NotificationStatus } from 'src/app/enums/notification-status';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { CreateNotificationDialogComponent } from '../admin-page/create-notification-dialog/create-notification-dialog.component';
@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss'],
  providers: [SignalRService]
})
export class NotificationsComponent implements OnInit {
  @Output() notReadedNotifiEvent = new EventEmitter<number>();
  notifications: NotificationsModel[] = [];
  notificationStatus = NotificationStatus;
  private Skip: number = 0;
  private Take: number = 5;
  public isFullListDisplayed: boolean = false;
  private token = localStorage.getItem("jwt");
  private currentUserId: string = this.jwtHelper.decodeToken(this.token!).id;
  constructor(
    private dialog: MatDialog,
    private notificationsService: NotificationsService,
    private jwtHelper: JwtHelperService,
    private signal: SignalRService) {
    this.signal.url = "https://localhost:6001/notify";
    this.signal.hubMethod = 'JoinToNotificationsHub';
    this.signal.hubMethodParams = this.currentUserId;
    this.signal.handlerMethod = "BroadcastNotification";
  }

  ngOnInit(): void {
    this.signal.getDataStream<any>().subscribe(message => {
      console.log(message.data);
      this.getUserNotifications();
    });
    this.getUserNotifications();
  }
  checkIfNotRead(): void {
    let notifi = this.notifications.filter(x => x.readNotificationStatus == 2).length;
    if (notifi != 0) {
      this.notReadedNotifiEvent.emit(notifi);
    }
  }
  getUserNotifications(): void {
    this.notificationsService.getUserUnreadNotifications().pipe().subscribe(
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
      this.notificationsService.getUserNotifications(this.Take, this.Skip).pipe().subscribe(
        response => {
          this.notifications = response;
          this.checkIfNotRead();
          console.log(response);
        },
        (error: HttpErrorResponse) => { console.error(error.error); }
      );
    }
    // else {
    //   this.isFullListDisplayed = true;
    // }
    this.Skip += this.Take;
  }
  readNotifications(): void {
    this.notificationsService.readNotifications(this.notifications).pipe().subscribe(
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
