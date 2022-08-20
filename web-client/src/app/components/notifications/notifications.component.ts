import { HttpClient, HttpErrorResponse, HttpResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NotificationsModel } from 'src/app/interfaces/notifications';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit {
  notification: NotificationsModel[] = [];
  constructor(private http: HttpClient,) { }

  async ngOnInit() {
    this.notification = await this.getUserNotifications();
  }
  private async getUserNotifications(): Promise<NotificationsModel[]> {
    const url = 'https://localhost:6001/api/Notification/'
    const notifications = await new Promise<NotificationsModel[]>((resolve, reject) => {
      this.http.get<NotificationsModel[]>(url)
        .subscribe({
          next: (res: NotificationsModel[]) => {
            resolve(res);
          },
          error: (err: HttpErrorResponse) => { reject(err), console.error(err) }
        });
    });
    return notifications;
  }

  async readNotifications() {
    const url = 'https://localhost:6001/api/Notification/'
    const notifications = await new Promise((resolve, reject) => {
      this.http.post(url, this.notification)
        .subscribe({
          next: (res) => {
            resolve(res);
            console.log(res);
          },
          error: (err: HttpErrorResponse) => { reject(err), console.error(err) }
        });
    });
    return notifications;
  }
}
