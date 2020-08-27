import { Component, OnInit } from '@angular/core';
import { Order } from '../models/order';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { OrdersService } from '../services/orders.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html'
})
export class OrdersComponent implements OnInit {
  orders: Order[] = [];
  
  constructor(private ordersService: OrdersService) 
  {

  }

  ngOnInit() {
    this.ordersService.getOrders().then(result => 
      result.forEach(x => this.orders = this.orders.concat(x)));
  }

}