import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Subscription } from 'rxjs';
import { Chat } from 'src/app/interfaces/chat-interfaces/chat';
import { UnreadMessagesChats } from 'src/app/interfaces/unread-messages-chats';
import { ChatService } from 'src/app/services/chatservice/chat.service';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import * as signalR from '@microsoft/signalr';
@Component({
  selector: 'app-chat-list',
  templateUrl: './chat-list.component.html',
  styleUrls: ['./chat-list.component.scss']
})
export class ChatListComponent implements OnInit, OnDestroy {
  token = localStorage.getItem("jwt");
  chatsSubscription!: Subscription;
  chats: Array<Chat> = [];
  @Input() currentUserId = '';
  @Input() role = '';
  unreadMessagesChats: Array<UnreadMessagesChats> = [];
  constructor(private chatService: ChatService, private router: Router,
    private sanitizeImgService: ImgSanitizerService, private jwtHelper: JwtHelperService,) { }

  ngOnInit(): void {
    this.getUserChats();
    const currentUserId = this.jwtHelper.decodeToken(this.token!).id;
    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl('https://localhost:6001/' + 'chatHub')
      .build();
    connection.start().then(function () {
      console.log('admin panel SignalR Connected!');
      connection.invoke('JoinToChatMessagesNotifications', currentUserId)
    }).catch(function (err) {
      return console.error(err.toString());
    });
    connection.on("BroadcastMessagesFormAllChats", (chatId) => {
      if (this.unreadMessagesChats.some(x => x.chatId == chatId)) {
        this.unreadMessagesChats.forEach(x => {
          if (x.chatId == chatId) {
            x.count += 1;
          }
        })
      }
      else {
        const chat: UnreadMessagesChats = {
          chatId: chatId,
          count: 1
        };
        this.unreadMessagesChats.push(chat);
      }
    });
  }
  ngOnDestroy(): void {
    this.chatsSubscription.unsubscribe();
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
  goToChat(chatId: string) {
    if (this.role == 'blablacar.user') {
      this.router.navigate(['/chat'], {
        queryParams: {
          chatId: chatId
        }
      });
    }
    else {
      this.router.navigate(['admin/chat'], {
        queryParams: {
          chatId: chatId
        }
      });
    }
  }
  sanitizeImg(img: string): SafeUrl {

    return this.sanitizeImgService.sanitiizeUserImg(img);
  }
  checkIfChatHasUnreadMessages(chatId: string): boolean {
    if (this.unreadMessagesChats.some(x => x.chatId == chatId)) {
      return true;
    }
    return false;
  }
  getUnreadMessagesCount(chatId: string) {
    return this.unreadMessagesChats.find(x => x.chatId == chatId)?.count;
  }
}
