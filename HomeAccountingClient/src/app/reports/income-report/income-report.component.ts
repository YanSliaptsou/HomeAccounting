import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ReportService } from 'src/app/shared/services/report.service';
import { IncomeReportDto } from 'src/app/_interfaces/Report/Income/IncomeReportDto';
import {Chart} from 'chart.js'
import { ReportChartDataType } from 'src/app/_interfaces/Report/ReportChartData';
import { ChartService } from 'src/app/shared/services/chart.service';
import { OutcomeReportDto } from 'src/app/_interfaces/Report/Outcome/OutcomeReportDto';
import { DateEnum, DateTimeService } from 'src/app/shared/services/date-time.service';
declare var google : any;
@Component({
  selector: 'app-income-report',
  templateUrl: './income-report.component.html',
  styleUrls: ['./income-report.component.css'],
  providers: [DatePipe]
})
export class IncomeReportComponent implements OnInit {

  constructor(private reportService : ReportService, public datepipe: DatePipe, private chartService : ChartService,
    private dateService : DateTimeService) { }

  incomeReport : IncomeReportDto = {
    currency : null,
    incomeAccountReports : null,
    totalSum : null
  };
  dateFrom : Date;
  dateTo : Date;
  reportForm : FormGroup;
  isError = false;
  errorMessage = "";

  ngOnInit(): void {
    google.charts.load('current', {packages: ['corechart']});
    this.reportForm = this.reportService.initReportForm();
  }

  getIncomeReport(report : any){
    this.isError = false;
    var reportForm = {... report};
    this.dateFrom = reportForm.dateFrom;
    this.dateTo = reportForm.dateTo;

    let dateFrom = this.dateService.convertDate(this.dateFrom, DateEnum.DateFrom) 
    let dateTo = this.dateService.convertDate(this.dateTo, DateEnum.DateTo)


    this.reportService.getIncome(dateFrom, dateTo).subscribe((response : any) => {
      this.incomeReport = response.data;
      this.chartService.buildChart(ReportChartDataType.IncomeByAccounts,this.incomeReport,null,this.incomeReport.currency)
    }, error => {
      this.isError = true;
      this.errorMessage = error.errorMessage;
    })
  }
}
