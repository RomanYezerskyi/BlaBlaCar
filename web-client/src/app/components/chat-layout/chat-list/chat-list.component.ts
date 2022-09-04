import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { Chat } from 'src/app/interfaces/chat-interfaces/chat';
import { ChatService } from 'src/app/services/chatservice/chat.service';

@Component({
  selector: 'app-chat-list',
  templateUrl: './chat-list.component.html',
  styleUrls: ['./chat-list.component.scss']
})
export class ChatListComponent implements OnInit {
  chatsSubscription!: Subscription;
  chats: Array<Chat> = [];
  constructor(private chatService: ChatService) { }

  ngOnInit(): void {
    this.getUserChats();
  }
  getUserChats() {
    this.chatsSubscription = this.chatService.getUserChats().subscribe(
      response => {
        this.chats = response;
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }

}
