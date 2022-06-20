export interface CategoryReceive{
    id : number;
    name : string;
    parentTransactionCategoryId : number;
    userId? : string;
    parentTransactionCategoryName? : string;
}