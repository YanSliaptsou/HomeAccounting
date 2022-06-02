import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { CurrenciesService } from 'src/app/shared/services/currencies.service';
import { Currency } from 'src/app/_interfaces/currency';
import { UserForRegistrationDto } from 'src/app/_interfaces/UserForRegistrationDto';

@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrls: ['./register-user.component.css']
})
export class RegisterUserComponent implements OnInit {

  registerForm : FormGroup;
  currencies: Currency[];

  constructor(private authService: AuthenticationService, private currencyService: CurrenciesService) { }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      userName: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required]),
      mainCurrencyCode: new FormControl(''),
      confirm: new FormControl(''),
      clientURI: new FormControl('')
    })

    this.currencyService.getCurrencies("api/currencies").subscribe((cur: Currency[]) => {
        this.currencies = cur;
    })
  }
  public validateControl = (controlName: string) => {
    return this.registerForm.get(controlName).invalid && this.registerForm!.get(controlName).touched
  }
  public hasError = (controlName: string, errorName: string) => {
    return this.registerForm?.get(controlName).hasError(errorName)
  }

  public registerUser = (registerFormValue : any) => {
    const formValues = { ...registerFormValue };

    const user: UserForRegistrationDto = {
      userName: formValues.userName,
      email: formValues.email,
      password: formValues.password,
      confirmPassword: formValues.confirm,
      mainCurrencyCode: formValues.mainCurrencyCode,
      clientURI: formValues.clientURI
    };

    this.authService.registerUser("api/users-accounts/register", user)
    .subscribe({
      next: (_) => console.log("Successful registration"),
      error: (err: HttpErrorResponse) => console.log(err.error.errors)
    })
}
}
