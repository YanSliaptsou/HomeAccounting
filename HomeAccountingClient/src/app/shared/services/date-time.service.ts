import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DateTimeService {

  timeFromTemplate = 'T00:00:00.179';
  timeToTemplate = 'T23:59:59.179';
  dateTemplate = 'YYYY-MM-dd';

  constructor(public datepipe: DatePipe) { }

  convertDate(date : Date, dateType : DateEnum) : string{
    switch(dateType){
      case DateEnum.DateFrom : {
        return this.datepipe.transform(date, this.dateTemplate) + this.timeFromTemplate;
      }
      case DateEnum.DateTo : {
        return this.datepipe.transform(date, this.dateTemplate) + this.timeToTemplate;
      }
    }
  }
}

export enum DateEnum{
  DateFrom,
  DateTo
}
