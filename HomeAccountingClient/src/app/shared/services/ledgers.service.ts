import { query } from '@angular/animations';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { AccountReceiveDto } from 'src/app/_interfaces/Account/AccountReceiveDto';
import { LedgerResponseDto } from 'src/app/_interfaces/Legder/LedgerResponseDto';
import { LedgerSendDto } from 'src/app/_interfaces/Legder/LegderSendDto';
import { environment } from 'src/environments/environment';
import { ExchangeRatesService } from './exchange-rates.service';

@Injectable({
  providedIn: 'root'
})
export class LedgersService {

  private ledgersUrl = environment.urlAddress + "/api/ledgers";
  private ADD_OPTION = 'ADD';
  private EDIT_OPTION = 'EDIT';
  private ACCOUNT_FROM = 'accountFrom';
  private ACCOUNT_TO = 'accountTo';
  private AMMOUNT_FROM = 'ammountFrom';
  private AMMOUNT_TO = 'ammountTo';
  private DATETIME = 'dateTime';

  constructor(private http : HttpClient) { }

  getLedgers(type : number = null, accountFromId : number = null, accountToId : number = null) : Observable<LedgerResponseDto[]>{

    var queryString = this.buildQueryString(type,accountFromId,accountToId);

    return this.http.get<LedgerResponseDto[]>(this.ledgersUrl + queryString);
  }

   getLedger(id : number) : Observable<LedgerResponseDto>{
    return this.http.get<LedgerResponseDto>(this.ledgersUrl + '/' + id);
  }

   addLedger(legder : LedgerSendDto) : Observable<any> {
    return this.http.post<any>(this.ledgersUrl, legder);
  }

   editLedger(id : number, ledger : LedgerSendDto) : Observable<any>{
    return this.http.put<any>(this.ledgersUrl + '/' + id, ledger)
  }

   deleteLedger(id : number) : Observable<any> {
    console.log(id);
    return this.http.delete<any>(this.ledgersUrl + '/' + id);
  }

  private buildQueryString(type : number = null, accountFromId : number = null, accountToId : number = null) : string {
    
    var finalString = ""

    if (type !== null){
      finalString += "?type=" + type
    }

    if (accountFromId !== null){
      finalString += "&accountFromId=" + accountFromId
    }

    if (accountToId !== null){
      finalString += "&accountToId=" + accountToId
    }

    return finalString;

  }

  initLedgerForm() : FormGroup{
    var ledgerForm = new FormGroup({
      type : new FormControl("", [Validators.required]),
      accountFrom : new FormControl("",[Validators.required]),
      ammountFrom : new FormControl("", [Validators.required]),
      accountTo : new FormControl("", [Validators.required]),
      ammountTo : new FormControl("", [Validators.required]),
      dateTime : new FormControl("", [Validators.required])
    })

    ledgerForm.get(this.ACCOUNT_FROM).disable();
    ledgerForm.get(this.AMMOUNT_FROM).disable();
    ledgerForm.get(this.ACCOUNT_TO).disable();
    ledgerForm.get(this.AMMOUNT_TO).disable();
    ledgerForm.get(this.DATETIME).disable();


    return ledgerForm;
  }

  sendForm(ledgers : any, ledgerIdToUpdate : number, option : string){

    var ledgersForm ={... ledgers}
    let ledger : LedgerSendDto = {
      type : ledgersForm.type,
      accountFromId : ledgersForm.accountFrom,
      accountToId : ledgersForm.accountTo,
      ammountFrom : ledgersForm.ammountFrom,
      ammountTo : ledgersForm.ammountTo,
      dateTime : ledgersForm.dateTime,
    }

    if (option === this.ADD_OPTION){
      this.addLedger(ledger).subscribe((response => {
        return response;
      }), error => {
        return error;
      })
    }
    else if (option == this.EDIT_OPTION){
      this.editLedger(ledgerIdToUpdate, ledger).subscribe((response => {
        return response
      }), error => {
        return error
      })
    }
  }

  removeLedger(id : number){
    console.log(id);
    this.deleteLedger(id).subscribe((response => {
      return response
    }), error => {
      return error;
    })
  }

  resolveTransType(ledger: LedgerResponseDto){
    if (ledger.type === 0){
      ledger.typeString = 'Debet';
    }
    else if (ledger.type === 1){
      ledger.typeString = 'Credit';
    }
  }

  enableForm(form : FormGroup, transactType : string){
    if (transactType == 'Outcome'){
      form.get(this.AMMOUNT_FROM).disable();
      form.get(this.ACCOUNT_TO).disable();
      form.get(this.AMMOUNT_TO).disable();
      form.get(this.DATETIME).disable();

      form.get(this.ACCOUNT_FROM).enable();
    }
    else if (transactType == 'Income'){

      form.get(this.ACCOUNT_FROM).disable();
      form.get(this.AMMOUNT_FROM).disable();

      form.get(this.ACCOUNT_TO).enable();
      form.get(this.AMMOUNT_TO).enable();
      form.get(this.DATETIME).enable();
    }
  }

  accountFromSelect(form : FormGroup){
    form.get(this.ACCOUNT_TO).enable();
  }

  accountToSelect(form : FormGroup, type : number){
    console.log(type);
    if (type == 1){
      form.get(this.AMMOUNT_FROM).enable();
    }
    form.get(this.AMMOUNT_TO).enable();
    form.get(this.DATETIME).enable();
  }
}
