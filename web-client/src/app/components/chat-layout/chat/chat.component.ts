import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, Subscription, takeUntil } from 'rxjs';
import { Chat } from 'src/app/interfaces/chat-interfaces/chat';
import { ChatService } from 'src/app/services/chatservice/chat.service';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import { MessageStatus } from 'src/app/enums/message-status';
import { Message } from 'src/app/interfaces/chat-interfaces/message';
import { SignalRService } from 'src/app/services/signalr-services/signalr.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
  providers: [SignalRService]
})
export class ChatComponent implements OnInit, OnDestroy {
  @Input() currentUserId = '';

  chatSubscription!: Subscription;
  messageSubscription!: Subscription;
  chat: Chat = {} as Chat;
  text = '';

  private unsubscribe$: Subject<void> = new Subject<void>();

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
    this.signal.url = "https://localhost:6001/chatHub";
    this.signal.hubMethod = "joinToChat";
    this.signal.hubMethodParams = this.chat.id;
    this.signal.handlerMethod = "BroadcastChatMessage";
    // this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {

    if (this.chat.id != '') {
      this.getChat();

      this.signal.getDataStream<Message>().subscribe(message => {
        // console.log(message.data);
        this.chat.messages?.push(message.data)
      });

    }
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();

    if (this.chatSubscription != undefined)
      this.chatSubscription.unsubscribe();
  }

  getUnreadMessageIndex(): number {
    return this.chat.messages?.findIndex(x => x.status == MessageStatus.Unread)!;
  }
  readMessages(): void {
    let messages = this.chat.messages?.filter(x => x.status == MessageStatus.Unread);
    console.log(messages);
    this.chatService.readMessagesInchat(messages!).pipe(takeUntil(this.unsubscribe$)).subscribe(response => {
      console.log(response);
    },
      (error: HttpErrorResponse) => { console.error(error.error); });
  }
  getChat(): void {
    this.chatService.getChatById(this.chat.id!).pipe(takeUntil(this.unsubscribe$)).subscribe(response => {
      this.chat = response;
      console.log(response);
      this.readMessages();
    },
      (error: HttpErrorResponse) => { console.error(error.error); });
  }
  sanitizeImg(img: string): SafeUrl {
    return this.sanitizeImgService.sanitiizeUserImg(img);
  }
  sendMessage() {
    if (this.text == '') return;
    const user = this.chat.users?.find(x => x.userId == this.currentUserId)?.user;
    const message = { text: this.text, chatId: this.chat.id!, user: user! };
    console.log(message);
    this.messageSubscription = this.chatService.createMessage(message).subscribe(
      response => {
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
}
