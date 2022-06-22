import { AccountReportDto } from "../AccountReportDto";

export interface OutcomeCategoryReportDto{
    categoryName : string;
    totalSum : number;
    currency : string;
    percentage : string;
    outcomeAccountsReports : AccountReportDto[]
}