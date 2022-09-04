import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Chat } from 'src/app/interfaces/chat-interfaces/chat';
import { RoleModel } from 'src/app/interfaces/role';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  constructor(private http: HttpClient) { }

  getUserChats(): Observable<Chat[]> {
    const url = 'https://localhost:6001/api/Chat/';
    return this.http.get<Chat[]>(url);
  }
  createPrivateChat(userId: string) {
    const url = 'https://localhost:6001/api/Chat/private/' + userId;

    return this.http.post<any>(url, userId);
  }
}
