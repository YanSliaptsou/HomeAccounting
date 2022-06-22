import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AccountService } from '../shared/services/account.service';
import { CategoryService } from '../shared/services/category.service';
import { CurrenciesService } from '../shared/services/currencies.service';
import { LimitService } from '../shared/services/limit.service';
import { AccountEditDto } from '../_interfaces/Account/AccountEditDto';
import { AccountReceiveDto } from '../_interfaces/Account/AccountReceiveDto';
import { AccountSendDto } from '../_interfaces/Account/AccountSendDto';
import { CategoryReceive} from '../_interfaces/Category/CategoryReceiveDto';
import { Currency } from '../_interfaces/currency';
import { LimitReceiveDto } from '../_interfaces/Limit/LimitReceiveDto';
import { LimitSendDto } from '../_interfaces/Limit/LimitSendDto';


@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.css'],
})
export class AccountsComponent implements OnInit {

  constructor(private currencyService : CurrenciesService, private categoryService : CategoryService,
    private accountService : AccountService,private limitService : LimitService ,private modalService: BsModalService) { }

    isAdding = true;
    isEditing = false;

    showSuccess = false;
    showError = false;
    errorMessage : string;
    successMessage : string;

    currencies : Currency[];
    categories : CategoryReceive[];
    accounts : AccountReceiveDto[];
    currentAccount : AccountReceiveDto = {
      appUserId : null,
      currencyId : null,
      id : null,
      name : null,
      type : null,
      transactionCategoryId : null
    };
    accountForm : FormGroup;

    currentType : string;
    currentCurrency : string;
    currentCategory : number;
    currentAccountName : string;

    modalRef?: BsModalRef;

    //--------------------------

    isAddingLimit = true;
    isEditingLimit = false;
    showSuccessLimit = false;
    showErrorLimit = false;
    errorMessageLimit : string;
    successMessageLimit : string;
    limits : LimitReceiveDto[];
    currentLimit : LimitReceiveDto;
    modalRefLimit?: BsModalRef;
    limitsForm : FormGroup;

    currentLimitAmmount : number;
    currentDateFrom : string;
    currentDateTo : string;

    isCurrencyDisabled = false;
    isNameDisabled = false;
    isTransactionCategoryDisabled = false;

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

    this.limitsForm = new FormGroup({
      limit : new FormControl("", [Validators.required, Validators.min(0)]),
      dateFrom : new FormControl("", [Validators.required]),
      dateTo : new FormControl("", [Validators.required]),
    })
  }

  openModalOnDelete(template: TemplateRef<any>, id : number) {
    this.modalRef = this.modalService.show(template, {class: 'modal-lg'});
    this.getConcreteAccount(id);
  }

  openModalOnDeleteLimit(template: TemplateRef<any>, id : number){
    this.modalRefLimit = this.modalService.show(template, {class: 'modal-lg'});
    this.getConcreteLimit(id);
  }

  loadLimits(account : AccountReceiveDto){
    account.limitsList = [];
    this.limitService.getLimits("api/limits/" + account.id)
      .subscribe((response : LimitReceiveDto[]) => {
        this.limits = response;
        console.log(this.limits);
        for(let lm of this.limits){
          account.limitsList.push(lm);
        }
      })
  }

  loadCurrencies(){
    this.currencyService.getCurrencies("api/currencies")
          .subscribe((response : any) => {
            this.currencies = response;
          })
  }

  loadCategories(){
    this.categoryService.getCategories("api/categories/list-except-repeated")
      .subscribe((response : any) => {
        this.categories = response.data;
      })
  }

  loadAccounts(type: string){
    this.accountService.getAccounts("api/accounts/" + type)
      .subscribe((response : any) => {
        this.accounts = response.data;
        for(let acc of this.accounts){
          this.loadLimits(acc);
        }
      })
  }

  getConcreteLimitToEdit(id : number){
    this.limitService.getConcreteLimit("api/limits/concrete/" + id)
      .subscribe((response : LimitReceiveDto) => {
        this.currentLimit = response;

        this.isEditingLimit = true;
        this.isAddingLimit = false;
        this.showSuccessLimit = true;
        this.showErrorLimit = false;

        this.successMessageLimit = "Limit " + response.id + " selected to edit";

        this.currentLimitAmmount = this.currentLimit.limit;
        this.currentDateFrom = this.currentLimit.limitFrom.toDateString();
        this.currentDateTo = this.currentLimit.limitTo.toDateString();

      })
  }

  getConcreteAccountToEdit(id : number){
    this.accountForm.get('currency').enable();
    this.accountForm.get('transactionCategory').disable();
    this.accountForm.get('name').enable();
    this.accountForm.get('type').disable();

    this.accountService.getConcreteAccount("api/accounts/account-by-id/" + id)
      .subscribe((response : any) => {
        this.currentAccount = response.data;
        console.log(this.currentAccount);

        this.isEditing = true;
        this.isAdding = false;
        this.showSuccess = true;
        this.showError = false;
        this.successMessage = "Account " + response.data.name + " selected to edit";

        this.currentType = this.currentAccount.type
        this.currentCurrency = this.currentAccount.currencyId
        this.currentCategory = this.currentAccount.transactionCategoryId
        this.currentAccountName = this.currentAccount.name
      })
  }

  getConcreteAccount(id : number){
    this.accountService.getConcreteAccount("api/accounts/account-by-id/" + id)
      .subscribe((response : any) => {
        this.currentAccount = response.data;
      })
  }

  getConcreteLimit(id : number){
    this.limitService.getConcreteLimit("api/limits/concrete/" + id)
    .subscribe((response : LimitReceiveDto) => {
      this.currentLimit = response;
    })
  }

  addLimit(limitForm : any){
    const limForm = {... limitForm}
    const lim : LimitSendDto = {
      limit : limForm.limit,
      limitFrom : limForm.dateFrom,
      limitTo : limForm.dateTo,
      accountId : this.currentAccount.id
    }
    this.limitService.addLimit("api/limits", lim)
      .subscribe((respose : any) => {
        this.loadLimits(this.currentAccount);
        this.showSuccessLimit = true;
        this.showErrorLimit = false;
        this.successMessageLimit = "Limit " + respose.id + " has successfuly added";
        console.log(this.successMessageLimit);
      }, error => {
        this.showSuccessLimit = false;
        this.showErrorLimit = true;
        this.errorMessageLimit = error;
        console.log(this.errorMessageLimit);
      })
  }

  addAccount(accoutnF : any){
    const accForm = {... accoutnF}

    var transCatId : number;
    if (this.categories.length == 1){
      transCatId = this.categories[0].id;
    }
    else{
      transCatId = accForm.transactionCategory;
    }
    
    const acc : AccountSendDto = {
      type : accForm.type,
      name : accForm.name,
      currencyId : accForm.currency,
      transactionCategoryId : transCatId
    }
    this.accountService.addAccount("api/accounts", acc)
    .subscribe((response : any) => {
      this.loadAccounts("All");
      this.loadCategories();
      this.showSuccess = true;
      this.showError = false;
      this.successMessage = "Account nmb." + acc.name + " has successfuly added.";
    }, error => {
      this.showSuccess = false;
      this.showError = true;
      this.errorMessage = error.errorMessage;
    })
  }

  editLimit(limitForm : any, id : number){
    this.isEditingLimit = false;
    this.isAddingLimit = true;

    const limForm = {... limitForm}

    const limit : LimitSendDto = {
      limit : limForm.limit,
      limitFrom : limForm.dateFrom,
      limitTo : limForm.dateTo,
      accountId : this.currentAccount.id
    }

    this.limitService.editLimit("api/limits/" + id, limit)
      .subscribe((response : any) => {
        console.log(this.currentAccount);
        this.loadLimits(this.currentAccount);
        this.showSuccessLimit = true;
        this.showErrorLimit = false;
        this.successMessageLimit = "Limit " + response.id + " has been successfully edited.";
      }, error => {
        this.showSuccessLimit = false;
        this.showErrorLimit = true;
        this.errorMessageLimit = error;
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
      this.successMessage = "Account " + response.data.name + " has been successfuly edited.";
    }, error => {
      this.showSuccess = false;
      this.showError = true;
      this.errorMessage = error.errorMessage;
    })
  }

  deleteLimit(id : number){
    this.limitService.deleteLimit("api/limits/" + id).subscribe((response : any) => {
      this.showSuccessLimit = true;
      this.showErrorLimit = false;
      this.successMessageLimit = "Limit nmb. " + id + " has been deleted successfully";
      this.loadLimits(this.currentAccount);
    }, error => {
      this.showSuccessLimit = false;
      this.showErrorLimit = true;
      this.errorMessageLimit = error;
    })

    this.modalRefLimit?.hide();
  }

  deleteAccount(id : number){
    this.accountService.deleteAccount("api/accounts/"+id).subscribe((response : any) => {
      this.showSuccess = true;
      this.showError = false;
      this.successMessage = "Account nmb. " + id + " has been deleted successfuly!"
      this.loadAccounts("All")
      this.loadCategories();
    }, error => {
      this.showError = true;
        this.showSuccess = false;
        this.errorMessage = error.errorMessage;
    })
    this.modalRef?.hide();
  }

  validateControl = (controlName: string, form : FormGroup) => {
    return form.get(controlName).invalid && form.get(controlName).touched
  }

  hasError = (form : FormGroup, controlName: string, errorName: string) => {
    return form.get(controlName).hasError(errorName)
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

  decline(): void {
    this.modalRef?.hide();
  }

  declineLim() : void {
    this.modalRefLimit?.hide();
  }

}
