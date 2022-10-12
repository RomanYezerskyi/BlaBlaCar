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
  showError(message: string, time: number = 0): void {
    this.errorMessages.push(message);
    let myTimer = setTimeout(() => { this.errorMessages.shift(); }, time == 0 ? 4000 : time);
  }
  showSuccessMessages(message: string, time: number = 0): void {
    this.successMessages.push(message);
    let myTimer = setTimeout(() => { this.successMessages.shift(); }, time == 0 ? 4000 : time);
  }
  hideSuccessMessages(): void {
    this.successMessages.shift();
  }
  hideErrorMessages(): void {
    this.errorMessages.shift();
  }
}
