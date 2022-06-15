import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { IncomeReportDto } from 'src/app/_interfaces/Report/Income/IncomeReportDto';
import { OutcomeReportDto } from 'src/app/_interfaces/Report/Outcome/OutcomeReportDto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  private reportUrl = environment.urlAddress + "/api/reports";
  constructor(private http : HttpClient) { }

  getIncome(dateFrom : string, dateTo : string) : Observable<IncomeReportDto>{
    return this.http.get<IncomeReportDto>(this.reportUrl + '/income' + this.buildQueryString(dateFrom,dateTo));
  }

  getOutcome(dateFrom : string, dateTo : string) : Observable<OutcomeReportDto>{
    return this.http.get<OutcomeReportDto>(this.reportUrl + '/outcome' + this.buildQueryString(dateFrom, dateTo));
  }

  buildQueryString(dateFrom : string, dateTo : string) : string{
    return '?dateFrom=' + dateFrom + '&dateTo=' + dateTo;
  }

  initReportForm() : FormGroup{
    var reportForm = new FormGroup({
      dateFrom : new FormControl("", [Validators.required]),
      dateTo : new FormControl("", [Validators.required])
    })

    return reportForm;
  }
}
