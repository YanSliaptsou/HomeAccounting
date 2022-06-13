import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CurrencyExchangeRate } from 'src/app/_interfaces/CurrencyExchangeRate';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ExchangeRatesService {

  private ledgersUrl = environment.urlAddress + "/api/exchange-rates";
  private queryParams = ['?currencyFrom=','&currencyTo=']
  constructor(private http : HttpClient) { }

  getCurrencyExchangeRate(currencyFrom : string, currencyTo : string) : Observable<CurrencyExchangeRate>{
    console.log(currencyFrom);
    console.log(currencyTo);
    console.log(this.ledgersUrl + "?currencyFrom=" + currencyFrom + "&currencyTo=" + currencyTo);
    return this.http.get<CurrencyExchangeRate>(this.ledgersUrl + '?currencyFrom=' + currencyFrom + 
    '&currencyTo=' + currencyTo);
  }
}
