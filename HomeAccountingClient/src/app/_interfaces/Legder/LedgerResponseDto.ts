export interface LedgerResponseDto{
    id : number,
    accountNameFrom : string;
    accountNameTo? : string;
    ammountFrom : number;
    ammountTo : number;
    type : number;
    dateTime : Date;
    currencyFrom : string;
    currencyTo : string;
    typeString?: string;
}