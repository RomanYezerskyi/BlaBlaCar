import { Component, OnDestroy, OnInit } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Subject, takeUntil } from 'rxjs';
import { AuthService } from 'src/app/core/services/auth-service/auth-service.service';
import { ChatService } from 'src/app/core/services/chat-service/chat.service';
import { ImgSanitizerService } from 'src/app/core/services/image-sanitizer-service/img-sanitizer.service';
import { SignalRService } from 'src/app/core/services/signalr-services/signalr.service';
import { UserService } from 'src/app/core/services/user-service/user.service';
import { UserStatus } from 'src/app/core/models/user-models/user-status';
import { environment } from 'src/environments/environment';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-admin-navbar',
  templateUrl: './admin-navbar.component.html',
  styleUrls: ['./admin-navbar.component.scss']
})
export class AdminNavbarComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  private token: string | null = localStorage.getItem("jwt");
  userStatus = UserStatus;
  toggle: boolean = true;
  notification: boolean = false;
  notifiNotReadCount: number = 0;
  unreadMessages: number = 0;
  constructor(
    private jwtHelper: JwtHelperService,
    private router: Router,
    private route: ActivatedRoute,
    public userService: UserService,
    private authService: AuthService,
    private signal: SignalRService,
    private chatService: ChatService,
    private imgSanitize: ImgSanitizerService) {
    this.setSignalRUrls();

  }

  ngOnInit(): void {
    this.getUser();
    this.isUnreadMessages();
    this.connectToChatMessagesSignalRHub();
    this.router.events.subscribe(val => {
      if (this.router.url.includes("/chat")) {
        this.unreadMessages = 0;
      }
    });
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  isUnreadMessages(): void {
    this.chatService.isUreadMessages().pipe(takeUntil(this.unsubscribe$)).subscribe(result => {
      if (result && !this.router.url.includes("/chat")) this.unreadMessages += result;
    });
  }
  setSignalRUrls(): void {
    if (!this.token) { return }
    const currentUserId = this.jwtHelper.decodeToken(this.token!).id;

    this.signal.setConnectionUrl = environment.chatHubConnectionUrl;
    this.signal.setHubMethod = environment.chatHubMethod;
    this.signal.setHubMethodParams = currentUserId;
    this.signal.setHandlerMethod = environment.chatHubHandlerMethod;
  }
  connectToChatMessagesSignalRHub(): void {
    this.signal.getDataStream<string>().pipe(takeUntil(this.unsubscribe$)).subscribe(message => {
      console.log(message.data);
      if (!this.router.url.includes("/chat")) {
        this.unreadMessages += 1;
      }
    });
  }
  getUser(): void {
    this.userService.getCurrentUser().pipe(takeUntil(this.unsubscribe$)).subscribe(
      response => {
        console.log("get user");
        console.log(response);
        this.userService.userProfile = response;
      },
      (error: HttpErrorResponse) => { console.log(error.error); }
    );;
  }
  change(): void {
    this.toggle = !this.toggle;
  }
  logOut(): void {
    this.authService.logOut();
  }
  sanitizaImg(img: string): SafeUrl {
    return this.imgSanitize.sanitiizeUserImg(img);
  }

}
