import { AccountReportDto } from "../AccountReportDto";

export interface IncomeReportDto{
    incomeAccountReports : AccountReportDto[];
    totalSum : number;
    currency : number;
}