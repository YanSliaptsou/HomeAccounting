import { Injectable } from '@angular/core';
import { AccountReportDto } from 'src/app/_interfaces/Report/AccountReportDto';
import { IncomeReportDto } from 'src/app/_interfaces/Report/Income/IncomeReportDto';
import { OutcomeReportDto } from 'src/app/_interfaces/Report/Outcome/OutcomeReportDto';
import { ReportChartData, ReportChartDataType } from 'src/app/_interfaces/Report/ReportChartData';
declare var google : any;

@Injectable({
  providedIn: 'root'
})
export class ChartService {

  constructor() {
    google.charts.load('current', {packages: ['corechart']});
   }

  private getChartData(dataType : ReportChartDataType, incomeReport : IncomeReportDto = null, 
        outcomeReport : OutcomeReportDto = null) : ReportChartData[]{
          let chartData : ReportChartData[] = [];
          switch(dataType){
            case ReportChartDataType.IncomeByAccounts : {
              for (let incomeAccount of incomeReport.incomeAccountReports){
                let ChartDataItem : ReportChartData = {
                  xValue : incomeAccount.accountName,
                  yValue : incomeAccount.sumInUsersCurrency
                }
                chartData.push(ChartDataItem)
              }
              break;
            }
            case ReportChartDataType.OutcomeByAccounts:{
              for (let category of outcomeReport.categoriesReport){
                for (let outcomeAccount of category.outcomeAccountsReports){
                  let ChartDataItem : ReportChartData = {
                    xValue : outcomeAccount.accountName,
                    yValue : outcomeAccount.sumInUsersCurrency
                  }
                  chartData.push(ChartDataItem);
                }
              }
              break;
            }

            case ReportChartDataType.OutcomeByCategories : {
              for (let category of outcomeReport.categoriesReport){
                let ChartDataItem : ReportChartData = {
                  xValue : category.categoryName,
                  yValue : category.totalSum
                }
                chartData.push(ChartDataItem);
              }
            }
            break;
          }

      return chartData;
  }

  public buildChart(dataType : ReportChartDataType, incomeReport : IncomeReportDto = null, 
    outcomeReport : OutcomeReportDto = null, userCurrency : string){
      let chartData = this.getChartData(dataType, incomeReport, outcomeReport);
      console.log(chartData);
      var func = (chart : any) => {
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Name');
        data.addColumn('number', 'Value');
        chartData.forEach(item => {
          data.addRows([
           [item.xValue, item.yValue]
          ]);
        });
  
      var options = {title: this.getChartTitle(dataType, userCurrency),
                       width:800,
                       height:600,
                       pieHole: 0.4,
                      };
      chart().draw(data, options);
      }

      var chart = () => new google.visualization.PieChart(document.getElementById(this.getChartElementId(dataType)));
      var callback = () => func(chart);
      google.charts.setOnLoadCallback(callback);
  }

  private getChartTitle(dataType : ReportChartDataType, userCurrency : string) : string{
    let title = "";
    switch(dataType){
      case ReportChartDataType.IncomeByAccounts: {
        title = "Income by accounts, " + userCurrency
        break;
      }

      case ReportChartDataType.OutcomeByAccounts: {
        title = "Outcome by accounts, " + userCurrency;
        break;
      }

      case ReportChartDataType.OutcomeByCategories : {
        title = "Outcome by categories, " + userCurrency;
        break;
      }
    }

    return title;
  }

  private getChartElementId(dataType : ReportChartDataType) : string{
    let chartElementId = "";

    switch(dataType){
      case ReportChartDataType.IncomeByAccounts:{
        chartElementId = "incomeAccountsChart";
        break;
      }

      case ReportChartDataType.OutcomeByAccounts:{
        chartElementId = "outcomeAccountsChart";
        break;
      }

      case ReportChartDataType.OutcomeByCategories: {
        chartElementId = "outcomeByCategories";
        break;
      }
    }
    return chartElementId;
  }

}
