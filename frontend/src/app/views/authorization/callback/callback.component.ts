import { Component, OnInit } from '@angular/core';
import {CurrentUser, UserService} from '../../../services/user.service';

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.css']
})
export class CallbackComponent implements OnInit {
  private currentUser: CurrentUser;
  private httpError: string;
  constructor(private userService: UserService) { }

  ngOnInit() {
    this.userService.getAuthenticatedUser().subscribe(resp => {
      this.currentUser = resp;
    }, err => {
      this.httpError = err;
    });
  }

}
