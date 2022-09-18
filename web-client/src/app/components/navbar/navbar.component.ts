import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { UserStatus } from 'src/app/interfaces/user-interfaces/user-status';
import { Router } from '@angular/router';
import { UserService } from 'src/app/services/userservice/user.service';
import { AuthService } from 'src/app/services/authservice/auth-service.service';
@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  userStatus = UserStatus;
  toggle: boolean = true;
  notification: boolean = false;
  notifiNotReadCount: number = 0;
  constructor(
    private jwtHelper: JwtHelperService,
    private router: Router,
    public userService: UserService,
    private authService: AuthService) { }

  ngOnInit(): void {
  }
  countNotify($event: number): void {
    this.notifiNotReadCount = $event;
  }
  change(): void {
    this.toggle = !this.toggle;
  }
  navigateToUserRequests = (id: number) => {
    this.router.navigate(['/admin/requests/', id])
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
}
