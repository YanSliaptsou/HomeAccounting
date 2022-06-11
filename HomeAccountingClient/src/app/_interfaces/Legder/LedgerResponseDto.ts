export interface LedgerResponseDto{
    /*public int Id { get; set; }
    public string AccountNameFrom { get; set; }
    public string AccountNameTo { get; set; }
    public decimal? AmmountFrom { get; set; }
    public decimal AmmountTo { get; set; }
    public LedgerType Type { get; set; }
    public DateTime DateTime { get; set; }*/

    id : number,
    accountNameFrom : string;
    accountNameTo? : string;
    ammountFrom : number;
    ammountTo : number;
    type : number;
    dateTime : Date;
}