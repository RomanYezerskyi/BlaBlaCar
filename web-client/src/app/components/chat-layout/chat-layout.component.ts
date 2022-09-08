import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-chat-layout',
  templateUrl: './chat-layout.component.html',
  styleUrls: ['./chat-layout.component.scss']
})
export class ChatLayoutComponent implements OnInit {
  token = localStorage.getItem("jwt");
  currentUserId = '';
  role = '';
  constructor(private jwtHelper: JwtHelperService, private router: Router) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    this.currentUserId = this.jwtHelper.decodeToken(this.token!).id;
    this.role = this.jwtHelper.decodeToken(this.token!).role;
  }

}
