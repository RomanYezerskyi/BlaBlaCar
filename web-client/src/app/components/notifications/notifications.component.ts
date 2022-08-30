import { HttpClient, HttpErrorResponse, HttpResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NotificationsModel } from 'src/app/interfaces/notifications';
import { NotificationsService } from 'src/app/services/notificationsservice/notifications.service';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit {
  notifications: NotificationsModel[] = [];
  constructor(private http: HttpClient, private notificationsService: NotificationsService) { }

  async ngOnInit() {
    this.getUserNotifications();
  }
  async getUserNotifications() {
    this.notificationsService.getUserNotifications().pipe().subscribe(
      response => {
        this.notifications = response;
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
