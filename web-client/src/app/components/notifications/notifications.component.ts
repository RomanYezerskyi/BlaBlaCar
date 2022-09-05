import { HttpClient, HttpErrorResponse, HttpResponse, HttpStatusCode } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { NotificationsModel } from 'src/app/interfaces/notifications';
import { NotificationsService } from 'src/app/services/notificationsservice/notifications.service';
import * as signalR from '@microsoft/signalr';
import { JwtHelperService } from '@auth0/angular-jwt';
@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit {
  notifications: NotificationsModel[] = [];
  token = localStorage.getItem("jwt");
  @Output() notReadedNotifiEvent = new EventEmitter<number>();
  constructor(private http: HttpClient, private notificationsService: NotificationsService, private jwtHelper: JwtHelperService) { }

  async ngOnInit() {
    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl('https://localhost:6001/' + 'notify')
      .build();

    const userId = this.jwtHelper.decodeToken(this.token!).id
    connection.start().then(function () {
      console.log('SignalR Connected!');
      connection.invoke('getConnectionId', userId)
        .then(function (connectionId) {
          console.log("connectionID: " + connectionId);
          // $("#signalRconnectionId").attr("value", connectionId);
        });
    }).catch(function (err) {
      return console.error(err.toString());
    });
    connection.on("BroadcastNotification", () => {
      console.log("Hi man!!!");
      this.getUserNotifications();
    });
    this.getUserNotifications();
  }
  checkIfNotRead() {
    console.log("c");
    let notifi = this.notifications.filter(x => x.readNotificationStatus == 2).length;
    if (notifi != 0) {
      console.log(notifi);
      this.notReadedNotifiEvent.emit(notifi);
    }
  }
  getUserNotifications() {

    this.notificationsService.getUserNotifications().pipe().subscribe(
      response => {
        this.notifications = response;
        this.checkIfNotRead();
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
    // const url = 'https://localhost:6001/api/Notification/'
    // const notifications = await new Promise<NotificationsModel[]>((resolve, reject) => {
    //   this.http.get<NotificationsModel[]>(url)
    //     .subscribe({
    //       next: (res: NotificationsModel[]) => {
    //         resolve(res);
    //       },
    //       error: (err: HttpErrorResponse) => { reject(err), console.error(err) }
    //     });
    // });
    // return notifications;
  }

  async readNotifications() {
    this.notificationsService.readNotifications(this.notifications).pipe().subscribe(
      response => {
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
    // const url = 'https://localhost:6001/api/Notification/'
    // const notifications = await new Promise((resolve, reject) => {
    //   this.http.post(url, this.notifications)
    //     .subscribe({
    //       next: (res) => {
    //         resolve(res);
    //         console.log(res);
    //       },
    //       error: (err: HttpErrorResponse) => { reject(err), console.error(err) }
    //     });
    // });
    // return notifications;
  }
}
