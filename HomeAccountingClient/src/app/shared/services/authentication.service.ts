import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthResponseDto } from 'src/app/_interfaces/AuthResponseDto';
import { RegistrationResponseDto } from 'src/app/_interfaces/RegistrationResponseDto';
import { UserForAuthenticationDto } from 'src/app/_interfaces/userForAuthenticationDto';
import { UserForRegistrationDto } from 'src/app/_interfaces/UserForRegistrationDto';
import { environment } from 'src/environments/environment';
import { Observable, ReplaySubject, Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ForgotPasswordDto } from 'src/app/_interfaces/ForgotPassword';
import { ResetPasswordResponseDto } from 'src/app/_interfaces/ResetPasswordResponseDto';
import { ResetPasswordDto } from 'src/app/_interfaces/ResetPasswordDto';
import { CustomEncoder } from '../CustomEncoder';
import { UserResponseDto } from 'src/app/_interfaces/UsersInterfaces/UserResponseDto';
import { UserRequestDto } from 'src/app/_interfaces/UsersInterfaces/UserRequstDto';

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

  public forgotPassword = (route: string, body: ForgotPasswordDto) => {
    return this.http.post<ResetPasswordResponseDto>(this.createCompleteRoute(route, this.baseUrl), body);
  }

  public resetPassword = (route: string, body: ResetPasswordDto) => {
    return this.http.post(this.createCompleteRoute(route, this.baseUrl), body);
  }

  public confirmEmail = (route: string, token: string, email: string) => {
    let params = new HttpParams({ encoder: new CustomEncoder() })
    params = params.append('token', token);
    params = params.append('email', email);
    console.log(params);
    return this.http.post(this.createCompleteRoute(route, this.baseUrl) + "?email=" + email + "&token=" + token, null);
  }

  public getUser() : Observable<UserResponseDto>{
    return this.http.get<UserResponseDto>(this.baseUrl + '/api/users-accounts');
  }

  public editUser(user : UserRequestDto){
    return this.http.post(this.baseUrl + '/api/users-accounts/edit', user);
  }
}
