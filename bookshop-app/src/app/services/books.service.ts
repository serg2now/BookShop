import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Book } from '../models/book';

@Injectable({
  providedIn: 'root'
})
export class BooksService {
  private baseUrl = environment.apiUrl + 'books/';
  
  constructor(private authService: AuthService, private http: HttpClient) { }

  async getBooks() : Promise<Observable<Book[]>>{
    var isTokenValid = this.authService.checkIsTokenValid();

    if (!isTokenValid){
     await this.authService.refreshUserToken();
    } 

    return await this.http.get<Observable<Book[]>>(this.baseUrl, {withCredentials : true}).toPromise();
  }
}
