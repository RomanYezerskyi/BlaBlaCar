import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { NotificationsModel } from 'src/app/interfaces/notifications';

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {

  constructor(private http: HttpClient) { }
  getUserNotifications(): Observable<NotificationsModel[]> {
    const url = 'https://localhost:6001/api/Notification/';
    return this.http.get<NotificationsModel[]>(url);
  }
  readNotifications(notifications: NotificationsModel[]): Observable<any> {
    const url = 'https://localhost:6001/api/Notification/'
    return this.http.post(url, notifications)
  }
}
