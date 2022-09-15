import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Form } from '@angular/forms';
import { Observable } from 'rxjs';
import { CarModel } from 'src/app/interfaces/car-interfaces/car';
import { CarUpdateModel } from 'src/app/interfaces/car-interfaces/car-update-model';

@Injectable({
  providedIn: 'root'
})
export class CarService {

  constructor(private http: HttpClient) { }
  getUserCars(): Observable<CarModel[]> {
    return this.http.get<CarModel[]>("https://localhost:6001/api/Car/");
  }
  addCar(formData: FormData): Observable<any> {
    return this.http.post("https://localhost:6001/api/Car", formData)
  }
  updateCar(updateCarModel: FormData): Observable<any> {
    return this.http.post<any>("https://localhost:6001/api/Car/update-car", updateCarModel);
  }
  updateCarDocuments(carModel: any): Observable<any> {
    return this.http.post<any>("https://localhost:6001/api/Car/update-doc", carModel);
  }
  deleteCar(carId: number): Observable<any> {
    return this.http.delete("https://localhost:6001/api/Car/" + carId)
  }
}
