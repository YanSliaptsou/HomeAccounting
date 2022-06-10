import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CategoryReceive } from 'src/app/_interfaces/Category/CategoryReceiveDto';
import { CategorySendDto } from 'src/app/_interfaces/Category/CategorySendDto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  private baseUrl = environment.urlAddress;
  constructor(private http: HttpClient) { }

  getCategories(route : string){
    return this.http.get<CategoryReceive[]>(this.createCompleteRoute(route, this.baseUrl));
  }

  getConcreteCategory(route: string){
    return this.http.get<CategoryReceive>(this.createCompleteRoute(route, this.baseUrl));
  }

  addCategory(route: string, category : CategorySendDto){
    return this.http.post<any>(this.createCompleteRoute(route, this.baseUrl), category);
  }

  editCategory(route: string, category: CategorySendDto){
    return this.http.put<any>(this.createCompleteRoute(route, this.baseUrl), category);
  }

  deleteCategory(route: string){
    return this.http.delete<any>(this.createCompleteRoute(route, this.baseUrl));
  }

  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }
}
