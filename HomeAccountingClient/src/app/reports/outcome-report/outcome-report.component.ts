import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ChartService } from 'src/app/shared/services/chart.service';
import { DateEnum, DateTimeService } from 'src/app/shared/services/date-time.service';
import { ReportService } from 'src/app/shared/services/report.service';
import { OutcomeReportDto } from 'src/app/_interfaces/Report/Outcome/OutcomeReportDto';
import { ReportChartDataType } from 'src/app/_interfaces/Report/ReportChartData';

@Component({
  selector: 'app-outcome-report',
  templateUrl: './outcome-report.component.html',
  styleUrls: ['./outcome-report.component.css'],
  providers: [DatePipe]
})
export class OutcomeReportComponent implements OnInit {

  constructor(private reportService : ReportService, public datepipe: DatePipe, private chartService : ChartService, private dateService : DateTimeService) { }

  outcomeReport : OutcomeReportDto = {
    currency : null,
    categoriesReport : null,
    totalSum : null
  };

  dateFrom : Date;
  dateTo : Date;
  reportForm : FormGroup;
  isError = false;
  errorMessage = "";

  ngOnInit(): void {
    this.reportForm = this.reportService.initReportForm();
  }

  getOutcomeReport(report : any){
    this.isError = false;
    var reportForm = {... report};
    this.dateFrom = reportForm.dateFrom;
    this.dateTo = reportForm.dateTo;

    let dateFrom = this.dateService.convertDate(this.dateFrom, DateEnum.DateFrom) 
    let dateTo = this.dateService.convertDate(this.dateTo, DateEnum.DateTo)


    this.reportService.getOutcome(dateFrom, dateTo).subscribe((response : any) => {
      this.outcomeReport = response.data;
      this.chartService.buildChart(ReportChartDataType.OutcomeByAccounts,null,this.outcomeReport,this.outcomeReport.currency);
      this.chartService.buildChart(ReportChartDataType.OutcomeByCategories,null,this.outcomeReport, this.outcomeReport.currency);
    }, error => {
      this.isError = true;
      this.errorMessage = error.errorMessage;
    })
  }

}
