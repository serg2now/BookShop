import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service'
import { User } from '../models/user'

@Component({
  selector: 'app-nav',
  templateUrl: './app-nav.component.html'
})
export class AppNavComponent implements OnInit {

  currentUser: User;
  
  constructor(private authService : AuthService, private router : Router) { }

  ngOnInit() {
  }

  loggedIn() {
    if (!this.currentUser || this.currentUser.userName.length == 0){
      this.currentUser = this.authService.getCurrnetUser();
    }

    return this.authService.loggedIn();
  }

  logout() {
    this.authService.logOut();
  }

  checkAdminRole(){
    var role = localStorage.getItem('user_role');
    return role.includes('Admin');
  }

}
