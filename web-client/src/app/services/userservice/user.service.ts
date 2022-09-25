import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { UserModel } from 'src/app/interfaces/user-interfaces/user-model';
@Injectable({
  providedIn: 'root'
})
export class UserService {
  public userProfile: UserModel = {} as UserModel;
  constructor(private http: HttpClient) {
    console.log(this.userProfile);
    if (this.userProfile.id == undefined) {
      this.getCurrentUser().subscribe(
        response => {
          console.log("get user");
          this.userProfile = response;
          console.log(this.userProfile);
        },
        (error: HttpErrorResponse) => { console.log(error.error); }
      );
    }
  }
  async chekIfUserExist(): Promise<any> {
    const url = 'https://localhost:6001/api/User/add-user';
    return await new Promise<any>((resolve, reject) => {
      this.http.get<any>(url).subscribe({
        next: (res) => resolve(res),
        error: (_) => resolve(_)
      });
    });
  }
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
    return this.http.put('https://localhost:6001/api/User/user-profile-image', formData);
  }
  addDrivingLicense(formData: FormData): Observable<any> {
    return this.http.put('https://localhost:6001/api/User/license', formData)
  }
  getUserStatistics(): Observable<any> {
    return this.http.get<any>('https://localhost:6001/api/User/statistics');
  }
}
