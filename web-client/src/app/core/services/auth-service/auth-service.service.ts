import { HttpClient, HttpErrorResponse, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';
import { AuthenticatedResponseModel } from 'src/app/interfaces/authenticated-response-model';
import { LoginModel } from 'src/app/interfaces/login-model';
import { map } from 'rxjs/operators';
import { RegisterModel } from 'src/app/interfaces/register-model';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public redirectUrl: string | undefined;
  private token: string | null = localStorage.getItem("jwt");
  constructor(private router: Router, private jwtHelper: JwtHelperService, private http: HttpClient) { }

  async IsLogged(): Promise<boolean> {
    const token = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    }
    const isRefreshSuccess = await this.tryRefreshingTokens(token as string);
    console.log("try refresh " + isRefreshSuccess);
    if (!isRefreshSuccess) {
      this.logOut();
      this.router.navigate(["auth/login"], { queryParams: { returnUrl: this.redirectUrl } });
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
    const refreshRes = await new Promise<AuthenticatedResponseModel>((resolve, reject) => {
      this.http.post<AuthenticatedResponseModel>("https://localhost:5001/api/token/refresh", credentials, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe({
        next: (res: AuthenticatedResponseModel) => resolve(res),
        error: (_: AuthenticatedResponseModel) => resolve(_)
      });
    });
    if (refreshRes.token == undefined || refreshRes.refreshToken == undefined) return !isRefreshSuccess
    localStorage.setItem("jwt", refreshRes.token);
    localStorage.setItem("refreshToken", refreshRes.refreshToken);
    isRefreshSuccess = true;
    return isRefreshSuccess;
  }
  logOut(): void {
    localStorage.removeItem("jwt");
    localStorage.removeItem("refreshToken");
    this.router.navigate(["auth/login"]);
  }
  login(credentials: LoginModel): Observable<AuthenticatedResponseModel> {
    return this.http.post<AuthenticatedResponseModel>("https://localhost:5001/api/auth/login", credentials, {
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
    return this.http.post<any>("https://localhost:5001/api/auth/register", credentials, {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    }).pipe(map(response => {
      return response;
    }));
  }
  isAdmin = (): boolean => {
    if (this.token && !this.jwtHelper.isTokenExpired(this.token)) {
      let check = this.jwtHelper.decodeToken(this.token).role == 'blablacar.admin';
      let check1 = this.jwtHelper.decodeToken(this.token)['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] == 'blablacar.admin';
      if (check || check1) {
        return true;
      }
    }
    return false
  }
}
