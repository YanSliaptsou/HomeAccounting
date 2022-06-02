import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegistrationResponseDto } from 'src/app/_interfaces/RegistrationResponseDto';
import { UserForRegistrationDto } from 'src/app/_interfaces/UserForRegistrationDto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private http: HttpClient) { }

  private baseUrl = environment.urlAddress

  public registerUser = (route: string, body: UserForRegistrationDto) => {
    return this.http.post<RegistrationResponseDto> (this.createCompleteRoute(route, this.baseUrl), body);
  }

  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }
}
