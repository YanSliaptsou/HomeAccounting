import { HttpClient } from '@angular/common/http';
import { AfterContentChecked, Component, OnInit, TemplateRef } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AccountService } from '../shared/services/account.service';
import { CurrenciesService } from '../shared/services/currencies.service';
import { ExchangeRatesService } from '../shared/services/exchange-rates.service';
import { LedgersService } from '../shared/services/ledgers.service';
import { AccountReceiveDto } from '../_interfaces/Account/AccountReceiveDto';
import { AccountSendDto } from '../_interfaces/Account/AccountSendDto';
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
    private http : HttpClient) { }
  

  ledgers : LedgerResponseDto[];
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
  incomeAccounts : AccountReceiveDto[];
  outcomeAccounts : AccountReceiveDto[];
  placeholder : number = 1;
  currencyFrom : string;
  currencyTo : string;

  ngOnInit(): void {
    this.loadLedgers();
    this.ledgersForm = this.ledgService.initLedgerForm()
    console.log(this.ledgersForm.valid)
    this.http.get<any>('https://localhost:5001/api/reports/outcome?dateFrom=2022-06-01T09:23:07.179&dateTo=2022-06-30T09:23:07.179')
    .subscribe((resp) => {
      console.log(resp);
    })
  }

  loadLedgers(){
    this.ledgService.getLedgers().subscribe((response : any) => {
      this.ledgers = response.data;
      console.log(this.ledgers);
      for(let ledger of this.ledgers){
        this.ledgService.resolveTransType(ledger);
      }
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
      this.loadLedgers();
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
      this.loadLedgers();
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
      this.loadLedgers();
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
