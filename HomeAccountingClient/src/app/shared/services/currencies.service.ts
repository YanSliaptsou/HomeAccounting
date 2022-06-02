import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Currency } from 'src/app/_interfaces/currency';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CurrenciesService {

  private baseUrl = environment.urlAddress

  
  constructor(private http: HttpClient) { }

  public getCurrencies (route: string){
    return this.http.get<Currency[]>(this.createCompleteRoute(route, this.baseUrl));
  }

  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }
}
