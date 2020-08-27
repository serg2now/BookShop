import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { AlertifyService } from '../services/alertify.service';
import { window } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router,
    private alertify: AlertifyService) {

  }

  canActivate(): boolean {
    var isUserLogedIn = this.authService.loggedIn();
    var role = localStorage.getItem('user_role');
    var route = location.pathname;

    if (!isUserLogedIn) {
      this.alertify.error('Вы не авторизованы!');
      this.router.navigate(['/']);

      return false;
    }

    if (route == '/' || route.includes('books') || (route.includes('orders') && role.includes('Admin'))){
      return true;
    } else{
      this.router.navigate(['/books']);
      return false;
    }
  }
}