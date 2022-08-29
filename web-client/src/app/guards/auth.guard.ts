import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthenticatedResponse } from '../interfaces/authenticated-response';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private jwtHelper: JwtHelperService, private http: HttpClient) { }

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    console.log(state.url);
    const token = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      console.log(this.jwtHelper.decodeToken(token))
      return true;
    }
    const isRefreshSuccess = await this.tryRefreshingTokens(token as string);
    console.log("try refresh " + isRefreshSuccess);
    if (!isRefreshSuccess) {
      this.logOut();
      this.router.navigate(["login"], { queryParams: { returnUrl: state.url } });
    }
    return isRefreshSuccess;
  }

  private async tryRefreshingTokens(token: string): Promise<boolean> {
    const refreshToken = localStorage.getItem("refreshToken");
    if (!token || !refreshToken) {
      return false;
    }
    const credentials = JSON.stringify({ accessToken: token, refreshToken: refreshToken });
    let isRefreshSuccess = true;
    const refreshRes = await new Promise<AuthenticatedResponse>((resolve, reject) => {
      this.http.post<AuthenticatedResponse>("https://localhost:5001/api/token/refresh", credentials, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe({
        next: (res: AuthenticatedResponse) => resolve(res),
        error: (_: AuthenticatedResponse) => resolve(_)
        // {
        //   reject(_);
        //   isRefreshSuccess = false;
        //   this.logOut();
        //   this.router.navigate(["login"]);
        // }
      });
    });
    if (refreshRes.token == undefined || refreshRes.refreshToken == undefined) return !isRefreshSuccess
    localStorage.setItem("jwt", refreshRes.token);
    localStorage.setItem("refreshToken", refreshRes.refreshToken);
    isRefreshSuccess = true;
    return isRefreshSuccess;
  }
  private async logOut() {

    // const refreshRes = await new Promise<any>((resolve, reject) => {
    //   this.http.post<any>("https://localhost:5001/api/token/revoke", {
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
  }
}
