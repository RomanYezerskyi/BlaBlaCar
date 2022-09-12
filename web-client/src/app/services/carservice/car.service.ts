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
  deleteDoc(file: string): Observable<any> {
    var filePath = new FormData()
    filePath.append("filePath", file)
    return this.http.post<any>("https://localhost:6001/api/Files", filePath)
  }
  updateCar(updateCarModel: FormData): Observable<any> {
    return this.http.post<any>("https://localhost:6001/api/Car/update-car", updateCarModel);
  }
  updateCarDocuments(carModel: any): Observable<any> {
    return this.http.post<any>("https://localhost:6001/api/Car/update-doc", carModel);
  }
}
