import { BrowserModule } from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';
import { RouterModule } from '@angular/router';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DeviceDetectorModule } from 'ngx-device-detector';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login-component/login-component'
import { AlertifyService } from './services/alertify.service'
import { AuthService } from './services/auth.service';
import { AppNavComponent } from './app-nav/app-nav.component';
import { BooksComponent } from './books/books.component';
import { OrdersComponent } from './orders/orders.component';
import { AuthGuard } from './guards/authGuard';
import { BooksService } from './services/books.service';
import { OrdersService } from './services/orders.service';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    AppNavComponent,
    BooksComponent,
    OrdersComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    BrowserAnimationsModule,
    BsDropdownModule.forRoot(),
    DeviceDetectorModule.forRoot(),
    AppRoutingModule,
    HttpClientModule,
    JwtModule.forRoot({
      config :{ 
      }
    }),
  ],
  providers: [
    AlertifyService,
    AuthService,
    AuthGuard,
    BooksService,
    OrdersService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
