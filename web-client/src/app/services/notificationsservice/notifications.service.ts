import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { FeedBackModel } from 'src/app/interfaces/feed-back-model';
import { NotificationsModel } from 'src/app/interfaces/notifications-model';

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {

  constructor(private http: HttpClient) { }
  getGlobalNotifications(take: number, skip: number): Observable<NotificationsModel[]> {
    const url = 'https://localhost:6001/api/Notification/global';
    return this.http.get<NotificationsModel[]>(url, { params: { take: take, skip: skip } });
  }
  getUsersNotifications(take: number, skip: number): Observable<NotificationsModel[]> {
    const url = 'https://localhost:6001/api/Notification/users';
    return this.http.get<NotificationsModel[]>(url, { params: { take: take, skip: skip } });
  }
  createNotification(notification: NotificationsModel): Observable<any> {
    const url = 'https://localhost:6001/api/Notification/create';
    return this.http.post(url, notification)
  }
  //
  getUserNotifications(take: number = 0, skip: number = 0): Observable<NotificationsModel[]> {
    let url = 'https://localhost:6001/api/Notification';
    if (take != 0 || skip != 0) {
      return this.http.get<NotificationsModel[]>(url, { params: { take: take, skip: skip } });
    }
    return this.http.get<NotificationsModel[]>(url);
  }
  getUserUnreadNotifications(): Observable<NotificationsModel[]> {
    let url = 'https://localhost:6001/api/Notification/unread';
    return this.http.get<NotificationsModel[]>(url);
  }
  readNotifications(notifications: NotificationsModel[]): Observable<any> {
    const url = 'https://localhost:6001/api/Notification/'
    return this.http.post(url, notifications)
  }
  addFeedBack(feedback: FeedBackModel): Observable<any> {
    const url = 'https://localhost:6001/api/Notification/feedback'
    return this.http.post(url, feedback)
  }
}
