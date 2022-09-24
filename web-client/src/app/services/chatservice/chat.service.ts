import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ChatModel } from 'src/app/interfaces/chat-interfaces/chat-model';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { MessageModel } from 'src/app/interfaces/chat-interfaces/message-model';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  constructor(private http: HttpClient) { }

  getUserChats(): Observable<ChatModel[]> {
    const url = 'https://localhost:6001/api/Chat/';
    return this.http.get<ChatModel[]>(url);
  }
  GetPrivateChat(userId: string): Observable<string> {
    const url = 'https://localhost:6001/api/Chat/private/' + userId;
    return this.http.get<string>(url);
  }
  getChatById(chatId: string): Observable<ChatModel> {
    const url = 'https://localhost:6001/api/Chat/' + chatId;
    return this.http.get<ChatModel>(url);
  }
  getChatMessages(chatId: string, take: number, skip: number): Observable<MessageModel[]> {
    const url = 'https://localhost:6001/api/Chat/chat-messages/' + chatId;
    return this.http.get<MessageModel[]>(url, { params: { take: take, skip: skip } });
  }
  createMessage(message: { text: string, chatId: string, user: UserModel }): Observable<any> {
    const url = 'https://localhost:6001/api/Chat/message';
    return this.http.post<any>(url, message);
  }
  readMessagesInchat(messages: MessageModel[]): Observable<any> {
    const url = 'https://localhost:6001/api/Chat/read-messages';
    return this.http.put<any>(url, messages);
  }
  isUreadMessages(): Observable<boolean> {
    const url = 'https://localhost:6001/api/Chat/unread-messages';
    return this.http.get<boolean>(url);
  }
}
