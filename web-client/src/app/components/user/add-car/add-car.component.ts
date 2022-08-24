import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, NgForm, Validators } from '@angular/forms';
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
    countOfSeats: 1, modelName: '', registNum: '', carType: 0,
    techPassportFile: []
  };
  carType = CarType;
  imageBase64 = '';
  fileSelected?: File;
  imageUrl?: string;
  private formData = new FormData();
  CarFormControl = new FormControl('', [Validators.required]);
  fileControl: FormControl;
  public files: any;
  constructor(private http: HttpClient, private sant: DomSanitizer,) {
    this.fileControl = new FormControl(this.files, [
      Validators.required,
    ])
  }

  ngOnInit(): void {
    this.fileControl.valueChanges.subscribe((files: any) => {
      this.uploadFile(files);
    })
  }

  uploadFile = (files: any) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload: Array<any> = files;
    for (let item of fileToUpload) {
      this.formData.append('techPassportFile', item, item.name);
    }
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
function MaxSizeValidator(arg0: number): import("@angular/forms").ValidatorFn {
  throw new Error('Function not implemented.');
}

