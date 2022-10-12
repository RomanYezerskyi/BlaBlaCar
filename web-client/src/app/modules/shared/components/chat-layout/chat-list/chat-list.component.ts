import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Subject, Subscription, takeUntil } from 'rxjs';
import { MessageStatus } from 'src/app/core/enums/message-status';
import { ChatModel } from 'src/app/core/models/chat-models/chat-model';
import { UnreadMessagesInChatsModel } from 'src/app/core/models/chat-models/unread-messages-in-chats-model';
import { ChatService } from 'src/app/core/services/chat-service/chat.service';
import { ImgSanitizerService } from 'src/app/core/services/image-sanitizer-service/img-sanitizer.service';
import { SignalRService } from 'src/app/core/services/signalr-services/signalr.service';
@Component({
  selector: 'app-chat-list',
  templateUrl: './chat-list.component.html',
  styleUrls: ['./chat-list.component.scss'],
  providers: [SignalRService]
})
export class ChatListComponent implements OnInit, OnDestroy {
  @Input() currentUserId: string = '';
  @Input() role: string = '';
  private unsubscribe$: Subject<void> = new Subject<void>();
  private token: string | null = localStorage.getItem("jwt");
  chats: Array<ChatModel> = [];
  currentChatId: string = '';
  unreadMessagesChats: Array<UnreadMessagesInChatsModel> = [];
  constructor(private chatService: ChatService,
    private router: Router,
    private sanitizeImgService: ImgSanitizerService,
    private jwtHelper: JwtHelperService,
    private signal: SignalRService,
    private route: ActivatedRoute,) {

    this.setSignalRUrls();
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['chatId']) {
        this.currentChatId = params['chatId'];
        this.markChatAsRead(this.currentChatId);
      }
    });
    this.getUserChats();

  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  markChatAsRead(chatId: string): void {
    console.log(chatId);
    this.unreadMessagesChats = this.unreadMessagesChats.filter(x => x.chatId != chatId);
  }
  setSignalRUrls(): void {
    const currentUserId = this.jwtHelper.decodeToken(this.token!).id;
    this.signal.setConnectionUrl = "https://localhost:6001/chatHub";
    this.signal.setHubMethod = "JoinToChatMessagesNotifications";
    this.signal.setHubMethodParams = currentUserId;
    this.signal.setHandlerMethod = "BroadcastMessagesFromChats";
  }

  connectToSignalRChatHub(): void {
    this.signal.getDataStream<string>().pipe(takeUntil(this.unsubscribe$)).subscribe(message => {
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
          const chat: UnreadMessagesInChatsModel = {
            chatId: message.data,
            count: 1
          };
          this.unreadMessagesChats.push(chat);
        }
      }
    });
  }

  getUserChats(): void {
    this.chatService.getUserChats().pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        this.chats = response;
        this.chats.forEach(chat => {
          if (chat.lastMessage?.status == MessageStatus.Unread) {
            const chatMess: UnreadMessagesInChatsModel = {
              chatId: chat.id!,
              count: 1
            };
            this.unreadMessagesChats.push(chatMess);
          }

        });
        console.log(response);
        if (response != null)
          this.connectToSignalRChatHub();
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
  goToChat(chatId: string) {
    console.log(this.router);
    this.router.navigate([this.router.url.split('?')[0]], {
      queryParams: {
        chatId: chatId
      }
    });
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
