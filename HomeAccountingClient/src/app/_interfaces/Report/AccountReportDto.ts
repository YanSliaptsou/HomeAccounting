export interface AccountReportDto{
    accountName : string;
    sumInLocalCurrency : number;
    localCurrencyCode : string
    sumInUsersCurrency : number;
    usersCurrencyCode : string;
    percentage : number;
}