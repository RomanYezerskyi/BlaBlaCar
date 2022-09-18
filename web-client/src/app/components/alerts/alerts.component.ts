import { Component, EventEmitter, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-alerts',
  templateUrl: './alerts.component.html',
  styleUrls: ['./alerts.component.scss']
})
export class AlertsComponent implements OnInit {
  @Input() showMessageEvent: EventEmitter<string> = new EventEmitter<string>;
  successMessages: Array<string> = [];
  errorMessages: Array<string> = [];
  constructor() { }

  ngOnInit(): void {
  }
  showError(message: string) {
    this.errorMessages.push(message);
    let myTimer = setTimeout(() => { this.errorMessages.shift(); }, 2000);
  }
  // show(message: string) {
  //   this.errorMessages.push(message);
  //   let myTimer = setTimeout(() => { this.errorMessages.shift(); }, 2000);
  // }

}
