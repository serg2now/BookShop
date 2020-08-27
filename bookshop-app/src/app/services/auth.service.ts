import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from '../models/user';
import { environment } from '../../environments/environment';
import { TokenExpirationData } from '../models/tokenExpirationData';
import { DeviceDetectorService } from 'ngx-device-detector';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = environment.apiUrl + 'auth/';
  private currentUser : User;
  private jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient, private deviceService: DeviceDetectorService, private router: Router ) 
  {
  }

  login(model: any) {
    var deviceInfo = this.deviceService.getDeviceInfo();
    var headers = new HttpHeaders();
    headers = headers.append("device-info", deviceInfo.os + "-" + deviceInfo.browser);

    return this.http.post(this.baseUrl + 'login', model, {withCredentials: true, headers: headers})
      .pipe(
      map((response: any) => {
        const user = response;

        if (user) {
         this.setUserData(user);
        }
      })
      )
  }

  getCurrnetUser(){
    var user = JSON.parse(localStorage.getItem('user'));
    this.currentUser = (user) ? user : {userName : ''};
    
    return this.currentUser;
  }

  loggedIn() {
    var user = JSON.parse(localStorage.getItem('user'));
    var t_exp = localStorage.getItem('t_exp');

    return user != null && t_exp != null;
  }

  checkIsTokenValid(){
    var t_exp = localStorage.getItem('t_exp');

      if (t_exp){
        let tokenData: TokenExpirationData = JSON.parse(t_exp); 
        let curr_date = Math.floor(new Date().getTime()/1000);

        return curr_date >= tokenData.nbf && curr_date < tokenData.exp;
      } else{
        this.logOut();
      }
    
      return false;
  }

  async refreshUserToken(){
    return await this.http.post(this.baseUrl + 'refreshToken', null, { withCredentials: true })
    .pipe(
    map((response: any) => {
      const user = response;

      if (user) {
       this.setUserData(user);
      }
    })
    ).toPromise();
  }

  setUserData(user: any) {
    this.currentUser = user.user;

    var decodedToken = this.jwtHelper.decodeToken(user.token);

    let tokenExpData: TokenExpirationData = { exp: decodedToken.exp, nbf: decodedToken.nbf };
    localStorage.setItem('user', JSON.stringify(this.currentUser));
    localStorage.setItem('t_exp', JSON.stringify(tokenExpData));
    localStorage.setItem('user_role', JSON.stringify(decodedToken.role));
  }

  logOut(){
    return this.http.post(this.baseUrl + 'logout', null, {withCredentials: true}).subscribe(res =>{
        this.currentUser = null;
        localStorage.removeItem('user');
        localStorage.removeItem('t_exp');
        localStorage.removeItem('user_role');

        this.router.navigate(['/'])
    });
  }
}
