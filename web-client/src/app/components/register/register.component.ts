import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticatedResponse } from 'src/app/interfaces/authenticated-response';
import { RegisterModel } from 'src/app/interfaces/register';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  invalidLogin: boolean | undefined;
  credentials: RegisterModel = {firstName:'',email:'',phoneNum:'' ,password:''};
  data: any;
  
  constructor(private router: Router, private http: HttpClient) { }

  ngOnInit(): void {
  }
  register = ( form: NgForm) => {
    if (form.valid) {
      this.http.post<AuthenticatedResponse>("https://localhost:5001/api/auth/register", this.credentials, {
        headers: new HttpHeaders({ "Content-Type": "application/json"})
      })
      .subscribe((res)=>{
        this.data = res
        console.log(this.data)
        console.error(res);
      })
    } 
  }
  
}

