import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AccountEditDto } from 'src/app/_interfaces/Account/AccountEditDto';
import { AccountReceiveDto } from 'src/app/_interfaces/Account/AccountReceiveDto';
import { AccountSendDto } from 'src/app/_interfaces/Account/AccountSendDto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private baseUrl = environment.urlAddress;
  constructor(private http: HttpClient) { }

  getAccounts(route : string){
    return this.http.get<AccountReceiveDto[]>(this.createCompleteRoute(route, this.baseUrl));
  }

  getConcreteAccount(route : string){
    return this.http.get<AccountReceiveDto>(this.createCompleteRoute(route, this.baseUrl));
  }

  addAccount(route: string, account : AccountSendDto){
    return this.http.post<any>(this.createCompleteRoute(route, this.baseUrl), account);
  }

  editAccount(route: string, account : AccountEditDto){
    return this.http.put<any>(this.createCompleteRoute(route, this.baseUrl), account);
  }

  deleteAccount(route: string){
    return this.http.delete<any>(this.createCompleteRoute(route, this.baseUrl));
  }

  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }
}
