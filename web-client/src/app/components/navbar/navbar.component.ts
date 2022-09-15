import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { JwtHelperService } from '@auth0/angular-jwt';
import { UserStatus } from 'src/app/interfaces/user-interfaces/user-status';
import { HttpClient, HttpHeaders, HttpStatusCode } from '@angular/common/http';
import { Router } from '@angular/router';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { UserProfileComponent } from '../user/user-profile/user-profile.component';
import { UserService } from 'src/app/services/userservice/user.service';
@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  userStatus = UserStatus;
  toggle: boolean = true;
  notification: boolean = false;
  notifiNotReadCount = 0;
  constructor(
    private jwtHelper: JwtHelperService,
    private http: HttpClient,
    private router: Router,
    public userService: UserService) { }

  ngOnInit(): void {

  }
  countNotify($event: number) {
    this.notifiNotReadCount = $event;
  }
  change() {
    this.toggle = !this.toggle;
  }
  navigateToUserRequests = (id: number) => {
    this.router.navigate(['/admin/requests/', id])
  }
  logOut = async () => {
    localStorage.removeItem("jwt");
    localStorage.removeItem("refreshToken");
  }
  isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    }
    return false;
  }
  isAdmin = (): boolean => {
    const token = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      let check = this.jwtHelper.decodeToken(token).role == 'blablacar.admin';
      let check1 = this.jwtHelper.decodeToken(token)['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] == 'blablacar.admin';
      if (check || check1) {
        return true;
      }
    }
    return false
  }
}
