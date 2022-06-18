import { HttpClient } from '@angular/common/http';
import { AfterContentChecked, Component, OnInit, TemplateRef } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AccountService } from '../shared/services/account.service';
import { DateEnum, DateTimeService } from '../shared/services/date-time.service';
import { ExchangeRatesService } from '../shared/services/exchange-rates.service';
import { LedgersService } from '../shared/services/ledgers.service';
import { AccountReceiveDto } from '../_interfaces/Account/AccountReceiveDto';
import { CurrencyExchangeRate } from '../_interfaces/CurrencyExchangeRate';
import { LedgerResponseDto } from '../_interfaces/Legder/LedgerResponseDto';
import { LedgerSendDto } from '../_interfaces/Legder/LegderSendDto';

@Component({
  selector: 'app-ledgers',
  templateUrl: './ledgers.component.html',
  styleUrls: ['./ledgers.component.css'],
  providers: []
})
export class LedgersComponent implements OnInit {

  private INCOME_ACCOUNTS_API = "api/accounts/Income";
  private OUTCOME_ACCOUNTS_API = "api/accounts/Outcome";

  private ACCOUNT_FROM = 'accountFrom';
  private ACCOUNT_TO = 'accountTo';
  private AMMOUNT_FROM = 'ammountFrom';
  private AMMOUNT_TO = 'ammountTo';
  private DATETIME = 'dateTime';

  isAdding : boolean = true;
  isEditing : boolean = false;
  modalRef?: BsModalRef;

  accountFromId? : number;
  accountToId : number;
  ammountFrom? : number;
  ammountTo : number;
  type : number;
  dateTime : Date;

  currencyExchangeRate : CurrencyExchangeRate;

  constructor(public ledgService : LedgersService, 
    private accountService : AccountService, 
    private modalService : BsModalService,
    private exchRateServ : ExchangeRatesService,
    private dtService : DateTimeService) { }
  

  ledgers : LedgerResponseDto[] = null;
  currentLedger : LedgerResponseDto = {
    accountNameFrom : null,
    ammountFrom : null,
    ammountTo : null,
    currencyFrom : null,
    currencyTo : null,
    dateTime : null,
    id : null,
    type : null,
    accountNameTo : null,
    typeString : null
  };
  ledgersForm : FormGroup;
  ledgersReportForm : FormGroup;
  incomeAccounts : AccountReceiveDto[];
  outcomeAccounts : AccountReceiveDto[];
  placeholder : number = 1;
  currencyFrom : string;
  currencyTo : string;

  accountId : number;
  dateFrom : Date;
  dateTo : Date;
  accountsForReport : AccountReceiveDto[];

  isError = false;
  errorMessage = "";

  ngOnInit(): void {
    this.ledgersForm = this.ledgService.initLedgerForm();
    this.ledgersReportForm = this.ledgService.initLedgerReportForm();
    this.loadAccountsForReport();
  }

  loadLedgers(ledgerForm : any, accId? : number, dtFrom? : Date, dtTo? : Date){

    this.isError = false;
    if (ledgerForm !== null){
    var ledgerReportForm = {... ledgerForm}

    this.accountId = ledgerReportForm.accountId;
    this.dateFrom = ledgerForm.dateFrom;
    this.dateTo = ledgerForm.dateTo;
    }
    else
    {
      this.accountId = accId;
      this.dateFrom = dtFrom;
      this.dateTo = dtTo;
    }

    let dateFrom = this.dtService.convertDate(this.dateFrom, DateEnum.DateFrom);
    let dateTo = this.dtService.convertDate(this.dateTo, DateEnum.DateTo);

    this.ledgService.getLedgers(this.accountId, dateFrom, dateTo).subscribe((response : any) => {
      this.ledgers = response.data;
      console.log(this.ledgers);
      for(let ledger of this.ledgers){
        this.ledgService.resolveTransType(ledger);
      }
    }, error => {
      this.isError = true;
      this.errorMessage = error.errorMessage;
    })
  }

  func(){
    var valueOfStr = this.ledgersForm.get(this.AMMOUNT_FROM).value as number;
    this.exchRateServ.getCurrencyExchangeRate(this.currencyFrom, this.currencyTo).subscribe((response : any) => {
      this.currencyExchangeRate = response.data;
      console.log(this.currencyExchangeRate);
      this.placeholder = valueOfStr * this.currencyExchangeRate.exchangeRateValue as number;
    })
  }

  loadAccountsForReport(){
    this.accountService.getAccounts(this.INCOME_ACCOUNTS_API).subscribe(response => {
      this.accountsForReport = response;
    })
  }

  loadIncomeAccounts(type : number){
    this.accountService.getAccounts(this.INCOME_ACCOUNTS_API).subscribe(response => {
      if (type == 0){
        this.incomeAccounts = response;
      }
      else{
        this.outcomeAccounts = response;
      }
    })
  }

  loadOutcomeAccounts(type : number){
    this.accountService.getAccounts(this.OUTCOME_ACCOUNTS_API).subscribe(response => {
      if (type == 0){
        this.outcomeAccounts = response;
      }
      else{
        this.incomeAccounts = response;
      }
    })
  }

  addLedger(form : any){
    this.isEditing = false;
    this.isAdding = true;
    var ledgersForm ={... form}
    let ledger : LedgerSendDto = {
      type : ledgersForm.type,
      accountFromId : ledgersForm.accountFrom,
      accountToId : ledgersForm.accountTo,
      ammountFrom : ledgersForm.ammountFrom,
      ammountTo : ledgersForm.ammountTo,
      dateTime : ledgersForm.dateTime,
    }
    this.ledgService.addLedger(ledger).subscribe((response => {

    }), error => {

    })
  }

  editLedger(form : any, ledgerId : number){
    this.isEditing = false;
    this.isAdding = true;
    var ledgersForm ={... form}
    let ledger : LedgerSendDto = {
      type : ledgersForm.type,
      accountFromId : ledgersForm.accountFrom,
      accountToId : ledgersForm.accountTo,
      ammountFrom : ledgersForm.ammountFrom,
      ammountTo : ledgersForm.ammountTo,
      dateTime : ledgersForm.dateTime,
    }
    this.ledgService.editLedger(ledgerId, ledger).subscribe((response => {
      this.loadLedgers(null,this.accountId, this.dateFrom, this.dateTo);
      }), error => {

      })
  }

  getLedger(id : number){
    this.isEditing = true;
    this.isAdding = false;
    this.ledgService.getLedger(id).subscribe((response : any) => {
      this.currentLedger = response.data;

      this.accountFromId = response.data.accountFromId;
      this.accountToId = response.data.accountToId;
      this.ammountFrom = response.data.ammountFrom;
      this.ammountTo = response.data.ammountTo;
      this.dateTime = response.data.dateTime;
      this.type = response.data.type;

      console.log(this.currentLedger);
    })
  }

  getLedgerToDelete(id : number){
    this.ledgService.getLedger(id).subscribe((response : any) => {
      this.currentLedger = response.data;
    });
  }

  deleteLedger(id : number){
    console.log(id);
    this.ledgService.deleteLedger(id).subscribe((response => {
      this.loadLedgers(null,this.accountId, this.dateFrom, this.dateTo);
    }), error => {
    })
    this.modalRef?.hide();
  }

  decline(): void {
    this.modalRef?.hide();
  }

  openModalOnDelete(template: TemplateRef<any>, id : number) {
    this.modalRef = this.modalService.show(template, {class: 'modal-lg'});
    this.getLedgerToDelete(id);
  }

  getCurrencyExchRate(currencyFrom : string, currencyTo : string){
    this.exchRateServ.getCurrencyExchangeRate(currencyFrom, currencyTo).subscribe(response => {
      this.currencyExchangeRate = response;
    })
  }
}
