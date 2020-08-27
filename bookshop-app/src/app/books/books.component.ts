import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Book } from '../models/book';
import { BooksService } from '../services/books.service';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-books',
  templateUrl: './books.component.html'
})
export class BooksComponent implements OnInit {
  books: Book[] = [];

  constructor(private booksService: BooksService) { }

  ngOnInit() {
    this.booksService.getBooks().then(result => {
      result.forEach(x => this.books = this.books.concat(x)); 
    })
  }

}
