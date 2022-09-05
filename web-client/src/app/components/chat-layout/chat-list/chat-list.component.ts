import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Chat } from 'src/app/interfaces/chat-interfaces/chat';
import { ChatService } from 'src/app/services/chatservice/chat.service';
import { ImgSanitizerService } from 'src/app/services/imgsanitizer/img-sanitizer.service';

@Component({
  selector: 'app-chat-list',
  templateUrl: './chat-list.component.html',
  styleUrls: ['./chat-list.component.scss']
})
export class ChatListComponent implements OnInit, OnDestroy {
  chatsSubscription!: Subscription;
  chats: Array<Chat> = [];
  @Input() currentUserId = '';
  constructor(private chatService: ChatService, private router: Router, private sanitizeImgService: ImgSanitizerService) { }

  ngOnInit(): void {
    this.getUserChats();
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
    this.router.navigate(['admin/chat'], {
      queryParams: {
        chatId: chatId
      }
    });
  }
  sanitizeImg(img: string): SafeUrl {

    return this.sanitizeImgService.sanitiizeUserImg(img);
  }

}
