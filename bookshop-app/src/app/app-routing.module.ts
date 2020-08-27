import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login-component/login-component';
import { BooksComponent } from './books/books.component';
import { OrdersComponent } from './orders/orders.component';
import { AuthGuard } from './guards/authGuard';


const routes: Routes = [
  { path: '', component: LoginComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
        { path: 'books', component: BooksComponent },        
        { path: 'orders', component: OrdersComponent }
    ]
},
{ path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
