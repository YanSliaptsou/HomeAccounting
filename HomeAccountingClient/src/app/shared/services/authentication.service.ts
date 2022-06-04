import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthResponseDto } from 'src/app/_interfaces/AuthResponseDto';
import { RegistrationResponseDto } from 'src/app/_interfaces/RegistrationResponseDto';
import { UserForAuthenticationDto } from 'src/app/_interfaces/userForAuthenticationDto';
import { UserForRegistrationDto } from 'src/app/_interfaces/UserForRegistrationDto';
import { environment } from 'src/environments/environment';
import { ReplaySubject, Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private http: HttpClient, private jwtHelper: JwtHelperService) { }

  private baseUrl = environment.urlAddress;
  private authChangeSub = new Subject<boolean>()
  public authChanged = this.authChangeSub.asObservable();

  public registerUser = (route: string, body: UserForRegistrationDto) => {
    return this.http.post<RegistrationResponseDto> (this.createCompleteRoute(route, this.baseUrl), body);
  }

  public loginUser = (route: string, body: UserForAuthenticationDto) => {
    return this.http.post<AuthResponseDto>(this.createCompleteRoute(route, this.baseUrl), body);
  }

  public sendAuthStateChangeNotification = (isAuthenticated : boolean) => {
    this.authChangeSub.next(isAuthenticated);
  }
  
  public logout = () => {
    localStorage.removeItem("token");
    this.sendAuthStateChangeNotification(false);
  }

  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }

  public isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("token");
 
    return token && !this.jwtHelper.isTokenExpired(token);
  }
}
