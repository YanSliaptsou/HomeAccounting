import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { PasswordConfirmationValidatorService } from 'src/app/shared/custom-validators/password-confirmation-validator.service';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { CurrenciesService } from 'src/app/shared/services/currencies.service';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { Currency } from 'src/app/_interfaces/currency';
import { UserForRegistrationDto } from 'src/app/_interfaces/UserForRegistrationDto';

@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrls: ['./register-user.component.css']
})
export class RegisterUserComponent implements OnInit {

  registerForm : FormGroup;
  public errorMessage: string;
  public showError: boolean;
  currencies: Currency[];

  constructor(private authService: AuthenticationService, 
    private currencyService: CurrenciesService,
    private confirmValService: PasswordConfirmationValidatorService,
    private router: Router) { }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      userName: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required]),
      mainCurrencyCode: new FormControl('', [Validators.required]),
      confirm: new FormControl('', [Validators.required]),
      clientURI: new FormControl('')
    })

    this.registerForm.get('confirm').setValidators([Validators.required, 
      this.confirmValService.validateConfirmPassword(this.registerForm.get('password'))]);

    this.registerForm.controls.password.valueChanges.subscribe(() => {
        this.registerForm.controls.confirm.updateValueAndValidity();
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
    this.showError = false;
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
    .subscribe(response =>{
      this.router.navigate(["/authentication/login"])
    }, error => {
      this.errorMessage = error;
      this.showError = true;
    })
}
}
