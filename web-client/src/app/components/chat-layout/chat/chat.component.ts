import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, Subscription, takeUntil } from 'rxjs';
import { ChatModel } from 'src/app/interfaces/chat-interfaces/chat-model';
import { ChatService } from 'src/app/services/chatservice/chat.service';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import { MessageStatus } from 'src/app/enums/message-status';
import { MessageModel } from 'src/app/interfaces/chat-interfaces/message-model';
import { SignalRService } from 'src/app/services/signalr-services/signalr.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
  providers: [SignalRService]
})
export class ChatComponent implements OnInit, OnDestroy {
  @Input() currentUserId: string = '';
  private unsubscribe$: Subject<void> = new Subject<void>();
  chat: ChatModel = {} as ChatModel;
  text: string = '';
  constructor(private route: ActivatedRoute,
    private chatService: ChatService,
    private sanitizeImgService: ImgSanitizerService,
    private router: Router,
    private signal: SignalRService) {
    this.route.queryParams.subscribe(params => {
      if (params['chatId']) {
        this.chat.id = params['chatId'];

      }
    });
    this.setSignalRUrls();
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    console.log(this.chat.id)
    if (this.chat.id != undefined) {
      this.getChat();
      this.connectToSignalRChatHub();
    }
  }
  connectToSignalRChatHub(): void {
    this.signal.getDataStream<MessageModel>().pipe(takeUntil(this.unsubscribe$)).subscribe(message => {
      this.readMessages(message.data);
      message.data.status = MessageStatus.Read;
      this.chat.messages?.push(message.data)
    });
  }
  setSignalRUrls(): void {
    this.signal.setConnectionUrl = "https://localhost:6001/chatHub";
    this.signal.setHubMethod = "joinToChat";
    this.signal.setHubMethodParams = this.chat.id;
    this.signal.setHandlerMethod = "BroadcastChatMessage";
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  getUnreadMessageIndex(): number {
    return this.chat.messages?.findIndex(x => x.status == MessageStatus.Unread)!;
  }
  readMessages(unreadMessage: MessageModel | null): void {
    let messages: MessageModel[] = [];
    if (unreadMessage == null)
      messages = this.chat.messages?.filter(x => x.status == MessageStatus.Unread)!;
    else
      messages!.push(unreadMessage!);
    console.log(messages!);
    if (messages.length == 0) return;
    this.chatService.readMessagesInchat(messages!).pipe(takeUntil(this.unsubscribe$)).subscribe(response => {
      console.log(response);
    },
      (error: HttpErrorResponse) => { console.error(error.error); });
  }
  getChat(): void {
    this.chatService.getChatById(this.chat.id!).pipe(takeUntil(this.unsubscribe$)).subscribe(response => {
      this.chat = response;
      console.log(response);
      this.readMessages(null);
    },
      (error: HttpErrorResponse) => { console.error(error.error); });
  }
  sanitizeImg(img: string): SafeUrl {
    return this.sanitizeImgService.sanitiizeUserImg(img);
  }
  sendMessage(): void {
    if (this.text == '') return;
    const user = this.chat.users?.find(x => x.userId == this.currentUserId)?.user;
    const message = { text: this.text, chatId: this.chat.id!, user: user! };
    console.log(message);
    this.chatService.createMessage(message).pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
}
