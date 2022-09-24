import { Component, OnDestroy, OnInit } from '@angular/core';
import { UserStatus } from 'src/app/interfaces/user-interfaces/user-status';
import * as signalR from '@microsoft/signalr';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ActivatedRoute, Router } from '@angular/router';
import { SignalRService } from 'src/app/services/signalr-services/signalr.service';
import { Subject, takeUntil } from 'rxjs';
@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.scss'],
  providers: [SignalRService]
})
export class AdminPanelComponent implements OnInit, OnDestroy {
  private unsubscribe$: Subject<void> = new Subject<void>();
  userStatus = UserStatus;
  private token: string | null = localStorage.getItem("jwt");
  chatId: string = '';
  unreadMessages: number = 0;
  constructor(private jwtHelper: JwtHelperService,
    private route: ActivatedRoute,
    private signal: SignalRService) {

    this.setSignalRUrls();
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['chatId']) {
        this.chatId = params['chatId'];
      }
    });
    this.connectToChatMessagesSignalRHub()
  }
  setSignalRUrls(): void {
    const currentUserId = this.jwtHelper.decodeToken(this.token!).id;
    this.signal.setConnectionUrl = "https://localhost:6001/chatHub";
    this.signal.setHubMethod = 'JoinToChatMessagesNotifications';
    this.signal.setHubMethodParams = currentUserId;
    this.signal.setHandlerMethod = "BroadcastMessagesFromChats";
  }
  connectToChatMessagesSignalRHub(): void {
    this.signal.getDataStream<string>().subscribe(message => {
      console.log(message.data);
      if (this.chatId == '' || this.chatId != message.data) {
        this.unreadMessages += 1;
      }
    });
  }
  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

}


