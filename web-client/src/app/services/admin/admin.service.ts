import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CarStatus } from 'src/app/interfaces/car-status';
import { RoleModel } from 'src/app/interfaces/role';
import { UserModel } from 'src/app/interfaces/user-model';
import { UserStatus } from 'src/app/interfaces/user-status';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: HttpClient) { }
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
  getUserRequests(status: UserStatus): Observable<UserModel[]> {
    const url = 'https://localhost:6001/api/Admin/requests/';
    return this.http.get<UserModel[]>(url + status);
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
