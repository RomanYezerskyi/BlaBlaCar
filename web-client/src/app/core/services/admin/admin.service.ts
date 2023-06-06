import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AdminStatisticsModel } from 'src/app/core/models/admin-models/admin-statistics-model';
import { ShortStatisticsModel } from 'src/app/core/models/admin-models/short-statistics-model';
import { UsersListRequestModel } from 'src/app/core/models/admin-models/users-list-request-model';
import { CarStatus } from 'src/app/core/models/car-models/car-status';
import { RoleModel } from 'src/app/core/models/auth-models/role-model';
import { UserModel } from 'src/app/core/models/user-models/user-model';
import { UserRequestResponseModel } from 'src/app/core/models/user-models/user-request-response-model';
import { UserStatus } from 'src/app/core/models/user-models/user-status';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private baseApiUrl = environment.baseApiUrl;
  private baseIdentityServerUrl = environment.baseIdentityServerUrl;
  constructor(private http: HttpClient) { }
  getTopListUsers(request: UsersListRequestModel): Observable<UserModel[]> {
    const url = this.baseApiUrl + 'Admin/top-list';
    return this.http.get<UserModel[]>(url,
      {
        params: {
          take: request.take,
          skip: request.skip,
          orderBy: request.orderBy
        },
        headers: { "ngrok-skip-browser-warning":"any"}
      });
  }
  getStatistics(searchDate: string): Observable<AdminStatisticsModel> {
    const url = this.baseApiUrl + 'Admin/statistics';
    return this.http.get<AdminStatisticsModel>(url, { params: { searchDate: searchDate }, headers: { "ngrok-skip-browser-warning":"any"} });
  }
  getShortStatistics(): Observable<ShortStatisticsModel> {
    const url = this.baseApiUrl + 'Admin/short-statistics';
    return this.http.get<ShortStatisticsModel>(url, {headers: { "ngrok-skip-browser-warning":"any"}});
  }
  //
  getRoles(): Observable<RoleModel[]> {
    const url = this.baseIdentityServerUrl + 'User/roles';
    return this.http.get<RoleModel[]>(url, {headers: { "ngrok-skip-browser-warning":"any"}});
  }
  getAdmins(): Observable<UserModel[]> {
    const url = this.baseIdentityServerUrl + 'User/admins';
    return this.http.get<UserModel[]>(url, {headers: { "ngrok-skip-browser-warning":"any"}});
  }
  changeUserRole(data: { roleName: string, userId: string }): Observable<any> {
    const url = this.baseIdentityServerUrl + 'User/';
    return this.http.post(url, data , {headers: { "ngrok-skip-browser-warning":"any"}});
  }
  getUserByEmail(email: string): Observable<UserModel> {
    const url = this.baseIdentityServerUrl + 'User/'
    return this.http.get<UserModel>(url + email, {headers: { "ngrok-skip-browser-warning":"any"}});
  }
  getUserRequests(status: UserStatus, take: number, skip: number): Observable<UserRequestResponseModel> {
    const url = this.baseApiUrl + 'Admin/requests';
    return this.http.get<UserRequestResponseModel>(url, { params: { take: take, skip: skip, status: status }, headers: { "ngrok-skip-browser-warning":"any"} });
  }

  changeUserDrivingLicenseStatus(newStatus: { status: UserStatus, userId: string }): Observable<any> {
    const url = this.baseApiUrl + 'Admin/user/status';
    return this.http.patch(url, newStatus,  {headers: { "ngrok-skip-browser-warning":"any"}});
  }
  changeCarDocumentsStatus(newStatus: { status: CarStatus, carId: number, }): Observable<any> {
    const url = this.baseApiUrl + 'Admin/car/status';
    return this.http.patch(url, newStatus, {headers: { "ngrok-skip-browser-warning":"any"}});
  }
}
