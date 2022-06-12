import { AfterContentChecked, Component, OnInit, TemplateRef } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AccountService } from '../shared/services/account.service';
import { CurrenciesService } from '../shared/services/currencies.service';
import { LedgersService } from '../shared/services/ledgers.service';
import { AccountReceiveDto } from '../_interfaces/Account/AccountReceiveDto';
import { AccountSendDto } from '../_interfaces/Account/AccountSendDto';
import { LedgerResponseDto } from '../_interfaces/Legder/LedgerResponseDto';

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

  constructor(public ledgService : LedgersService, private accountService : AccountService, private modalService : BsModalService) { }
  

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

  ngOnInit(): void {
    this.loadLedgers();
    this.loadIncomeAccounts();
    this.loadOutcomeAccounts();
    this.ledgersForm = this.ledgService.initLedgerForm()
    console.log(this.ledgersForm.valid)
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

  loadIncomeAccounts(){
    this.accountService.getAccounts(this.INCOME_ACCOUNTS_API).subscribe(response => {
      this.incomeAccounts = response;
    })
  }

  loadOutcomeAccounts(){
    this.accountService.getAccounts(this.OUTCOME_ACCOUNTS_API).subscribe(response => {
      this.outcomeAccounts = response;
    })
  }

  addLedger(form : any){
    this.isEditing = false;
    this.isAdding = true;
    this.ledgService.sendForm(form, null, 'ADD');
    this.loadLedgers();
  }

  editLedger(form : any, ledgerId : number){
    this.isEditing = false;
    this.isAdding = true;
    this.ledgService.sendForm(form, ledgerId, 'EDIT');
    this.loadLedgers();
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
    this.ledgService.removeLedger(id);
    this.modalRef?.hide();
    this.loadLedgers();
  }

  decline(): void {
    this.modalRef?.hide();
  }

  openModalOnDelete(template: TemplateRef<any>, id : number) {
    this.modalRef = this.modalService.show(template, {class: 'modal-lg'});
    this.getLedgerToDelete(id);
  }
}
