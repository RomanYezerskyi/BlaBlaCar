import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-chat-layout',
  templateUrl: './chat-layout.component.html',
  styleUrls: ['./chat-layout.component.scss']
})
export class ChatLayoutComponent implements OnInit {
  token = localStorage.getItem("jwt");
  currentUserId = '';

  constructor(private jwtHelper: JwtHelperService) { }

  ngOnInit(): void {
    this.currentUserId = this.jwtHelper.decodeToken(this.token!).id;
  }

}
