import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { PasswordConfirmationValidatorService } from 'src/app/shared/custom-validators/password-confirmation-validator.service';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { ResetPasswordDto } from 'src/app/_interfaces/ResetPasswordDto';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {

  resetPasswordForm: FormGroup;
  showSuccess: boolean;
  showError: boolean;
  errorMessage: string;
  private token: string;
  private email: string;
  
  constructor(private authService: AuthenticationService, private passConfValidator: PasswordConfirmationValidatorService, 
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.resetPasswordForm = new FormGroup({
      password: new FormControl('', [Validators.required]),
      confirm: new FormControl('')
  });

  this.resetPasswordForm.get('confirm').setValidators([Validators.required,
    this.passConfValidator.validateConfirmPassword(this.resetPasswordForm.get('password'))]);
  
    this.token = this.route.snapshot.queryParams['token'];
    this.email = this.route.snapshot.queryParams['email'];
}

public validateControl = (controlName: string) => {
  return this.resetPasswordForm.get(controlName).invalid && this.resetPasswordForm.get(controlName).touched
}
public hasError = (controlName: string, errorName: string) => {
  return this.resetPasswordForm.get(controlName).hasError(errorName)
}
public resetPassword = (resetPasswordFormValue : any) => {
  this.showError = this.showSuccess = false;
  const resetPass = { ... resetPasswordFormValue };
  const resetPassDto: ResetPasswordDto = {
    password: resetPass.password,
    confirmPassword: resetPass.confirm,
    token: this.token,
    email: this.email
  }

this.authService.resetPassword('api/users-accounts/resetpassword', resetPassDto)
  .subscribe(response => {
    this.showSuccess = true;
  }, error => {
    this.showError = true;
    this.errorMessage = error;
  })

}
}
