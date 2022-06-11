export interface LedgerSendDto{
    /*public int? AccountFromId { get; set; }

        [Required(ErrorMessage = "Account id is required")]
        public int AccountToId { get; set; }

        public decimal? AmmountFrom { get; set; }

        [Required(ErrorMessage = "Account id is required")]
        public decimal AmmountTo { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public LedgerType Type { get; set; }
        public DateTime? DateTime { get; set; }*/

    accountFromId? : number;
    accountToId : number;
    ammountFrom? : number;
    ammountTo : number;
    type : number;
    dateTime : Date;
}