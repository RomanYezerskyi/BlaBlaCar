import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { FeedBackModel } from 'src/app/core/models/notifications-models/feed-back-model';
import { NotificationsModel } from 'src/app/core/models/notifications-models/notifications-model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {
  private baseApiUrl = environment.baseApiUrl;
  userNotifications: NotificationsModel[] = [];
  constructor(private http: HttpClient) {

  }
  getGlobalNotifications(take: number, skip: number): Observable<NotificationsModel[]> {
    const url = this.baseApiUrl + 'Notification/global';
    return this.http.get<NotificationsModel[]>(url, { params: { take: take, skip: skip }, headers: { "ngrok-skip-browser-warning":"any"} });
  }
  getUsersNotifications(take: number, skip: number): Observable<NotificationsModel[]> {
    const url = this.baseApiUrl + 'Notification/last-notifications';
    return this.http.get<NotificationsModel[]>(url, { params: { take: take, skip: skip }, headers: { "ngrok-skip-browser-warning":"any"} });
  }
  createNotification(notification: NotificationsModel): Observable<any> {
    const url = this.baseApiUrl + 'Notification/create';
    return this.http.post(url, notification, {headers: { "ngrok-skip-browser-warning":"any"}})
  }
  //
  getUserNotifications(take: number = 0, skip: number = 0): Observable<NotificationsModel[]> {
    let url = this.baseApiUrl + 'Notification';
    if (take != 0 || skip != 0) {
      return this.http.get<NotificationsModel[]>(url, { params: { take: take, skip: skip }, headers: { "ngrok-skip-browser-warning":"any"} });
    }
    return this.http.get<NotificationsModel[]>(url);
  }
  getUserUnreadNotifications(): Observable<NotificationsModel[]> {
    let url = this.baseApiUrl + 'Notification/unread';
    return this.http.get<NotificationsModel[]>(url , {headers: { "ngrok-skip-browser-warning":"any"}});
  }
  readNotifications(notifications: NotificationsModel[]): Observable<any> {
    const url = this.baseApiUrl + 'Notification/'
    return this.http.put(url, notifications, {headers: { "ngrok-skip-browser-warning":"any"}});
  }
  addFeedBack(feedback: FeedBackModel): Observable<any> {
    const url = this.baseApiUrl + 'Notification/feedback'
    return this.http.post(url, feedback, {headers: { "ngrok-skip-browser-warning":"any"}});
  }
  getUserFeedBacks(take: number = 0, skip: number = 0): Observable<FeedBackModel[]> {
    const url = this.baseApiUrl + 'Notification/user-feedbacks'
    return this.http.get<FeedBackModel[]>(url, { params: { take: take, skip: skip }, headers: { "ngrok-skip-browser-warning":"any"} });
  }
}
