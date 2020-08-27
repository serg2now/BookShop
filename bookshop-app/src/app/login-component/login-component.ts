import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../services/alertify.service';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'login-component',
  templateUrl: './login-component.html'
})
export class LoginComponent implements OnInit {

  model: any = {};

  constructor(private alertifyService : AlertifyService, private authService : AuthService, private router : Router) { }

  ngOnInit() {
    if (this.authService.loggedIn()){
      this.router.navigate(['/books']);
    }
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      var currentUser = this.authService.getCurrnetUser();
      this.alertifyService.success(`Привет, ${currentUser.userName}`);
    }, error => {
      this.alertifyService.error("Ошибка входа!");
      }, () => {
        this.router.navigate(['/books']);
      });
  }

}
