import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Chat } from 'src/app/interfaces/chat-interfaces/chat';
import { RoleModel } from 'src/app/interfaces/role';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { Message } from 'src/app/interfaces/chat-interfaces/message';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  constructor(private http: HttpClient) { }

  getUserChats(): Observable<Chat[]> {
    const url = 'https://localhost:6001/api/Chat/';
    return this.http.get<Chat[]>(url);
  }
  createPrivateChat(userId: string): Observable<string> {
    const url = 'https://localhost:6001/api/Chat/private/' + userId;
    return this.http.post<string>(url, userId);
  }
  getChatById(chatId: string): Observable<Chat> {
    const url = 'https://localhost:6001/api/Chat/chat/' + chatId;
    return this.http.get<Chat>(url);
  }
  createMessage(message: { text: string, chatId: string, user: UserModel }): Observable<any> {
    const url = 'https://localhost:6001/api/Chat/message';
    return this.http.post<any>(url, message);
  }
  readMessagesInchat(messages: Message[]): Observable<any> {
    const url = 'https://localhost:6001/api/Chat/read-messages';
    return this.http.post<any>(url, messages);
  }
}
