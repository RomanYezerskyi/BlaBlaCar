import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-auth-navbar',
  templateUrl: './auth-navbar.component.html',
  styleUrls: ['./auth-navbar.component.scss']
})
export class AuthNavbarComponent implements OnInit {
  toggle: boolean = true;
  constructor(private jwtHelper: JwtHelperService) { }

  ngOnInit(): void {
  }
  change(): void {
    this.toggle = !this.toggle;
  }
  isUserAuthenticated(): boolean {
    const token = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    }
    return false;
  }
}
