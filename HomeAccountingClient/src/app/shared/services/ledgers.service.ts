import { query } from '@angular/animations';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { LedgerResponseDto } from 'src/app/_interfaces/Legder/LedgerResponseDto';
import { LedgerSendDto } from 'src/app/_interfaces/Legder/LegderSendDto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LedgersService {

  private ledgersUrl = environment.urlAddress + "/api/ledgers";
  private ADD_OPTION = 'ADD';
  private EDIT_OPTION = 'EDIT';

  constructor(private http : HttpClient) { }

  getLedgers(type : number = null, accountFromId : number = null, accountToId : number = null) : Observable<LedgerResponseDto[]>{

    var queryString = this.buildQueryString(type,accountFromId,accountToId);

    return this.http.get<LedgerResponseDto[]>(this.ledgersUrl + queryString)
    .pipe(
      catchError(this.handleError<LedgerResponseDto[]>('getLedgers', []))
    );
  }

  addLedger(legder : LedgerSendDto) : Observable<any> {
    return this.http.post<any>(this.ledgersUrl, legder)
    .pipe(
      catchError(this.handleError<any>('addLegder'))
    )
  }

  editLedger(id : number, ledger : LedgerSendDto) : Observable<any>{
    return this.http.put<any>(this.ledgersUrl + '/' + id, ledger)
      .pipe(
        catchError(this.handleError<any>('editLedger'))
      )
  }

  deleteLedger(id : number) : Observable<any> {
    return this.http.delete<any>(this.ledgersUrl + '/' + id)
      .pipe(
        catchError(this.handleError<any>('deleteLedger'))
      )
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      return of(result as T);
    };
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

      }), error => {

      })
    }
    else if (option == this.EDIT_OPTION){
      this.editLedger(ledgerIdToUpdate, ledger).subscribe((response => {

      }), error => {
        
      })
    }
  }
}
