import { HttpClient, HttpErrorResponse, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';
import { AuthenticatedResponse } from 'src/app/interfaces/authenticated-response';
import { LoginModel } from 'src/app/interfaces/login';
import { map } from 'rxjs/operators';
import { RegisterModel } from 'src/app/interfaces/register';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public redirectUrl: string | undefined;
  constructor(private router: Router, private jwtHelper: JwtHelperService, private http: HttpClient) { }

  async IsLogged(): Promise<boolean> {
    const token = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      console.log(this.jwtHelper.decodeToken(token))
      return true;
    }
    const isRefreshSuccess = await this.tryRefreshingTokens(token as string);
    console.log("try refresh " + isRefreshSuccess);
    if (!isRefreshSuccess) {
      this.logOut();
      this.router.navigate(["login"], { queryParams: { returnUrl: this.redirectUrl } });
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
      });
    });
    if (refreshRes.token == undefined || refreshRes.refreshToken == undefined) return !isRefreshSuccess
    localStorage.setItem("jwt", refreshRes.token);
    localStorage.setItem("refreshToken", refreshRes.refreshToken);
    isRefreshSuccess = true;
    return isRefreshSuccess;
  }
  private async logOut() {
    localStorage.removeItem("jwt");
    localStorage.removeItem("refreshToken");
  }
  login(credentials: LoginModel): Observable<AuthenticatedResponse> {
    let invalidLogin = false;
    return this.http.post<AuthenticatedResponse>("https://localhost:5001/api/auth/login", credentials, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })

    }).pipe(map(response => {
      const token = response.token;
      const refreshToken = response.refreshToken;
      localStorage.setItem("jwt", token);
      localStorage.setItem("refreshToken", refreshToken);
      return response;
    }));
  }
  Register(credentials: RegisterModel): Observable<any> {
    let invalidLogin = false;
    return this.http.post<any>("https://localhost:5001/api/auth/register", credentials, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    }).pipe(map(response => {
      return response;
    }));
  }


}
