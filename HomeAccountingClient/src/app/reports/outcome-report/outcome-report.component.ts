import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ChartService } from 'src/app/shared/services/chart.service';
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

  constructor(private reportService : ReportService, public datepipe: DatePipe, private chartService : ChartService) { }

  outcomeReport : OutcomeReportDto = {
    currency : null,
    categoriesReport : null,
    totalSum : null
  };

  dateFrom : Date;
  dateTo : Date;
  reportForm : FormGroup;

  ngOnInit(): void {
    this.reportForm = this.reportService.initReportForm();
  }

  getOutcomeReport(report : any){
    var reportForm = {... report};
    this.dateFrom = reportForm.dateFrom;
    this.dateTo = reportForm.dateTo;

    let dateFrom = this.datepipe.transform(this.dateFrom, 'YYYY-MM-dd') +'T00:00:00.179'; //this.datepipe.transform(this.dateFrom, 'yyyy-mm-dd');
    let dateTo = this.datepipe.transform(this.dateTo, 'YYYY-MM-dd') + 'T23:59:59.179';


    this.reportService.getOutcome(dateFrom, dateTo).subscribe((response : any) => {
      this.outcomeReport = response.data;
      this.chartService.buildChart(ReportChartDataType.OutcomeByAccounts,null,this.outcomeReport,this.outcomeReport.currency);
      this.chartService.buildChart(ReportChartDataType.OutcomeByCategories,null,this.outcomeReport, this.outcomeReport.currency);
    }, error => {

    })
  }

}
