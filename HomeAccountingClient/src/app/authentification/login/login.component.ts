import { HttpErrorResponse, HttpRequest } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { AuthResponseDto } from 'src/app/_interfaces/AuthResponseDto';
import { UserForAuthenticationDto } from 'src/app/_interfaces/userForAuthenticationDto';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  private returnUrl: string;
  loginForm: FormGroup;
  errorMessage: string = '';
  showError: boolean;

  constructor(private authService: AuthenticationService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      username: new FormControl("", [Validators.required]),
      password: new FormControl("", [Validators.required])
    })
    this.returnUrl = '/reports';
  }

  validateControl = (controlName: string) => {
    return this.loginForm.get(controlName).invalid && this.loginForm!.get(controlName).touched
  }

  hasError = (controlName: string, errorName: string) => {
    return this.loginForm.get(controlName).hasError(errorName)
  }

  loginUser = (loginFormValue : any) => {
    this.showError = false;
    const login = {... loginFormValue };
    const userForAuth: UserForAuthenticationDto = {
      email: login.username,
      password: login.password
    }


    this.authService.loginUser('api/users-accounts/login', userForAuth)
    .subscribe((response : AuthResponseDto) => {
      localStorage.setItem('token', response.token)
      this.authService.sendAuthStateChangeNotification(response.isAuthSuccessful);
      this.router.navigate([this.returnUrl])
    }, error => {
      console.log(error)
      this.errorMessage = error;
      this.showError = true;
    })

}
}
