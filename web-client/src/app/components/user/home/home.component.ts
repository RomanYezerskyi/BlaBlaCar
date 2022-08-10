import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  data: any;
  constructor(private jwtHelper: JwtHelperService, private http: HttpClient) { }
  
  ngOnInit(): void {
    this.http.get("https://localhost:6001/api/WeatherForecast")
    .subscribe({
      next: (result: any) => this.data = result,
      error: (err: HttpErrorResponse) => console.log(err)
    })
  }
  logOut = () => {
    localStorage.removeItem("jwt");
    localStorage.removeItem("refreshToken");
  }
  isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
  return false;
  }
  // }
  // logOut = () => {
  //   localStorage.removeItem("jwt");
  // }

}
