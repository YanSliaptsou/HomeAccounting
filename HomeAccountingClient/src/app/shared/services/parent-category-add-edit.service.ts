import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ParentCategoryReceive } from 'src/app/_interfaces/ParentCategory/ParentCategoryReceiveDto';
import { ParentCategorySendDto } from 'src/app/_interfaces/ParentCategory/ParentCategorySendDto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ParentCategoryAddEditService {

  private baseUrl = environment.urlAddress;
  constructor(private http: HttpClient) { }

  getParentCategories(route : string){
    return this.http.get<ParentCategoryReceive[]>(this.createCompleteRoute(route, this.baseUrl));
  }

  getConcreteParentCategory(route: string){
    return this.http.get<ParentCategoryReceive>(this.createCompleteRoute(route, this.baseUrl));
  }

  addParentCategory(route: string, parentCategory : ParentCategorySendDto){
    return this.http.post<any>(this.createCompleteRoute(route, this.baseUrl), parentCategory);
  }

  editParentCategory(route: string, parentCategory: ParentCategorySendDto){
    return this.http.put<any>(this.createCompleteRoute(route, this.baseUrl), parentCategory);
  }

  deleteParentCategory(route: string){
    return this.http.delete<any>(this.createCompleteRoute(route, this.baseUrl));
  }

  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }


}
