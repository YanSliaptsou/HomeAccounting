import { HttpErrorResponse } from '@angular/common/http';

import { AuthenticationService } from './../../shared/services/authentication.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ForgotPasswordDto } from 'src/app/_interfaces/ForgotPassword';
@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  forgotPasswordForm: FormGroup
  successMessage: string;
  errorMessage: string;
  showSuccess: boolean;
  showError: boolean;
  
  constructor(private authService: AuthenticationService) { }
  
  ngOnInit(): void {
    this.forgotPasswordForm = new FormGroup({
      email: new FormControl("", [Validators.required])
    })
  }
  public validateControl = (controlName: string) => {
    return this.forgotPasswordForm.get(controlName).invalid && this.forgotPasswordForm.get(controlName).touched
  }
  public hasError = (controlName: string, errorName: string) => {
    return this.forgotPasswordForm.get(controlName).hasError(errorName)
  }
  public forgotPassword = (forgotPasswordFormValue : any) => {
    this.showError = this.showSuccess = false;
    const forgotPass = { ...forgotPasswordFormValue };
    const forgotPassDto: ForgotPasswordDto = {
      email: forgotPass.email,
      clientURI: 'http://localhost:4200/authentication/resetpassword'
    }

    this.authService.forgotPassword('api/users-accounts/forgotpassword', forgotPassDto)
    .subscribe(response => {
      this.showSuccess = true;
      this.successMessage = 'The link has been sent, please check your email to reset your password.'
    }, error => {
      this.showError = true;
      this.errorMessage = error;
    })
  }
  }