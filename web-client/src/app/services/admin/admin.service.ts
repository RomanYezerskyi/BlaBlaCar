import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AdminStatistics } from 'src/app/interfaces/admin-interfaces/admin-statistics';
import { UsersListRequest } from 'src/app/interfaces/admin-interfaces/users-list-request';
import { CarStatus } from 'src/app/interfaces/car-interfaces/car-status';
import { RoleModel } from 'src/app/interfaces/role';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { UserRequestResponseModel } from 'src/app/interfaces/user-interfaces/user-request-response-model';
import { UserStatus } from 'src/app/interfaces/user-interfaces/user-status';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: HttpClient) { }
  getTopListUsers(request: UsersListRequest): Observable<UserModel[]> {
    const url = 'https://localhost:6001/api/Admin/top-list/' + request.take + "/" + request.skip + "/" + request.orderBy;
    return this.http.get<UserModel[]>(url);
  }
  getStatistics(): Observable<AdminStatistics> {
    const url = 'https://localhost:6001/api/Admin/statistics';
    return this.http.get<AdminStatistics>(url);
  }
  //
  getRoles(): Observable<RoleModel[]> {
    const url = 'https://localhost:5001/api/User/roles'
    return this.http.get<RoleModel[]>(url);
  }
  changeUserRole(data: { roleName: string, userId: string }): Observable<any> {
    const url = 'https://localhost:5001/api/User/'
    return this.http.post(url, data);
  }
  getUserByEmail(email: string): Observable<UserModel> {
    const url = 'https://localhost:5001/api/User/'
    return this.http.get<UserModel>(url + email);
  }
  getUserRequests(status: UserStatus, take: number, skip: number): Observable<UserRequestResponseModel> {
    const url = 'https://localhost:6001/api/Admin/requests/';
    return this.http.get<UserRequestResponseModel>(url + status + "/" + take + "/" + skip);
  }
  getUserRequest(userId: string): Observable<UserModel> {
    const url = 'https://localhost:6001/api/Admin/';
    return this.http.get<UserModel>(url + userId);
  }
  changeUserDrivingLicenseStatus(newStatus: { status: UserStatus, userId: string }): Observable<any> {
    const url = 'https://localhost:6001/api/Admin/user/status';
    return this.http.post(url, newStatus);
  }
  changeCarDocumentsStatus(newStatus: { status: CarStatus, carId: number, }): Observable<any> {
    const url = 'https://localhost:6001/api/Admin/car/status';
    return this.http.post(url, newStatus);
  }
}
