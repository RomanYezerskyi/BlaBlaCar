import { Component, OnDestroy, OnInit } from '@angular/core';
import { UserStatus } from 'src/app/core/models/user-models/user-status';
import * as signalR from '@microsoft/signalr';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ActivatedRoute, Router } from '@angular/router';
import { SignalRService } from 'src/app/core/services/signalr-services/signalr.service';
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
  // chatId: string = '';
  // unreadMessages: number = 0;
  constructor(private jwtHelper: JwtHelperService,
    private route: ActivatedRoute,
    private signal: SignalRService) {

    // this.setSignalRUrls();
  }

  ngOnInit(): void {

  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

}


