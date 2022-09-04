import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  searchUsers(userData: string): Observable<UserModel[]> {
    const url = 'https://localhost:6001/api/User/users/' + userData;
    return this.http.get<UserModel[]>(url);
  }
  getUserFromApi(userId: string): Observable<UserModel> {
    const url = 'https://localhost:6001/api/User/';
    return this.http.get<UserModel>(url + userId);
  }

  getCurrentUser(): Observable<UserModel> {
    const url = 'https://localhost:6001/api/User';
    return this.http.get<UserModel>(url);
  }
  updateUserPassword(newPasswordModel: { userId: string, currentPassword: string, newPassword: string }): Observable<any> {
    return this.http.post("https://localhost:5001/api/User/update-password", newPasswordModel);
  }
  updateUserInfo(userModel: {
    id: string,
    email: string,
    firstName: string,
    phoneNumber: string
  }): Observable<any> {
    return this.http.post("https://localhost:6001/api/User/update", userModel);
  }
  updateUserPhoto(formData: FormData): Observable<any> {
    return this.http.post('https://localhost:6001/api/User/updateUserImg', formData);
  }
  addDrivingLicense(formData: FormData): Observable<any> {
    return this.http.post('https://localhost:6001/api/User/license', formData)
  }
}
