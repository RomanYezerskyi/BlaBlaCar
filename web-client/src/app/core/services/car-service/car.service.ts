import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Form } from '@angular/forms';
import { Observable } from 'rxjs';
import { CarModel } from 'src/app/core/models/car-models/car-model';
import { CarUpdateModel } from 'src/app/core/models/car-models/car-update-model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CarService {
  private baseApiUrl = environment.baseApiUrl;
  constructor(private http: HttpClient) { }
  getUserCars(): Observable<CarModel[]> {
    const url = this.baseApiUrl + 'Car/';
    return this.http.get<CarModel[]>(url, {headers: { "ngrok-skip-browser-warning":"any"}});
  }
  addCar(formData: FormData): Observable<any> {
    const url = this.baseApiUrl + 'Car/';
    return this.http.post(url, formData, {headers: { "ngrok-skip-browser-warning":"any"}})
  }
  updateCar(updateCarModel: FormData): Observable<any> {
    const url = this.baseApiUrl + 'Car/';
    return this.http.put<any>(url, updateCarModel, {headers: { "ngrok-skip-browser-warning":"any"}});
  }
  updateCarDocuments(carModel: any): Observable<any> {
    const url = this.baseApiUrl + 'Car/update-doc/';
    return this.http.put<any>(url, carModel , {headers: { "ngrok-skip-browser-warning":"any"}});
  }
  deleteCar(carId: number): Observable<any> {
    const url = this.baseApiUrl + 'Car/';
    return this.http.delete(url + carId, {headers: { "ngrok-skip-browser-warning":"any"}})
  }
}
