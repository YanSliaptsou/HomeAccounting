import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LimitReceiveDto } from 'src/app/_interfaces/Limit/LimitReceiveDto';
import { LimitSendDto } from 'src/app/_interfaces/Limit/LimitSendDto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LimitService {

  private baseUrl = environment.urlAddress;
  constructor(private http: HttpClient) { }

  getLimits(route : string){
    return this.http.get<LimitReceiveDto[]>(this.createCompleteRoute(route, this.baseUrl));
  }

  getConcreteLimit(route : string){
    return this.http.get<LimitReceiveDto>(this.createCompleteRoute(route, this.baseUrl));
  }

  addLimit(route: string, limit : LimitSendDto){
    return this.http.post<any>(this.createCompleteRoute(route, this.baseUrl), limit);
  }

  editLimit(route : string, limit: LimitSendDto){
    return this.http.put<any>(this.createCompleteRoute(route, this.baseUrl), limit);
  }

  deleteLimit(route : string){
    return this.http.delete<any>(this.createCompleteRoute(route, this.baseUrl));
  }

  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }
}
