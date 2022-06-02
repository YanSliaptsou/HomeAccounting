import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-notfound',
  templateUrl: './notfound.component.html',
  styleUrls: ['./notfound.component.css']
})
export class NotfoundComponent implements OnInit {

  public notFoundText: string = `404 SORRY COULDN'T FIND IT!!!`
  constructor() { }

  ngOnInit(): void {
  }

}
