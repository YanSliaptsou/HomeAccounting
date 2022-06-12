export interface LedgerSendDto{
    accountFromId? : number;
    accountToId : number;
    ammountFrom? : number;
    ammountTo : number;
    type : number;
    dateTime : Date;
}