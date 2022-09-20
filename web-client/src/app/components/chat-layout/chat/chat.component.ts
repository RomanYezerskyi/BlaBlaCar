import { HttpErrorResponse } from '@angular/common/http';
import { AfterViewChecked, AfterViewInit, Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { skip, Subject, Subscription, takeUntil } from 'rxjs';
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
export class ChatComponent implements OnInit, OnDestroy, AfterViewChecked {
  @Input() currentUserId: string = '';
  @ViewChild('scrollMe') private myScrollContainer!: ElementRef;
  disableScrollDown: boolean = false;
  private unsubscribe$: Subject<void> = new Subject<void>();
  chat: ChatModel = { messages: [], users: [] = [] };
  text: string = '';
  private messagesSkip: number = 0;
  private messagesTake: number = 10;
  isFullListDisplayed: boolean = false;
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

    if (this.chat.id != undefined) {
      this.getChat();

      this.connectToSignalRChatHub();
    }
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  ngAfterViewChecked() {
    this.scrollToBottom();
  }
  scroll(): void {
    let element = this.myScrollContainer.nativeElement;
    let atBottom = (element.scrollTop + 20) >= (element.scrollHeight - element.offsetHeight);
    // console.log("element Height " + element.scrollHeight);
    // console.log("element offsetHeight " + element.offsetHeight);
    // console.log("element scroll " + element.scrollTop)
    // console.log(atBottom);
    if (atBottom) {
      this.disableScrollDown = false
    } else {
      this.disableScrollDown = true
    }
    if (element.scrollTop < 100 && !this.isFullListDisplayed) {
      element.scrollTop = 120;
      this.messagesSkip = this.chat.messages?.length!;
      this.getChatMessages();
    }
  }
  scrollToBottom() {
    if (this.disableScrollDown) {
      return
    }
    this.myScrollContainer.nativeElement.scrollTop = this.myScrollContainer.nativeElement.scrollHeight;

  }
  connectToSignalRChatHub(): void {
    this.signal.getDataStream<MessageModel>().pipe(takeUntil(this.unsubscribe$)).subscribe(message => {
      this.readMessages(message.data);
      message.data.status = MessageStatus.Read;
      this.chat.messages?.push(message.data);
      console.log(message);
    });
  }
  setSignalRUrls(): void {
    this.signal.setConnectionUrl = "https://localhost:6001/chatHub";
    this.signal.setHubMethod = "joinToChat";
    this.signal.setHubMethodParams = this.chat.id!;
    this.signal.setHandlerMethod = "BroadcastChatMessage";
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
    if (messages == undefined) return;
    this.chatService.readMessagesInchat(messages!).pipe(takeUntil(this.unsubscribe$)).subscribe(response => {
      console.log(response);
    },
      (error: HttpErrorResponse) => { console.error(error.error); });
  }
  getChat(): void {
    this.chatService.getChatById(this.chat.id!).pipe(takeUntil(this.unsubscribe$)).subscribe(response => {
      this.chat = response;
      this.chat.messages = [];
      console.log(response);
      this.getChatMessages()
    },
      (error: HttpErrorResponse) => { console.error(error.error); });
  }
  getChatMessages() {
    console.log(this.messagesSkip);
    console.log(this.chat.messages?.length);
    if (this.messagesSkip <= this.chat.messages?.length!) {
      this.chatService.getChatMessages(this.chat.id!, this.messagesTake, this.messagesSkip)
        .pipe(takeUntil(this.unsubscribe$)).subscribe(response => {
          if (response.length > 0) {
            response.forEach(m => {
              m.user = this.chat.users?.find(x => x.userId == m.userId)?.user!;
            });
            this.chat.messages = response.concat(this.chat.messages!);
            console.log(this.chat.messages);
            this.readMessages(null);
          }
          else {
            console.log("aa");
            this.isFullListDisplayed = true;
          }
        },
          (error: HttpErrorResponse) => { console.error(error.error); });
    }

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
