import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { JwtHelperService } from '@auth0/angular-jwt';
import { UserStatus } from 'src/app/interfaces/user-status';
import { HttpClient, HttpHeaders, HttpStatusCode } from '@angular/common/http';
@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  status = UserStatus;
  toggle: boolean = true;
  constructor(private jwtHelper: JwtHelperService, private http: HttpClient) { }

  ngOnInit(): void {

  }
  change() {
    this.toggle = !this.toggle;
  }
  logOut = async () => {
    // const refreshRes = await new Promise<HttpStatusCode>((resolve, reject) => {
    //   this.http.post("https://localhost:5001/api/token/revoke", {
    //     headers: new HttpHeaders({
    //       "Content-Type": "application/json"
    //     })
    //   }).subscribe({
    //     next: (res: any) => { resolve(res); console.log(res) },
    //     error: (_) => { reject; console.log(_) }
    //   });
    // });
    // console.log("aa");
    localStorage.removeItem("jwt");
    localStorage.removeItem("refreshToken");
    // return refreshRes;
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
      // console.log(this.jwtHelper.decodeToken(token))
      // console.log("aaa");
      // console.log(this.jwtHelper.decodeToken(token)['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']);
      // console.log(this.jwtHelper.decodeToken(token));
      let check = this.jwtHelper.decodeToken(token).role == 'blablacar.admin';
      let check1 = this.jwtHelper.decodeToken(token)['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] == 'blablacar.admin';
      if (check || check1) {
        return true;
      }
    }
    return false
  }
}
