export interface AccountReceiveDto{
    id : number;
    transactionCategoryId? : number;
    name : string;
    type : string;
    currencyId : string;
    appUserId : string;
}