import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Subscription } from 'rxjs';
import { Chat } from 'src/app/interfaces/chat-interfaces/chat';
import { UnreadMessagesChats } from 'src/app/interfaces/chat-interfaces/unread-messages-chats';
import { ChatService } from 'src/app/services/chatservice/chat.service';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import { SignalRService } from 'src/app/services/signalr-services/signalr.service';
@Component({
  selector: 'app-chat-list',
  templateUrl: './chat-list.component.html',
  styleUrls: ['./chat-list.component.scss'],
  providers: [SignalRService]
})
export class ChatListComponent implements OnInit, OnDestroy {
  token = localStorage.getItem("jwt");
  chatsSubscription!: Subscription;
  chats: Array<Chat> = [];
  currentChatId: string = '';
  @Input() currentUserId = '';
  @Input() role = '';
  unreadMessagesChats: Array<UnreadMessagesChats> = [];
  constructor(private chatService: ChatService,
    private router: Router,
    private sanitizeImgService: ImgSanitizerService,
    private jwtHelper: JwtHelperService,
    private signal: SignalRService,
    private route: ActivatedRoute,) {

    const currentUserId = this.jwtHelper.decodeToken(this.token!).id;
    this.signal.url = "https://localhost:6001/chatHub";
    this.signal.hubMethod = "JoinToChatMessagesNotifications";
    this.signal.hubMethodParams = currentUserId;
    this.signal.handlerMethod = "BroadcastMessagesFromChats";
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['chatId']) {
        this.currentChatId = params['chatId'];
      }
    });
    this.getUserChats();
    this.signal.getDataStream<string>().subscribe(message => {
      console.log(message.data);
      if (this.currentChatId != message.data) {
        if (this.unreadMessagesChats.some(x => x.chatId == message.data)) {
          this.unreadMessagesChats.forEach(x => {
            if (x.chatId == message.data) {
              x.count += 1;
            }
          })
        }
        else {
          const chat: UnreadMessagesChats = {
            chatId: message.data,
            count: 1
          };
          this.unreadMessagesChats.push(chat);
        }
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
