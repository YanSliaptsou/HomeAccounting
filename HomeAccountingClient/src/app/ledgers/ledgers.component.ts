import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { AccountService } from '../shared/services/account.service';
import { CurrenciesService } from '../shared/services/currencies.service';
import { LedgersService } from '../shared/services/ledgers.service';
import { AccountReceiveDto } from '../_interfaces/Account/AccountReceiveDto';
import { AccountSendDto } from '../_interfaces/Account/AccountSendDto';
import { LedgerResponseDto } from '../_interfaces/Legder/LedgerResponseDto';

@Component({
  selector: 'app-ledgers',
  templateUrl: './ledgers.component.html',
  styleUrls: ['./ledgers.component.css']
})
export class LedgersComponent implements OnInit {

  private INCOME_ACCOUNTS_API = "api/accounts/Income";
  private OUTCOME_ACCOUNTS_API = "api/accounts/Outcome";

  constructor(private ledgService : LedgersService, private accountService : AccountService) { }

  ledgers : LedgerResponseDto[];
  ledgersForm : FormGroup;
  incomeAccounts : AccountReceiveDto[];
  outcomeAccounts : AccountReceiveDto[];

  ngOnInit(): void {
    this.loadLedgers();
    this.loadIncomeAccounts();
    this.loadOutcomeAccounts();
    this.ledgersForm = this.ledgService.initLedgerForm()
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

}
