import { OutcomeCategoryReportDto } from "./OutcomeCategoryReportDto";

export interface OutcomeReportDto{
    categoriesReport : OutcomeCategoryReportDto[];
    totalSum : number;
    currency: string;
}