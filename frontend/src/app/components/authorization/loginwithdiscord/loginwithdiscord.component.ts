import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-loginwithdiscord',
  templateUrl: './loginwithdiscord.component.html',
  styleUrls: ['./loginwithdiscord.component.css']
})
export class LoginwithdiscordComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  RedirectToDiscordOAuth() {
    window.location.replace('http://localhost:8080/authentication/signinwithdiscord');
  }
}
