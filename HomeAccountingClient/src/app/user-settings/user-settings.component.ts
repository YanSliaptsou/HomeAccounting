import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '../shared/services/authentication.service';
import { CurrenciesService } from '../shared/services/currencies.service';
import { Currency } from '../_interfaces/currency';
import { UserRequestDto } from '../_interfaces/UsersInterfaces/UserRequstDto';
import { UserResponseDto } from '../_interfaces/UsersInterfaces/UserResponseDto';

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.css']
})
export class UserSettingsComponent implements OnInit {

  constructor(private authService : AuthenticationService, private currencyService : CurrenciesService) { }

  settingsForm : FormGroup;
  currencies : Currency[];
  appUser : UserResponseDto;

  ngOnInit(): void {
    this.settingsForm = this.initSettingForm();
    this.getCurrentUser();
    this.getCurrencies();
  }

  getCurrencies(){
    this.currencyService.getCurrencies('api/currencies').subscribe((response : any) => {
      this.currencies = response;
    })
  }

  getCurrentUser(){
    this.authService.getUser().subscribe((response : any) => {
      this.appUser = response.data;
    })
  }

  initSettingForm() : FormGroup{
    var settingsForm = new FormGroup({
      userName : new FormControl("", [Validators.required]),
      currency : new FormControl("", [Validators.required])
    })

    return settingsForm;
  }

  editSettings(form : any){

    const setForm = {... form}

    const settings : UserRequestDto = {
      mainCurrencyId : setForm.currency,
      userName : setForm.userName
    };

    this.authService.editUser(settings).subscribe((response : any) => {
      this.getCurrentUser();
    });
  }

}
