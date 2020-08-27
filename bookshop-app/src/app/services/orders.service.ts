import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Order } from '../models/order';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  private baseUrl = environment.apiUrl + 'orders/';
  
  constructor(private authService: AuthService, private http: HttpClient) { }

  async getOrders() : Promise<Observable<Order[]>>{
    var isTokenValid = this.authService.checkIsTokenValid();

    if (!isTokenValid){
      await this.authService.refreshUserToken();
    } 

    return await this.http.get<Observable<Order[]>>(this.baseUrl, {withCredentials : true}).toPromise();
  }
}
