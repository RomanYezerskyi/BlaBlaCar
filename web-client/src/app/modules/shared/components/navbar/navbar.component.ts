import { Component, OnDestroy, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { UserStatus } from 'src/app/core/models/user-models/user-status';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from 'src/app/core/services/user-service/user.service';
import { AuthService } from 'src/app/core/services/auth-service/auth-service.service';
import { Subject, takeUntil } from 'rxjs';
import { SignalRService } from 'src/app/core/services/signalr-services/signalr.service';
import { ChatService } from 'src/app/core/services/chat-service/chat.service';
import { ImgSanitizerService } from 'src/app/core/services/image-sanitizer-service/img-sanitizer.service';
import { SafeUrl } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';
@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit, OnDestroy {
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
  countNotify($event: number): void {
    this.notifiNotReadCount = $event;
  }
  change(): void {
    this.toggle = !this.toggle;
  }
  logOut(): void {
    this.authService.logOut();
  }
  isUserAuthenticated(): boolean {
    const token = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    }
    return false;
  }
  isAdmin = (): boolean => {
    return this.authService.isAdmin();
  }
  sanitizaImg(img: string): SafeUrl {
    return this.imgSanitize.sanitiizeUserImg(img);
  }
}
