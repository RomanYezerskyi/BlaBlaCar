import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AdminStatisticsModel } from 'src/app/interfaces/admin-interfaces/admin-statistics-model';
import { UsersListRequestModel } from 'src/app/interfaces/admin-interfaces/users-list-request-model';
import { CarStatus } from 'src/app/interfaces/car-interfaces/car-status';
import { RoleModel } from 'src/app/interfaces/role-model';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
import { UserRequestResponseModel } from 'src/app/interfaces/user-interfaces/user-request-response-model';
import { UserStatus } from 'src/app/interfaces/user-interfaces/user-status';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: HttpClient) { }
  getTopListUsers(request: UsersListRequestModel): Observable<UserModel[]> {
    const url = 'https://localhost:6001/api/Admin/top-list';
    return this.http.get<UserModel[]>(url,
      {
        params: {
          take: request.take,
          skip: request.skip,
          orderBy: request.orderBy
        }
      });
  }
  getStatistics(searchDate: string): Observable<AdminStatisticsModel> {
    const url = 'https://localhost:6001/api/Admin/statistics/' + searchDate;
    return this.http.get<AdminStatisticsModel>(url);
  }
  //
  getRoles(): Observable<RoleModel[]> {
    const url = 'https://localhost:5001/api/User/roles'
    return this.http.get<RoleModel[]>(url);
  }
  getAdmins(): Observable<UserModel[]> {
    const url = 'https://localhost:5001/api/User/admins'
    return this.http.get<UserModel[]>(url);
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
    const url = 'https://localhost:6001/api/Admin/requests/' + status;
    return this.http.get<UserRequestResponseModel>(url, { params: { take: take, skip: skip } });
  }

  changeUserDrivingLicenseStatus(newStatus: { status: UserStatus, userId: string }): Observable<any> {
    const url = 'https://localhost:6001/api/Admin/user/status';
    return this.http.patch(url, newStatus);
  }
  changeCarDocumentsStatus(newStatus: { status: CarStatus, carId: number, }): Observable<any> {
    const url = 'https://localhost:6001/api/Admin/car/status';
    return this.http.patch(url, newStatus);
  }
}
