import { Component, OnInit } from '@angular/core';
import { UserStatus } from 'src/app/interfaces/user-interfaces/user-status';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.scss']
})
export class AdminPanelComponent implements OnInit {
  userStatus = UserStatus;
  constructor() { }

  ngOnInit(): void {
  }

}
