import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ReportService } from 'src/app/shared/services/report.service';
import { IncomeReportDto } from 'src/app/_interfaces/Report/Income/IncomeReportDto';
import {Chart} from 'chart.js'

@Component({
  selector: 'app-income-report',
  templateUrl: './income-report.component.html',
  styleUrls: ['./income-report.component.css'],
  providers: [DatePipe]
})
export class IncomeReportComponent implements OnInit {

  constructor(private reportService : ReportService, public datepipe: DatePipe) { }

  incomeReport : IncomeReportDto = {
    currency : null,
    incomeAccountReports : null,
    totalSum : null
  };
  dateFrom : Date;
  dateTo : Date;
  reportForm : FormGroup;

  ngOnInit(): void {
    this.reportForm = this.reportService.initReportForm();
  }

  getIncomeReport(report : any){
    var reportForm = {... report};
    this.dateFrom = reportForm.dateFrom;
    this.dateTo = reportForm.dateTo;

    let dateFrom = this.datepipe.transform(this.dateFrom, 'YYYY-MM-dd') +'T00:00:00.179'; //this.datepipe.transform(this.dateFrom, 'yyyy-mm-dd');
    let dateTo = this.datepipe.transform(this.dateTo, 'YYYY-MM-dd') + 'T23:59:59.179';


    this.reportService.getIncome(dateFrom, dateTo).subscribe((response : any) => {
      this.incomeReport = response.data;
    }, error => {

    })
  }
}
