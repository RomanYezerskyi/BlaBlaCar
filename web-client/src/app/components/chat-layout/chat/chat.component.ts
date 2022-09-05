import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Chat } from 'src/app/interfaces/chat-interfaces/chat';
import { ChatService } from 'src/app/services/chatservice/chat.service';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';
import * as signalR from '@microsoft/signalr';
import { JwtHelperService } from '@auth0/angular-jwt';
@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit, OnDestroy {
  chatSubscription!: Subscription;
  messageSubscription!: Subscription;
  chat: Chat = { id: '' };
  text = '';
  @Input() currentUserId = '';
  constructor(private route: ActivatedRoute, private chatService: ChatService, private sanitizeImgService: ImgSanitizerService) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['chatId']) {
        this.chat.id = params['chatId'];
        this.getChat();
      }
    });
    if (this.chat.id != '') {
      console.log('aa');
      const connection = new signalR.HubConnectionBuilder()
        .configureLogging(signalR.LogLevel.Information)
        .withUrl('https://localhost:6001/' + 'chatHub')
        .build();

      const chatId = this.chat.id!;
      connection.start().then(function () {
        console.log('SignalR Connected!');
        connection.invoke('getChatConnectionId', chatId)
          .then(function (connectionId) {
            console.log("connectionChatID: " + connectionId);
          });
      }).catch(function (err) {
        return console.error(err.toString());
      });
      connection.on("BroadcastChatMessage", (message) => {
        console.log("Hi chat!!!");
        this.chat.messages?.push(message)
        console.log(message);
      });
    }
  }
  ngOnDestroy(): void {
    this.chatSubscription.unsubscribe();
  }
  getChat() {
    this.chatSubscription = this.chatService.getChatById(this.chat.id!).subscribe(
      response => {
        this.chat = response;
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
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
        alert(response);
        console.log(response);
      },
      (error: HttpErrorResponse) => { console.error(error.error); }
    );
  }
}
