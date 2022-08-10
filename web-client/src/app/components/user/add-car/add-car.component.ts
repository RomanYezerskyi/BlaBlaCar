import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { CarType } from 'src/app/enums/car-type';
import { AddNewCarModel } from 'src/app/interfaces/addnew-car';

@Component({
  selector: 'app-add-car',
  templateUrl: './add-car.component.html',
  styleUrls: ['./add-car.component.scss']
})
export class AddCarComponent implements OnInit {
  newCar: AddNewCarModel = {
    countOfSeats: 0, modelName: '', registNum: '', carType: 0,
    techPassportFile: []
  };
  carType = CarType;
  imageBase64 = '';
  fileSelected?: File;
  imageUrl?: string;
  private formData = new FormData();
  constructor(private http: HttpClient, private sant: DomSanitizer,) { }

  ngOnInit(): void {
  }

  uploadFile = (files: any) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload: Array<any> = files;
    console.log(fileToUpload);
    for (let item of fileToUpload) {
      this.formData.append('techPassportFile', item, item.name);
    }

    //this.formData.append('TechPassportFile', fileToUpload, fileToUpload.name);

    //this.newCar.techPassportFile = this.formData;
    // console.log(this.newCar.techPassportFile.get);
  }

  addCar = (form: NgForm) => {
    if (form.valid) {
      this.formData.append("ModelName", this.newCar.modelName);
      this.formData.append("RegistNum", this.newCar.registNum);
      this.formData.append("CountOfSeats", this.newCar.countOfSeats.toString());
      this.http.post("https://localhost:6001/api/Car", this.formData)
        .subscribe({
          next: (res) => {
            console.log(res)
          },
          error: (err: HttpErrorResponse) => console.error(err)
        })
    }
  }

}
