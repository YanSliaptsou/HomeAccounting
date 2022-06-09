import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TabsetConfig } from 'ngx-bootstrap/tabs';
import { AccountService } from '../shared/services/account.service';
import { CategoryService } from '../shared/services/category.service';
import { CurrenciesService } from '../shared/services/currencies.service';
import { AccountEditDto } from '../_interfaces/Account/AccountEditDto';
import { AccountReceiveDto } from '../_interfaces/Account/AccountReceiveDto';
import { AccountSendDto } from '../_interfaces/Account/AccountSendDto';
import { CategoryReceive} from '../_interfaces/Category/CategoryReceiveDto';
import { Currency } from '../_interfaces/currency';


@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.css'],
})
export class AccountsComponent implements OnInit {

  constructor(private currencyService : CurrenciesService, private categoryService : CategoryService,
    private accountService : AccountService) { }

    isAdding = true;
    isEditing = false;

    showSuccess = false;
    showError = false;
    errorMessage : string;
    successMessage : string;

    currencies : Currency[];
    categories : CategoryReceive[];
    accounts : AccountReceiveDto[];
    currentAccount : AccountReceiveDto;
    accountForm : FormGroup;

    currentType : string;
    currentCurrency : string;
    currentCategory : number;
    currentAccountName : string;

  ngOnInit(): void {
    this.loadCategories();
    this.loadCurrencies();
    this.loadAccounts('All');

    this.accountForm = new FormGroup({
      type : new FormControl("", [Validators.required]),
      currency : new FormControl("", [Validators.required]),
      name : new FormControl("", [Validators.required]),
      transactionCategory : new FormControl("", [Validators.required])
    })

    this.accountForm.get('currency').disable();
    this.accountForm.get('transactionCategory').disable();
    this.accountForm.get('name').disable();

  }

  loadCurrencies(){
    this.currencyService.getCurrencies("api/currencies")
          .subscribe((response : any) => {
            console.log(response);
            this.currencies = response;
          })
  }

  loadCategories(){
    this.categoryService.getCategories("api/categories/list-except-repeated")
      .subscribe((response : any) => {
        this.categories = response;
        console.log(response);
      })
  }

  loadAccounts(type: string){
    this.accountService.getAccounts("api/accounts/" + type)
      .subscribe((response : any) => {
        this.accounts = response;
        console.log(response);
      })
  }

  getConcreteAccountToEdit(id : number){
    this.accountForm.get('currency').enable();
    this.accountForm.get('transactionCategory').disable();
    this.accountForm.get('name').enable();
    this.accountForm.get('type').disable();

    this.accountService.getConcreteAccount("api/accounts/account-by-id/" + id)
      .subscribe((response : any) => {
        this.currentAccount = response;
        console.log(this.currentAccount);

        this.isEditing = true;
        this.isAdding = false;
        this.showSuccess = true;
        this.showError = false;
        this.successMessage = "Account " + response.name + " selected to edit";

        this.currentType = this.currentAccount.type
        this.currentCurrency = this.currentAccount.currencyId
        this.currentCategory = this.currentAccount.transactionCategoryId
        this.currentAccountName = this.currentAccount.name
      })
    

  }

  addAccount(accoutnF : any){
    const accForm = {... accoutnF}
    const acc : AccountSendDto = {
      type : accForm.type,
      name : accForm.name,
      currencyId : accForm.currency,
      transactionCategoryId : accForm.transactionCategory
    }
    this.accountService.addAccount("api/accounts", acc)
    .subscribe((response : any) => {
      this.loadAccounts("All");
      this.showSuccess = true;
      this.showError = false;
      this.successMessage = "Account " + response.name + " has successfuly added.";
    }, error => {
      this.showSuccess = false;
      this.showError = true;
      this.errorMessage = error;
    })
  }

  editAccount(accForm : any, id: number){
    this.accountForm.get('type').enable();
    this.isEditing = false;
    this.isAdding = true;

    const aForm = {... accForm}

    const account : AccountEditDto = {
      type : aForm.type,
      name : aForm.name,
      currencyId : aForm.currency
    }

    this.accountService.editAccount("api/accounts/" + id,account).subscribe((response : any) => {
      this.loadAccounts("All");
      this.showSuccess = true;
      this.showError = false;
      this.successMessage = "Account " + response.name + " has been successfuly edited.";
    }, error => {
      this.showSuccess = false;
      this.showError = true;
      this.errorMessage = error;
    })
  }

  deleteAccount(id : number){
    this.accountService.deleteAccount("api/accounts/"+id).subscribe((response : any) => {
      this.showSuccess = true;
      this.showError = false;
      this.successMessage = "Account nmb. " + id + " has been deleted successfuly!"
      this.loadAccounts("All")
    }, error => {
      this.showError = true;
        this.showSuccess = false;
        this.errorMessage = error;
    })
  }

  validateControl = (controlName: string) => {
    return this.accountForm.get(controlName).invalid && this.accountForm!.get(controlName).touched
  }

  hasError = (controlName: string, errorName: string) => {
    return this.accountForm.get(controlName).hasError(errorName)
  }

  updateForm(type : string){

    this.accountForm.get('currency').enable();
    this.accountForm.get('name').enable();
    console.log(this.currentType);
    if (type === 'income'){
      this.accountForm.get('transactionCategory').disable();
    }

    else if (type == 'outcome'){
      this.accountForm.get('transactionCategory').enable();
    }
  }

}
