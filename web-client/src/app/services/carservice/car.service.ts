import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Form } from '@angular/forms';
import { Observable } from 'rxjs';
import { CarModel } from 'src/app/interfaces/car';

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
}
