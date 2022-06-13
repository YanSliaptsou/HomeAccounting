export interface LimitReceiveDto{
    id : number;
    accountId : number;
    limit : number;
    limitFrom : Date;
    limitTo : Date;
    totalSpend : number,
    percentage : number;
}