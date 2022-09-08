import { Component, OnInit } from '@angular/core';
import { UserStatus } from 'src/app/interfaces/user-interfaces/user-status';
import * as signalR from '@microsoft/signalr';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ActivatedRoute, Router } from '@angular/router';
import { SignalRService } from 'src/app/services/signalr-services/signalr.service';
@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.scss'],
  providers: [SignalRService]
})
export class AdminPanelComponent implements OnInit {
  userStatus = UserStatus;
  token = localStorage.getItem("jwt");
  chatId: string = '';
  unreadMessages: number = 0;
  constructor(private jwtHelper: JwtHelperService,
    private route: ActivatedRoute,
    private signal: SignalRService) {
    const currentUserId = this.jwtHelper.decodeToken(this.token!).id;
    this.signal.url = "https://localhost:6001/chatHub";
    this.signal.hubMethod = 'JoinToChatMessagesNotifications';
    this.signal.hubMethodParams = currentUserId;
    this.signal.handlerMethod = "BroadcastMessagesFromChats";
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['chatId']) {
        this.chatId = params['chatId'];
      }
    });
    console.log("aaa");
    this.signal.getDataStream<string>().subscribe(message => {
      console.log(message.data);
      if (this.chatId == '' || this.chatId != message.data) {
        this.unreadMessages += 1;
      }
    });
    // const currentUserId = this.jwtHelper.decodeToken(this.token!).id;
    // const connection = new signalR.HubConnectionBuilder()
    //   .configureLogging(signalR.LogLevel.Information)
    //   .withUrl('https://localhost:6001/' + 'chatHub')
    //   .build();
    // connection.start().then(function () {
    //   console.log('admin panel SignalR Connected!');
    //   connection.invoke('JoinToChatMessagesNotifications', currentUserId)
    // }).catch(function (err) {
    //   return console.error(err.toString());
    // });
    // connection.on("BroadcastMessagesFromChats", (chatId) => {
    //   this.unreadMessages += 1;
    //   if (this.chatId == '' || this.chatId != chatId) {
    //     this.unreadMessages += 1;
    //   }
    // });
  }
}


