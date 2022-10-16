import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-chat-layout',
  templateUrl: './chat-layout.component.html',
  styleUrls: ['./chat-layout.component.scss']
})
export class ChatLayoutComponent implements OnInit {
  private token: string | null = localStorage.getItem("jwt");
  currentUserId: string = '';
  role: string = '';
  isChatSelected: boolean = false;
  constructor(private jwtHelper: JwtHelperService, private route: ActivatedRoute, private router: Router,) {
    // this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['chatId']) {
        this.isChatSelected = true;
      }
    });
    this.currentUserId = this.jwtHelper.decodeToken(this.token!).id;
  }

}
