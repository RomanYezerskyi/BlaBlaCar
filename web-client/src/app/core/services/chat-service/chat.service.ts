import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ChatModel } from 'src/app/core/models/chat-models/chat-model';
import { UserModel } from 'src/app/core/models/user-models/user-model';
import { MessageModel } from 'src/app/core/models/chat-models/message-model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private baseApiUrl = environment.baseApiUrl;
  constructor(private http: HttpClient) { }
  getUserChats(): Observable<ChatModel[]> {
    const url = this.baseApiUrl + 'Chat/';
    return this.http.get<ChatModel[]>(url, {headers: { "ngrok-skip-browser-warning":"any"}});
  }
  GetPrivateChat(userId: string): Observable<string> {
    const url = this.baseApiUrl + 'Chat/private/' + userId;
    return this.http.get<string>(url, {headers: { "ngrok-skip-browser-warning":"any"}});
  }
  getChatById(chatId: string): Observable<ChatModel> {
    const url = this.baseApiUrl + 'Chat/' + chatId;
    return this.http.get<ChatModel>(url, {headers: { "ngrok-skip-browser-warning":"any"}});
  }
  getChatMessages(chatId: string, take: number, skip: number): Observable<MessageModel[]> {

    const url = this.baseApiUrl + 'Chat/chat-messages/' + chatId;
    return this.http.get<MessageModel[]>(url, { params: { take: take, skip: skip }, headers: { "ngrok-skip-browser-warning":"any"} });
  }
  createMessage(message: { text: string, chatId: string, user: UserModel }): Observable<any> {
    const url = this.baseApiUrl + 'Chat/message';
    return this.http.post<any>(url, message, {headers: { "ngrok-skip-browser-warning":"any"}});
  }
  readMessagesInchat(messages: MessageModel[]): Observable<any> {
    const url = this.baseApiUrl + 'Chat/read-messages';
    return this.http.put<any>(url, messages, {headers: { "ngrok-skip-browser-warning":"any"}});
  }
  isUreadMessages(): Observable<number> {
    const url = this.baseApiUrl + 'Chat/unread-messages';
    return this.http.get<number>(url, {headers: { "ngrok-skip-browser-warning":"any"}});
  }
}
